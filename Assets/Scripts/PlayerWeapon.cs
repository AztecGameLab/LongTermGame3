using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float targetDistance = 50.0f;                    //Distance to crosshair
    public float timeToRecenter = 0.5f;

    private GameObject weapon;
    private WeaponInfo weaponInfo;
    private ProjectileSpawner projSpawner;
    private Vector2 aimOffset;
    private Vector2 fireOffset;
    private float lastFireTime;
    
    private Quaternion GetAimRotation()
    {
        Vector3 shootFrom = gameObject.transform.position;
        Quaternion rotation = gameObject.transform.parent.rotation;
        Vector3 shootAt = gameObject.transform.parent.position + rotation * new Vector3(0, 0, targetDistance);

        Vector3 target = shootAt + new Vector3(aimOffset.x, aimOffset.y, 0.0f);

        return Quaternion.LookRotation(target - shootFrom, Vector3.up);
    }

    private void InitCenterAim()
    {
        lastFireTime = 0;
        fireOffset = Vector2.zero;
        aimOffset = Vector2.zero;
        gameObject.transform.rotation = GetAimRotation();
    }

    public void OnFireWeapon()
    {
        if (weapon != null)
        {
            Debug.Log("Kashingk!");
            Debug.Log(weaponInfo.accuracy);

            float accuracyScale = 5.0f;
            float accuracyFactor = (100.0f - Mathf.Min(weaponInfo.accuracy, 100.0f)) / (100.0f * accuracyScale);
            float scaledErrorX = targetDistance * accuracyFactor;
            float scaledErrorY = scaledErrorX * 0.5f;
            aimOffset = aimOffset + new Vector2(Random.Range(-scaledErrorX, scaledErrorX), Random.Range(-scaledErrorY, scaledErrorY));
            
            gameObject.transform.rotation = GetAimRotation();

            fireOffset = aimOffset;
            lastFireTime = Time.time;
            projSpawner.SpawnProjectile();
        }
    }

    public void EquipWeapon(GameObject newWeapon)
    {
        if (weaponInfo != null) weaponInfo.isEquipped = false; ;

        weapon = newWeapon;
        weapon.transform.parent = gameObject.transform;
        weapon.transform.position = gameObject.transform.position;
        weaponInfo = weapon.GetComponent<WeaponInfo>();
        projSpawner = weapon.GetComponent<ProjectileSpawner>();
        weaponInfo.isEquipped = true;
        InitCenterAim();

        //weaponInfo.isAutomatic = true;
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

        if (aimOffset != Vector2.zero)
        {
            float delta = Time.time - lastFireTime;

            if (delta > timeToRecenter)
            {
                aimOffset = Vector2.zero;
            }
            else
            {
                float factor = 1.0f - ((timeToRecenter - delta) / timeToRecenter);
                factor *= factor;
                aimOffset.x = Mathf.Lerp(fireOffset.x, 0, factor);
                aimOffset.y = Mathf.Lerp(fireOffset.y, 0, factor);
            }

            gameObject.transform.rotation = GetAimRotation();
        }

    }
}
