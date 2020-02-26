using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    private int weaponCount;//keep track of weapons spawned
    [SerializeField]//Can be used to set range of the initial values based on player progress
    private float testProgressModifier = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        weaponCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //use Left Shift to spawn a new weapon for testing right now
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            SpawnWeapon();
        }
    }

    //Spawn new weapon, add weapon info component, and set values according to player progress
    void SpawnWeapon()
    {
        GameObject newWeapon = new GameObject();
        weaponCount++;
        newWeapon.AddComponent<WeaponInfo>();
        newWeapon.GetComponent<WeaponInfo>().weaponName = "Weapon" + weaponCount;
        newWeapon.GetComponent<WeaponInfo>().SetInitialValues(testProgressModifier);
    }
}
