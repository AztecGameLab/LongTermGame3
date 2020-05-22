using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with TestPlayer");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with TestPlayer");
    }

    private void Update()
    {
        HandleInputSetProjectile();
    }

    void OnPickup(GameObject weaponSpawner)
    {
        GameObject newWeapon = weaponSpawner.transform.GetChild(0).gameObject;
        GameObject playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon");

        Debug.Log(newWeapon);
        Debug.Log(playerWeapon);


        playerWeapon.GetComponent<PlayerWeapon>().EquipWeapon(newWeapon);

    }

    private void Awake()
    {
    }

    void HandleInputSetProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayerWeaponAmmo(ProjectileInfo.Type.Standard);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerWeaponAmmo(ProjectileInfo.Type.Shotgun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayerWeaponAmmo(ProjectileInfo.Type.Assault);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetPlayerWeaponAmmo(ProjectileInfo.Type.Sniper);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //Force collision
            GameObject pickup = GameObject.FindGameObjectsWithTag("Weapon")[0];

            OnPickup(pickup);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            //Force collision
            GameObject pickup = GameObject.FindGameObjectsWithTag("Weapon")[1];

            OnPickup(pickup);
        }
    }

    void SetPlayerWeaponAmmo(ProjectileInfo.Type ammoType)
    {
        GameObject weapon = GameObject.FindGameObjectWithTag("PlayerWeapon");

        weapon.GetComponent<PlayerWeapon>().SetProjectile(ammoType);
    }

    void TestCreateWeapon()
    {
        WeaponSpawner weaponSpawner = gameObject.GetComponent<WeaponSpawner>();

        StartCoroutine(TestEquipWeapon(weaponSpawner.SpawnWeapon()));
    }

    IEnumerator TestEquipWeapon(GameObject weapon)
    {
        yield return 0;

        //EquipWeapon(weapon);
    }
}
