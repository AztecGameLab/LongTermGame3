using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    const int layerPlayer = 13;

    public float targetDistance = 50.0f;            //Distance to crosshair
    public float timeToRecenter = 0.5f;             //Time to recenter shot
    public float timeToRecover = 0.4f;              //Max recoil recovery time
    public float recoilTime = 0.1f;
    public float recoilMaxTilt = 30.0f;
    public float recoilMaxOffsetZ = 1.5f;

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

    public int GetAmmoCurrent()
    {
        return ammoCount;
    }

    public int GetAmmoMax()
    {
        return spawner.weaponStats.ammoMaxCount;
    }

    public void EquipWeapon(GameObject newWeapon)
    {
        if (weapon != null)
        {
            weapon.GetComponent<WeaponInfo>().isEquipped = false;
            Destroy(weapon);
            weapon = null;
        }
        
        envelope = gameObject.transform.GetChild(0).gameObject;

        weapon = newWeapon;
        weapon.transform.parent = envelope.transform;
        weapon.transform.localPosition = new Vector3(0, 0, 0);
        weapon.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        weapon.transform.localRotation = Quaternion.identity;
        weapon.GetComponent<WeaponInfo>().isEquipped = true;

        Vector3 newPosition = new Vector3(0, 0, weapon.GetComponent<WeaponInfo>().weaponCenter.y * 6.25f - 0.8f);

        Debug.Log($"Weapon Center: {newPosition}");
        envelope.transform.localPosition = newPosition;

        spawner = weapon.GetComponent<ProjectileSpawner>();
        spawner.InitWeaponStats();

        HudCanvas.instance.SetAmmo(ammoCount);

        InitCenterAim();

        gameObject.layer = layerPlayer;
        SetLayerRecursive(weapon, layerPlayer);
    }

    Quaternion GetAimRotation()
    {
        Vector3 shootFrom = gameObject.transform.position;
        Quaternion rotation = gameObject.transform.parent.rotation;
        Vector3 shootAt = gameObject.transform.parent.position + rotation * new Vector3(0, 0, targetDistance);

        Vector3 target = shootAt + new Vector3(aimOffset.x, aimOffset.y, 0.0f);

        return Quaternion.LookRotation(target - shootFrom, Vector3.up);
    }

    void SetRecoil(bool aimSet)
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

    void InitCenterAim()
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

            HudCanvas.instance.SetAmmo(ammoCount);

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

        HudCanvas.instance.SetAmmo(ammoCount);
    }


    private void Start()
    {
    }

    private void OnDestroy()
    {
        ProjectileSpawner.ResetProjectilePool();
    }

    private void FixedUpdate()
    {
        //TODO Temporary fix! (still needed)
        //  Where is the Weapon coming from on Startup being put in the wrong place!  I cannot find the related code...
        if (gameObject.transform.childCount > 1)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;

                if (child.tag == "Weapon")
                {
                    Destroy(child);
                }
            }
        }

    }

    private void Update()
    {
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

                //TODO Switch to quarter sinusoid
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
