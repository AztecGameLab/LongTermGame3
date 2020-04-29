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
    public float statUpperBound;
    public float statLowerBound;
    public Vector3 weaponCenter;
    public string reciverType;
    public GameObject bullet;  //Used for now if no ProjectileSpawner

    private ProjectileSpawner projectileSpawner;

    void Start()
    {
        bullet = (GameObject)Resources.Load<GameObject>("Bullet");
        projectileSpawner = gameObject.GetComponent<ProjectileSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayValuesTest();
        }

        if (isEquipped && isAutomatic)
        {
            if (Input.GetMouseButton(0))
            {
                if (projectileSpawner != null)
                {
                    projectileSpawner.OnWeaponTrigger();
                }
                else
                {
                    //WeaponAttack();                    
                }
            }
        }
        
        else if (isEquipped && !isAutomatic)
        {
            if (Input.GetMouseButtonDown(0)) //Left Click
            {
                if (projectileSpawner != null)
                {
                    projectileSpawner.OnWeaponTrigger();
                }
                else
                {
                    //WeaponAttack();
                }
            }
        }
        
    }

    //Sets the initial values based on a range of the player's progress
    //This can be changed for balancing or even scrapped later if need be, its main purpose is ensuring unique values
    public void SetInitialValues(float progressModifier, float statGap)
    {
        if(statGap > progressModifier)
        {
            statGap = progressModifier;
        }
        statUpperBound = progressModifier;
        statLowerBound = progressModifier - statGap;
        ammoSize = (int)Random.Range(progressModifier-statGap, progressModifier);
        projectileCount = (int)Random.Range(progressModifier - statGap, progressModifier);
        if(projectileCount == 0)
        {
            projectileCount = 1;
        }
        fireRate = Random.Range(progressModifier - statGap, progressModifier);
        reloadSpeed = Random.Range(progressModifier - statGap, progressModifier);
        muzzleVelocity = Random.Range(progressModifier - statGap, progressModifier);
        recoil = Random.Range(progressModifier - statGap, progressModifier);
        accuracy = Random.Range(progressModifier - statGap, progressModifier);
        effectiveRange = Random.Range(progressModifier - statGap, progressModifier);
        damageDropoff = Random.Range(progressModifier - statGap, progressModifier);
        isAutomatic = (Random.value > 0.5f);
        weight = Random.Range(progressModifier - statGap, progressModifier);
        effectiveRange = Random.Range(progressModifier - statGap, progressModifier);
    }

    //test methods to show weapon values and test firing
    void DisplayValuesTest()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
}
