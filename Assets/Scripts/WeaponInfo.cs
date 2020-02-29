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
 
    private float testProgressModifier;

    void Start()
    {
        //SetInitialValues(testProgressModifier);
        //Create components to modify weapon values
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayValuesTest();
        }

        if (Input.GetMouseButtonDown(0))//Right Click
        {
            WeaponAttack();
        }
    }

    //Sets the initial values based on a range of the player's progress
    //This can be changed for balancing or even scrapped later if need be, its main purpose is ensuring unique values
    public void SetInitialValues(float progressModifier)
    {
        //Can place an if statement here later to set low end of range based on progressModifier for balancing
        ammoSize = (int)Random.Range(1.0f, progressModifier);
        projectileCount = (int)Random.Range(1.0f, progressModifier);
        fireRate = Random.Range(1.0f, progressModifier);
        reloadSpeed = Random.Range(1.0f, progressModifier);
        muzzleVelocity = Random.Range(1.0f, progressModifier);
        recoil = Random.Range(1.0f, progressModifier);
        accuracy = Random.Range(1.0f, progressModifier);
        effectiveRange = Random.Range(1.0f, progressModifier);
        damageDropoff = Random.Range(1.0f, progressModifier);
        isAutomatic = (Random.value > 0.5f);

    }

    //test methods to show weapon values and test firing
    void DisplayValuesTest()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }

    void WeaponAttack()
    {

        Debug.Log(weaponName + ": Bang!");
    }
}
