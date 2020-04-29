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
    void Awake()
    {
        recievers = new string[] { "standard", "automatic", "shotgun", "sniper" };
        magazines = new string[] { "standard", "quick", "extended" };
        barrels = new string[] { "standard", "long", "short" };
        stocks = new string[] { "standard", "compact", "heavy" };
    }
    public void SetWeaponValues(WeaponInfo weapon)
    {
        SetReciever(weapon);
        SetBarrel(weapon);
        SetMagazine(weapon);
        SetStock(weapon);
        //weaponBody = (GameObject)Resources.Load("Body1");
    }

    public void SetReciever(WeaponInfo weapon)
    {
        recieverChoice = (int)Random.Range(0.0f, 3.0f);
        choice = recievers[recieverChoice];
        Debug.Log(choice);

        if (recieverChoice == 3) // Sniper
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 1;
            //weapon.muzzleVelocity = 
            weaponReciever = (GameObject)Resources.Load("SniperReciever");
            weapon.reciverType = "Sniper";

        }
        else if (recieverChoice == 1) // Automatic
        {
            weapon.isAutomatic = true;
            weapon.projectileCount = 1;
            weapon.fireRate += weapon.fireRate * 0.5f;
            weaponReciever = (GameObject)Resources.Load("AutomaticReciever");
            weapon.reciverType = "Automatic";
        }
        else if (recieverChoice == 2) // Shotgun
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = Random.Range(4, 16);
            weaponReciever = (GameObject)Resources.Load("ShotgunReciever");
            weapon.reciverType = "Shotgun";
        }
        else // Standard
        {
            weapon.isAutomatic = false;
            weapon.projectileCount = 1;
            weaponReciever = (GameObject)Resources.Load("StandardReciever");
            weapon.reciverType = "Standard";
        }
    }
    public void SetBarrel(WeaponInfo weapon)
    {
        barrelChoice = (int)Random.Range(0.0f, 2.0f);
        choice = barrels[barrelChoice];
        Debug.Log(choice);
        if (weapon.accuracy >= weapon.statLowerBound && weapon.accuracy <= weapon.statUpperBound * 0.33f)//short
        {
            //weapon.accuracy -= weapon.accuracy * 0.20f;
            //weapon.muzzleVelocity -= weapon.muzzleVelocity * 0.20f;
            //weapon.fireRate += weapon.fireRate * 0.30f;
            //weaponBarrel = (GameObject)Resources.Load("Barrel2");
            weaponBarrel = (GameObject)Resources.Load("ShortBarrel");
        }
        else if (weapon.accuracy >= weapon.statUpperBound * 0.66f && weapon.accuracy <= weapon.statUpperBound)//long
        {
            //weapon.accuracy += weapon.accuracy * 0.15f;
            //weapon.muzzleVelocity += weapon.muzzleVelocity * 0.20f;
            //weapon.fireRate -= weapon.fireRate * 0.30f;
            weaponBarrel = (GameObject)Resources.Load("LongBarrel");
        }
        else//standard
        {
            weaponBarrel = (GameObject)Resources.Load("StandardBarrel");
        }
    }

    public void SetMagazine(WeaponInfo weapon)
    {
        if(weapon.ammoSize >= weapon.statUpperBound * 0.66f && weapon.ammoSize <= weapon.statUpperBound) // Extended
        {
            weaponMagazine = (GameObject)Resources.Load("ExtendedMagazine");
            Debug.Log(magazines[2]);
        }

        else if (weapon.ammoSize >= weapon.statLowerBound && weapon.ammoSize <= weapon.statUpperBound * 0.33f) // Quick
        {
            weaponMagazine = (GameObject)Resources.Load("QuickMagazine");
            Debug.Log(magazines[1]);
        }
        else // Standard
        {
            weaponMagazine = (GameObject)Resources.Load("StandardMagazine");
            Debug.Log(magazines[0]);
        }
    }
    public void SetStock(WeaponInfo weapon)
    {

        if (weapon.recoil >= weapon.statUpperBound*0.66f && weapon.recoil <= weapon.statUpperBound) // heavy
        {

            weaponStock = (GameObject)Resources.Load("HeavyStock");
            Debug.Log(stocks[2]);
        }

        else if (weapon.recoil >= weapon.statLowerBound && weapon.recoil <= weapon.statUpperBound*0.33f) // compact
        {

            weaponStock = (GameObject)Resources.Load("CompactStock");
            Debug.Log(stocks[1]);
        }
        else // standard
        {
            
            weaponStock = (GameObject)Resources.Load("StandardStock");
            Debug.Log(stocks[0]);
        }
    }
}
