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
    
    private int weaponCount;//keep track of weapons spawned
    [SerializeField]//Can be used to set range of the initial values based on player progress
    private float testProgressModifier = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        weaponCount = 0;
        compGen = gameObject.AddComponent<WeaponComponentGenerator>();
        //GameObject gun = SpawnWeapon();
        
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
    GameObject SpawnWeapon()
    {
        weaponCount++;
        GameObject newWeapon = new GameObject();
        //GameObject reciever = Instantiate(newReciever, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject body = Instantiate(weaponBody, new Vector3(0, 10, 0), Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject barrel = Instantiate(newBarrel, body.transform.GetChild(0).transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject magazine = Instantiate(newMagazine, body.transform.GetChild(1).transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject stock = Instantiate(newStock, body.transform.GetChild(2).transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        GameObject reciever = Instantiate(newReciever, body.transform.GetChild(3).transform.position, Quaternion.Euler(new Vector3(0, -90.0f, 0)));
        newWeapon.AddComponent<WeaponInfo>();
        newWeapon.name = "Weapon" + weaponCount;
        newWeapon.GetComponent<WeaponInfo>().weaponName = newWeapon.name;
        newWeapon.GetComponent<WeaponInfo>().SetInitialValues(testProgressModifier);
        //reciever.transform.SetParent(newWeapon.transform);
        body.transform.SetParent(newWeapon.transform);
        barrel.transform.parent = body.transform.GetChild(0);
        magazine.transform.parent = body.transform.GetChild(1);
        stock.transform.parent = body.transform.GetChild(2);
        reciever.transform.parent = body.transform.GetChild(3);
        newWeapon.AddComponent<MeshCollider>();
        //newWeapon.GetComponent<MeshCollider>().convex = true;
        newWeapon.GetComponent<MeshCollider>().isTrigger = true;
        //newWeapon.AddComponent<Rigidbody>();
        compGen.SetWeaponValues(newWeapon.GetComponent<WeaponInfo>());
        newWeapon.AddComponent<ProjectileSpawner>();
        newWeapon.GetComponent<ProjectileSpawner>().InitializeThis();

        return newWeapon;
    }
}
