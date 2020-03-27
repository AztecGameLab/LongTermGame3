﻿using System.Collections;
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
    public GameObject weaponBody;
    public GameObject weaponReciever;
    public GameObject weaponBarrel;
    public GameObject weaponMagazine;
    public GameObject weaponStock;
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
        weaponBody = (GameObject)Resources.Load("Body1");
    }

    public void SetReciever(WeaponInfo weapon)
    {
        recieverChoice = (int)Random.Range(0.0f, 3.0f);
        choice = recievers[recieverChoice];
        Debug.Log(choice);

        if (recieverChoice == 0) // Standard
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 1;
            weaponReciever = (GameObject)Resources.Load("Reciever1");
            //weaponReciever = (GameObject)Resources.Load("Reciever2");
        }
        else if (recieverChoice == 1) // Automatic
        {
            weapon.isAutomatic = true;
            weapon.projectileCount = 1;
            weapon.fireRate += weapon.fireRate * 0.5f;
            weaponReciever = (GameObject)Resources.Load("Reciever2");
            //weaponReciever = (GameObject)Resources.Load("Reciever1");
        }
        else if (recieverChoice == 2) // Shotgun
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 4 * (int)weapon.testProgressModifier;
            //weaponReciever = (GameObject)Resources.Load("Reciever2");
            weaponReciever = (GameObject)Resources.Load("Reciever1");
        }
        else // Sniper
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 1;
            //weaponReciever = (GameObject)Resources.Load("Reciever2");
            weaponReciever = (GameObject)Resources.Load("Reciever1");
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
            //weaponBarrel = (GameObject)Resources.Load("Barrel2");
            weaponBarrel = (GameObject)Resources.Load("Barrel1");
        }
        else if (barrelChoice == 1)//long
        {
            weapon.accuracy += weapon.accuracy * 0.15f;
            weapon.muzzleVelocity += weapon.muzzleVelocity * 0.20f;
            weapon.fireRate -= weapon.fireRate * 0.30f;
            weaponBarrel = (GameObject)Resources.Load("Barrel2");
            //weaponBarrel = (GameObject)Resources.Load("Barrel1");
        }
        else//short
        {
            weapon.accuracy -= weapon.accuracy * 0.20f;
            weapon.muzzleVelocity -= weapon.muzzleVelocity * 0.20f;
            weapon.fireRate += weapon.fireRate * 0.30f;
            //weaponBarrel = (GameObject)Resources.Load("Barrel2");
            weaponBarrel = (GameObject)Resources.Load("Barrel1");
        }
    }

    public void SetMagazine(WeaponInfo weapon)
    {
        magazineChoice = (int)Random.Range(0.0f, 2.0f);
        Debug.Log(magazines[magazineChoice]);
        if(magazineChoice == 0) // Standard
        {
            //weaponMagazine = (GameObject)Resources.Load("Magazine2");
            weaponMagazine = (GameObject)Resources.Load("Magazine1");
        }

        else if (magazineChoice == 1) // Quick
        {
            weapon.ammoSize -= (int)(weapon.ammoSize * 0.15f);
            weapon.reloadSpeed += weapon.reloadSpeed * 0.15f;
            //weaponMagazine = (GameObject)Resources.Load("Magazine2");
            weaponMagazine = (GameObject)Resources.Load("Magazine1");
        }
        else // Extended
        {
            weapon.ammoSize += (int)(weapon.ammoSize * 0.15f);
            weapon.reloadSpeed -= weapon.reloadSpeed * 0.15f;
            weaponMagazine = (GameObject)Resources.Load("Magazine2");
            //weaponMagazine = (GameObject)Resources.Load("Magazine1");
        }
    }
    public void SetStock(WeaponInfo weapon)
    {
        stockChoice = (int)Random.Range(0.0f, 2.0f);
        Debug.Log(stocks[stockChoice]);
        if (stockChoice == 0) // standard
        {
            //weaponStock = (GameObject)Resources.Load("Stock2");
            weaponStock = (GameObject)Resources.Load("Stock1");
        }

        else if (stockChoice == 1) // compact
        {
            weapon.recoil += weapon.recoil * 0.20f;
            weapon.accuracy -= weapon.accuracy * 0.15f;
            weapon.weight -= weapon.weight * 0.30f;
            weapon.reloadSpeed += weapon.reloadSpeed * 0.15f;
            //weaponStock = (GameObject)Resources.Load("Stock2");
            weaponStock = (GameObject)Resources.Load("Stock1");
        }
        else // heavy
        {
            weapon.recoil -= weapon.recoil * 0.20f;
            weapon.ammoSize += (int)(weapon.ammoSize * 0.15f);
            weapon.reloadSpeed -= weapon.reloadSpeed * 0.15f;
            weapon.weight += weapon.weight * 0.30f;
            weaponStock = (GameObject)Resources.Load("Stock2");
            //weaponStock = (GameObject)Resources.Load("Stock1");
        }
    }
}
