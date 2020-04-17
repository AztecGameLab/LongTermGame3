using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float targetDistance = 50.0f;            //Distance to crosshair
    public float timeToRecenter = 0.5f;             //Time to recenter shot
    public float timeToRecover = 0.4f;              //Max recoil recovery time
    public float recoilTime = 0.1f;
    public float recoilMaxTilt = 30.0f;
    public float recoilMaxOffsetZ = 1.5f;

    //Temporary, should come from weapon game object
    public ProjectileInfo.Type ammoType = ProjectileInfo.Type.Shotgun;

    private GameObject weapon;
    private GameObject envelope;
    private ProjectileSpawner spawner;

    //For accuracy
    private Vector2 aimOffset;
    private Vector2 fireOffset;

    //For recoil
    private Vector2 recoilMin;
    private Vector2 recoilMax;
    private Vector2 reloadMax;
    private float recoilSet;   //0 to 1
    private float recoilCurrent;
    private float reloadCurrent;
    private float reloadFire;
    private float recoilFire;

    //For accuracy / recoil recovery
    private float lastFireTime;

    //Reload
    private bool isReloading;
    private bool isCharging;
    private float reloadStartTime;

    private int ammoCount;

    public void EquipWeapon(GameObject newWeapon)
    {
        if (weapon != null) weapon.GetComponent<WeaponInfo>().isEquipped = false;

        envelope = gameObject.transform.GetChild(0).gameObject;

        weapon = newWeapon;
        weapon.transform.parent = envelope.transform;
        weapon.transform.localPosition = new Vector3(0, 0, 0);
        weapon.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        weapon.GetComponent<WeaponInfo>().isEquipped = true;
        spawner = weapon.GetComponent<ProjectileSpawner>();

        spawner.InitWeaponStats(ammoType);

        InitCenterAim();

        gameObject.layer = 13;
        SetLayerRecursive(weapon, 13);
    }

    private Quaternion GetAimRotation()
    {
        Vector3 shootFrom = gameObject.transform.position;
        Quaternion rotation = gameObject.transform.parent.rotation;
        Vector3 shootAt = gameObject.transform.parent.position + rotation * new Vector3(0, 0, targetDistance);

        Vector3 target = shootAt + new Vector3(aimOffset.x, aimOffset.y, 0.0f);

        return Quaternion.LookRotation(target - shootFrom, Vector3.up);
    }

    private void SetRecoil(bool aimSet)
    {
        float recoilAngle = 0;
        float reloadAngle = 0;
        float recoilZ = 0;
        float reloadZ = 0;

        if (recoilCurrent > 0)
        {
            recoilAngle = Mathf.Lerp(recoilMin.x, recoilMax.x, recoilCurrent);
            recoilZ = Mathf.Lerp(recoilMin.y, recoilMax.y, recoilCurrent);
        }

        if (reloadCurrent > 0)
        {
            reloadAngle = Mathf.Lerp(recoilMin.x, reloadMax.x, reloadCurrent);
            reloadZ = Mathf.Lerp(recoilMin.y, reloadMax.y, reloadCurrent);
        }

        //If tilting down to reload, start reload animation after recoil
        //  'reloadMaxOffsetZ' should be 0 for this scenario
        if (spawner.weaponStats.reloadMaxTilt < 0)
        {
            recoilAngle = (recoilCurrent > 0) ? Mathf.Max(recoilAngle, reloadAngle) : Mathf.Min(recoilAngle, reloadAngle);
        }
        else
        {
            recoilAngle = Mathf.Max(recoilAngle, reloadAngle);
        }

        recoilZ = Mathf.Min(recoilZ, reloadZ);

        Vector3 newPos = new Vector3(gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y, recoilZ);

        gameObject.transform.localPosition = newPos;

        if (!aimSet) gameObject.transform.rotation = GetAimRotation();
        Quaternion kickUp = Quaternion.Euler(new Vector3(-recoilAngle, 0, 0));

        gameObject.transform.rotation *= kickUp;
    }

    public void SetLayerRecursive(GameObject target, int layer)
    {
        target.layer = layer;

        foreach (Transform t in target.transform)
        {
            SetLayerRecursive(t.gameObject, layer);
        }
    }

    private void InitCenterAim()
    {
        lastFireTime = 0;
        fireOffset = Vector2.zero;
        aimOffset = Vector2.zero;
        recoilMin = new Vector2(0, gameObject.transform.localPosition.z);
        recoilMax = recoilMin + new Vector2(recoilMaxTilt, -recoilMaxOffsetZ);
        reloadMax = recoilMin + new Vector2(spawner.weaponStats.reloadMaxTilt, -spawner.weaponStats.reloadMaxOffsetZ);
        recoilSet = 0;
        recoilFire = 0;
        reloadFire = 0;
        recoilCurrent = 0;
        reloadCurrent = 0;
        ammoCount = spawner.weaponStats.ammoMaxCount;
        gameObject.transform.rotation = GetAimRotation();
    }

    public void OnFireWeapon()
    {
        if (isReloading) return;

        if (weapon != null)
        {
            float accuracyFactor = spawner.weaponStats.aimDrift;
            float scaledErrorX = targetDistance * accuracyFactor;
            float scaledErrorY = scaledErrorX;
            Vector2 errorScaleY = new Vector2(0.1f, 0.5f);
            aimOffset = aimOffset + new Vector2(Random.Range(-scaledErrorX, scaledErrorX), Random.Range(-scaledErrorY * errorScaleY.x, scaledErrorY * errorScaleY.y));

            gameObject.transform.rotation = GetAimRotation();
            SetRecoil(true);

            lastFireTime = Time.time;
            spawner.SpawnProjectile();

            fireOffset = aimOffset;

            float recoilAmount = Random.Range(0.8f, 1.0f) * spawner.weaponStats.shotRecoil;
            recoilFire = recoilCurrent;
            recoilSet = Mathf.Min(recoilCurrent + recoilAmount, 1.0f);

            ammoCount--;

            if (ammoCount == 0)
            {
                SetReloading();
            }
        }
    }

    void SetReloading()
    {
        //To do:  Update for full arm movement when gun center is fixed
        isReloading = true;
        reloadStartTime = Time.time;
    }

    void SetCharging()
    {
        isCharging = true;

        //Play sound
        spawner.StartPlayRecharge();
    }

    void SetReloaded()
    {
        ammoCount = spawner.weaponStats.ammoMaxCount;
        reloadCurrent = 0;
        isReloading = false;
        isCharging = false;

        //Stop sound
        spawner.StopPlayRecharge();
    }

    void TestCreateWeapon()
    {
        WeaponSpawner weaponSpawner = gameObject.GetComponent<WeaponSpawner>();

        StartCoroutine(TestEquipWeapon(weaponSpawner.SpawnWeapon()));
    }

    IEnumerator TestEquipWeapon(GameObject weapon)
    {
        yield return 0;

        EquipWeapon(weapon);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (weapon == null) TestCreateWeapon();

        bool aimSet = false;
        bool needSetRecoil = false;

        float delta = Time.time - lastFireTime;

        if (aimOffset != Vector2.zero)
        {
            if (delta > timeToRecenter)
            {
                aimOffset = Vector2.zero;
            }
            else
            {
                float factor = 1.0f - ((timeToRecenter - delta) / timeToRecenter);
                factor *= factor;
                aimOffset.x = Mathf.LerpAngle(fireOffset.x, 0, factor);
                aimOffset.y = Mathf.Lerp(fireOffset.y, 0, factor);
            }

            gameObject.transform.rotation = GetAimRotation();
            aimSet = true;
        }

        if (recoilSet > 0)
        {
            //Recoil increasing
            if (delta >= recoilTime)
            {
                recoilCurrent = recoilSet;
                recoilFire = recoilSet;
                recoilSet = 0;
            }
            else
            {
                float f = delta / recoilTime;

                //To do: Switch to quarter sinusoid
                recoilCurrent = Mathf.Lerp(recoilFire, recoilSet, f);
            }

            needSetRecoil = true;
        }
        else if (recoilCurrent > 0)
        {
            //Recoil recovering
            delta -= recoilTime;

            if (delta >= timeToRecover)
            {
                recoilCurrent = 0;
                recoilFire = 0;
                recoilSet = 0;

                if (spawner.weaponStats.reloadMaxTilt < 0)
                {
                    //If tilting down to reload, start reload animation after recoil
                    //  'reloadMaxOffsetZ' should be 0 for this scenario
                    reloadStartTime = Time.time;
                }
            }
            else
            {
                float f = delta / timeToRecover;

                //To do: Switch to half sinusoid
                recoilCurrent = Mathf.Lerp(recoilFire, recoilSet, f);
            }

            needSetRecoil = true;
        }

        if (isReloading)
        {
            delta = Time.time - reloadStartTime;

            if (delta > spawner.weaponStats.reloadSeconds)
            {
                SetReloaded();
            }
            else
            {
                float frame = delta / spawner.weaponStats.reloadSeconds;

                if (frame < spawner.weaponStats.reloadGunLowerEnd)
                {
                    reloadCurrent = frame / spawner.weaponStats.reloadGunLowerEnd;

                }
                else if (frame < spawner.weaponStats.reloadGunRaiseEnd)
                {
                    if (!isCharging)
                    {
                        SetCharging();
                    }

                    if (frame >= spawner.weaponStats.reloadArmReturnStart)
                    {
                        reloadCurrent = (spawner.weaponStats.reloadGunRaiseEnd - frame) / (spawner.weaponStats.reloadGunRaiseEnd - spawner.weaponStats.reloadArmReturnStart);
                    }
                }
                else
                {
                    reloadCurrent = 0;
                }
            }

            needSetRecoil = true;
        }

        if (needSetRecoil)
        {
            SetRecoil(aimSet);
        }
    }
}
