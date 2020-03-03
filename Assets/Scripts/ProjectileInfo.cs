using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : MonoBehaviour
{
    public Vector3 startPosition;
    public WeaponInfo weapon;
    public AmmoTypeInfo ammoType;

    void OnTriggerEnter(Collider collision)
    {
        //To do:  If hit enemy: calculate damage from weaponInfo, ammoTypeInfo, and enemy stats
        //  On all collision: set projectile to inactive
        //  'startPosition' is for determining effect of damage falloff
    }
}
