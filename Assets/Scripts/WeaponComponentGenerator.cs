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
    private int magazineChoice;
    private int stockChoice;
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
        SetReciever(weapon);
        SetBarrel(weapon);
        SetMagazine(weapon);
        SetStock(weapon);
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
        choice = barrels[barrelChoice];
        Debug.Log(choice);
        if (barrelChoice == 0)//standard
        {
            //weapon.
        }
        else if (barrelChoice == 1)//long
        {
            weapon.accuracy += weapon.accuracy * 0.15f;
            weapon.muzzleVelocity += weapon.muzzleVelocity * 0.20f;
            weapon.fireRate -= weapon.fireRate * 0.30f;
        }
        else//short
        {
            weapon.accuracy -= weapon.accuracy * 0.20f;
            weapon.muzzleVelocity -= weapon.muzzleVelocity * 0.20f;
            weapon.fireRate += weapon.fireRate * 0.30f;
        }
    }

    public void SetMagazine(WeaponInfo weapon)
    {
        magazineChoice = (int)Random.Range(0.0f, 2.0f);
        Debug.Log(magazines[magazineChoice]);
        if(magazineChoice == 0)
        {
            
        }

        else if (magazineChoice == 1)
        {
            weapon.ammoSize -= (int)(weapon.ammoSize * 0.15f);
            weapon.reloadSpeed += weapon.reloadSpeed * 0.15f;
        }
        else
        {
            weapon.ammoSize += (int)(weapon.ammoSize * 0.15f);
            weapon.reloadSpeed -= weapon.reloadSpeed * 0.15f;
        }
    }
    public void SetStock(WeaponInfo weapon)
    {
        stockChoice = (int)Random.Range(0.0f, 2.0f);
        Debug.Log(stocks[stockChoice]);
        if (stockChoice == 0)
        {

        }

        else if (stockChoice == 1)
        {
            weapon.recoil += weapon.recoil * 0.20f;
            weapon.accuracy -= weapon.accuracy * 0.15f;
            weapon.weight -= weapon.weight * 0.30f;
            weapon.reloadSpeed += weapon.reloadSpeed * 0.15f;
        }
        else
        {
            weapon.recoil -= weapon.recoil * 0.20f;
            weapon.ammoSize += (int)(weapon.ammoSize * 0.15f);
            weapon.reloadSpeed -= weapon.reloadSpeed * 0.15f;
            weapon.weight += weapon.weight * 0.30f;
        }
    }
}
