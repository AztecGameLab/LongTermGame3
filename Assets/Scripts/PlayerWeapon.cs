using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public WeaponSpawner testWeaponSpawner;

    private GameObject weapon;

    private void TestCreateWeapon()
    {
        weapon = testWeaponSpawner.SpawnWeapon();
        weapon.transform.parent = gameObject.transform;
        weapon.transform.position = gameObject.transform.position;

        WeaponInfo setWeapon = gameObject.GetComponent<WeaponInfo>();
        AmmoTypeInfo setAmmoType = gameObject.GetComponent<AmmoTypeInfo>();

        ProjectileSpawner projSpawn = weapon.GetComponent<ProjectileSpawner>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (weapon == null) TestCreateWeapon();   
    }
}
