using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float targetDistance = 50.0f;            //Distance to crosshair
    public float timeToRecenter = 0.5f;             //Time to recenter shot
    public float timeToRecover = 0.5f;              //Max recoil recovery time
    public float recoilTime = 0.1f;
    public float recoilMaxTilt = 30.0f;
    public float recoilMaxOffsetZ = 1.0f;

    private GameObject weapon;
    private GameObject envelope;
    private WeaponInfo weaponInfo;
    private ProjectileSpawner projSpawner;

    //For accuracy
    private Vector2 aimOffset;
    private Vector2 fireOffset;

    //For recoil
    private Vector2 recoilMin;
    private Vector2 recoilMax;
    private float recoilSet;   //0 to 1
    private float recoilCurrent;
    private float recoilFire;

    //For accuracy / recoil recovery
    private float lastFireTime;

    //Reload
    private bool isReloading;
    private int ammoCount;

    //To do:  Set ammoMax from weapon stats
    private int ammoMax = 10;
    
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
        float recoilAngle = Mathf.Lerp(recoilMin.x, recoilMax.x, recoilCurrent);
        float recoilZ = Mathf.Lerp(recoilMin.y, recoilMax.y, recoilCurrent);

        Vector3 newPos = new Vector3(gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y, recoilZ);

        gameObject.transform.localPosition = newPos;

        if (!aimSet) gameObject.transform.rotation = GetAimRotation();
        Quaternion kickUp = Quaternion.Euler(new Vector3(-recoilAngle, 0, 0));
        gameObject.transform.rotation *= kickUp;
    }

    public void SetLayerRecursive(GameObject target, int layer)
    {
        Debug.Log($"Setting Layer for {target.gameObject}");
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
        recoilSet = 0;
        recoilFire = 0;
        recoilCurrent = 0;
        ammoCount = ammoMax;
        gameObject.transform.rotation = GetAimRotation();
    }

    public void OnFireWeapon()
    {
        //To do: Temporary (remove)
        if (isReloading) { SetReloaded(); return; }

        if (isReloading) return;

        if (weapon != null)
        {
            float accuracyScale = 5.0f;
            float accuracyFactor = (100.0f - Mathf.Min(weaponInfo.accuracy, 100.0f)) / (100.0f * accuracyScale);
            float scaledErrorX = targetDistance * accuracyFactor;
            float scaledErrorY = scaledErrorX;
            Vector2 errorScaleY = new Vector2(0.1f, 0.5f);
            aimOffset = aimOffset + new Vector2(Random.Range(-scaledErrorX, scaledErrorX), Random.Range(-scaledErrorY * errorScaleY.x, scaledErrorY * errorScaleY.y));
            
            gameObject.transform.rotation = GetAimRotation();
            SetRecoil(true);

            lastFireTime = Time.time;
            projSpawner.SpawnProjectile();

            fireOffset = aimOffset;

            float recoilFixedTemp = 20.0f;
            float recoilAmount = Random.Range(0.8f, 1.0f) * (recoilFixedTemp / 100.0f);
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
        //To do:  set reloading "animation"
        isReloading = true;
    }

    void SetReloaded()
    {
        ammoCount = ammoMax;
        isReloading = false;
    }

    public void EquipWeapon(GameObject newWeapon)
    {
        if (weaponInfo != null) weaponInfo.isEquipped = false; ;

        envelope = gameObject.transform.GetChild(0).gameObject;

        weapon = newWeapon;
        envelope.transform.position = gameObject.transform.position;
        weapon.transform.parent = envelope.transform;
        weapon.transform.position = envelope.transform.position - new Vector3(0.0f, -0.5f, -1.2f);
        weaponInfo = weapon.GetComponent<WeaponInfo>();
        projSpawner = weapon.GetComponent<ProjectileSpawner>();
        weaponInfo.isEquipped = true;
        InitCenterAim();

        gameObject.layer = 13;
        SetLayerRecursive(weapon, 13);

        weaponInfo.isAutomatic = true;
    }

    private void TestCreateWeapon()
    {
        WeaponSpawner weaponSpawner = gameObject.GetComponent<WeaponSpawner>();

        EquipWeapon(weaponSpawner.SpawnWeapon());
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (weapon == null) TestCreateWeapon();

        bool aimSet = false;

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
            }
            else
            {
                float f = delta / timeToRecover;

                //To do: Switch to half sinusoid
                recoilCurrent = Mathf.Lerp(recoilFire, recoilSet, f);
            }
        }

        if (recoilCurrent > 0)
        {
            SetRecoil(aimSet);
        }
    }
}
