using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject newReciever;
    public GameObject newBarrel;
    public GameObject newStock;
    public GameObject newMagazine;
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
        weaponCount++;
        GameObject newWeapon = new GameObject();
        //GameObject newWeapon = (GameObject)Instantiate(Resources.Load("TestReciever"));
        GameObject reciever = Instantiate(newReciever, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject barrel = Instantiate(newBarrel, reciever.transform.GetChild(0).transform.position, Quaternion.identity);
        GameObject magazine = Instantiate(newMagazine, reciever.transform.GetChild(1).transform.position, Quaternion.identity);
        GameObject stock = Instantiate(newStock, reciever.transform.GetChild(2).transform.position, Quaternion.identity);
        newWeapon.AddComponent<WeaponInfo>();
        newWeapon.name = "Weapon" + weaponCount;
        newWeapon.GetComponent<WeaponInfo>().weaponName = newWeapon.name;
        newWeapon.GetComponent<WeaponInfo>().SetInitialValues(testProgressModifier);
        reciever.transform.SetParent(newWeapon.transform);
        barrel.transform.parent = reciever.transform.GetChild(0);
        magazine.transform.parent = reciever.transform.GetChild(1);
        stock.transform.parent = reciever.transform.GetChild(2);
        newWeapon.AddComponent<MeshCollider>();
        newWeapon.AddComponent<Rigidbody>();
        //barrel.transform.SetParent(reciever.transform.GetChild(0).transform);
        //Instantiate(newBarrel, new Vector3(0, 0, 0), Quaternion.identity);
        //GameObject barrelSlot = newWeapon.transform.GetChild(0).gameObject;
        //barrelSlot = newBarrel;
    }
}
