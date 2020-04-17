using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public WeaponComponentGenerator compGen;
    public GameObject weaponBody;
    public GameObject newReciever;
    public GameObject newBarrel;
    public GameObject newStock;
    public GameObject newMagazine;
    public bool testSpawnOnShift = true;
    public bool spawnOnAwake = true;
    
    private int weaponCount;//keep track of weapons spawned
    [SerializeField]//Can be used to set range of the initial values based on player progress
    [Range(30.0f, 100f)]
    public float testProgressModifier;
    [Range(0.0f, 50f)]
    public float statGap;

    void Start()
    {
       
        
    }
    void Awake()
    {
        weaponCount = 0;
        compGen = gameObject.AddComponent<WeaponComponentGenerator>();

        //Needed for now so ProjectileTestScene can spawn from PlayerWeapon
        if (spawnOnAwake)
        {
            GameObject weapon = SpawnWeapon();
        }
    }
    // Update is called once per frame
    void Update()
    {
        //use Left Shift to spawn a new weapon for testing right now
        //DB:  added 'testSpawnOnShift' to disable this test where needed
        //  Also setting isEquipped by default so I don't break the test scene
        //if (testSpawnOnShift && Input.GetKeyDown(KeyCode.LeftShift)){
        //    GameObject weapon = SpawnWeapon();
        //    weapon.GetComponent<WeaponInfo>().isEquipped = true;
        //}
    }

    //Spawn new weapon, add weapon info component, and set values according to player progress
    public GameObject SpawnWeapon()
    {
        weaponCount++;
        GameObject newWeapon = new GameObject();
        newWeapon.transform.SetParent(this.transform);
        newWeapon.AddComponent<WeaponInfo>();
        newWeapon.name = "Weapon" + weaponCount;
        newWeapon.GetComponent<WeaponInfo>().weaponName = newWeapon.name;
        newWeapon.GetComponent<WeaponInfo>().SetInitialValues(testProgressModifier, statGap);
        compGen.SetWeaponValues(newWeapon.GetComponent<WeaponInfo>());
        newReciever = compGen.weaponReciever;
        newBarrel = compGen.weaponBarrel;
        newMagazine = compGen.weaponMagazine;
        newStock = compGen.weaponStock;
        GameObject reciever = Instantiate(newReciever, transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject barrel = Instantiate(newBarrel, reciever.transform.GetChild(1).transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject magazine = Instantiate(newMagazine, reciever.transform.GetChild(2).transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject stock = Instantiate(newStock, reciever.transform.GetChild(3).transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        
        reciever.transform.SetParent(newWeapon.transform);
        barrel.transform.parent = reciever.transform.GetChild(1);
        magazine.transform.parent = reciever.transform.GetChild(2);
        stock.transform.parent = reciever.transform.GetChild(3);
        
        newWeapon.AddComponent<MeshCollider>();
        newWeapon.GetComponent<MeshCollider>().convex = true;
        newWeapon.GetComponent<MeshCollider>().isTrigger = true;
        newWeapon.AddComponent<ProjectileSpawner>().enabled = true;
        newWeapon.AddComponent<AudioSource>();
        newWeapon.layer = 10;
        //newWeapon.tag = "Weapon";

        return newWeapon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12) {
            GetComponentInChildren<WeaponInfo>().isEquipped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            GetComponentInChildren<WeaponInfo>().isEquipped = false;
        }
    }
}
