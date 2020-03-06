using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponentGenerator : MonoBehaviour
{
    public string[] recievers;
    public string[] magazines;
    public string[] barrels;
    public string[] stocks;
    private int recieverChoice;
    private int barrelChoice;
    private string choice;
    // Start is called before the first frame update
    void Start()
    {
        recievers = new string[] { "standard", "automatic", "shotgun", "sniper" };
        magazines = new string[] { "standard", "quick", "extended" };
        barrels = new string[] { "standard", "long", "short" };
        stocks = new string[] { "standard", "compact", "heavy" };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWeaponValues(WeaponInfo weapon)
    {
        weapon.ammoSize = 20;
        weapon.muzzleVelocity *= 100.0f;
        SetReciever(weapon);
        SetBarrel(weapon);
    }

    public void SetReciever(WeaponInfo weapon)
    {
        recieverChoice = (int)Random.Range(0.0f, 3.0f);
        choice = recievers[recieverChoice];
        Debug.Log(choice);

        if (recieverChoice == 0)
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 1;
        }
        else if (recieverChoice == 1)
        {
            weapon.isAutomatic = true;
            weapon.projectileCount = 1;
            weapon.fireRate += weapon.fireRate * 0.5f;
        }
        else if (recieverChoice == 2)
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 4 * (int)weapon.testProgressModifier;
        }
        else
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 1;
        }
    }
    public void SetBarrel(WeaponInfo weapon)
    {
        barrelChoice = (int)Random.Range(0.0f, 2.0f);
        choice = recievers[recieverChoice];
        Debug.Log(choice);
        if(barrelChoice == 0)
        {
            //weapon.
        }
    }
}
