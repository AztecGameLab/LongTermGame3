using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponInfo : MonoBehaviour
{
    //weapon values, more can be added later
    //also kept them public for use with JSON utility, can be changed to private later and use getters/setters
    public string weaponName;
    public int ammoSize;
    public bool isAutomatic;
    public bool isEquipped;
    public int projectileCount;
    public float fireRate;
    public float reloadSpeed;
    public float muzzleVelocity;
    public float recoil;
    public float accuracy;
    public float effectiveRange;
    public float damageDropoff;
    public float weight;
 
    public float testProgressModifier;

    public GameObject bullet;
    void Start()
    {
        bullet = (GameObject)Resources.Load<GameObject>("Bullet");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayValuesTest();
        }

        if (isAutomatic)
        {
            if (Input.GetMouseButton(0))
            {
                WeaponAttack();
            }
        }
        
        else if (!isAutomatic)
        {
            if (Input.GetMouseButtonDown(0))//Left Click
            {
                WeaponAttack();
            }
        }
        
    }

    //Sets the initial values based on a range of the player's progress
    //This can be changed for balancing or even scrapped later if need be, its main purpose is ensuring unique values
    public void SetInitialValues(float progressModifier)
    {
        //Can place an if statement here later to set low end of range based on progressModifier for balancing
        ammoSize = (int)Random.Range(20.0f, progressModifier);
        projectileCount = (int)Random.Range(20.0f, progressModifier);
        fireRate = Random.Range(20.0f, progressModifier);
        reloadSpeed = Random.Range(20.0f, progressModifier);
        muzzleVelocity = Random.Range(20.0f, progressModifier);
        recoil = Random.Range(20.0f, progressModifier);
        accuracy = Random.Range(20.0f, progressModifier);
        effectiveRange = Random.Range(20.0f, progressModifier);
        damageDropoff = Random.Range(20.0f, progressModifier);
        isAutomatic = (Random.value > 0.5f);
        weight = Random.Range(20.0f, progressModifier);
        testProgressModifier = progressModifier;
    }

    //test methods to show weapon values and test firing
    void DisplayValuesTest()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }

    void WeaponAttack()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject newBullet = Instantiate(bullet, transform.GetChild(0).transform.GetChild(0).transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 2);
            int axis = (int)Random.Range(0.0f, 2.0f);
            if (axis == 0)
            {
                newBullet.GetComponent<Rigidbody>().velocity = new Vector3(newBullet.GetComponent<Rigidbody>().velocity.x * (Random.Range(1.0f, accuracy)), newBullet.GetComponent<Rigidbody>().velocity.y, newBullet.GetComponent<Rigidbody>().velocity.z);
            }
            else if (axis == 1)
            {
                newBullet.GetComponent<Rigidbody>().velocity = new Vector3(newBullet.GetComponent<Rigidbody>().velocity.x , newBullet.GetComponent<Rigidbody>().velocity.y * (Random.Range(1.0f, accuracy)), newBullet.GetComponent<Rigidbody>().velocity.z);
            }
            else
            {
                newBullet.GetComponent<Rigidbody>().velocity = new Vector3(newBullet.GetComponent<Rigidbody>().velocity.x, newBullet.GetComponent<Rigidbody>().velocity.y, newBullet.GetComponent<Rigidbody>().velocity.z * (Random.Range(1.0f, accuracy)));
            }
        }
        Debug.Log(weaponName + ": Bang!");
    }
}
