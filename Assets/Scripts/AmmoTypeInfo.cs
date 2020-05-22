using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmmoTypeInfo : MonoBehaviour
{
    //Temporary list of possible damage types (unused)
    public enum DamageType
    {
        Skin,
        Armor,
        Phantasmal,
        Cybernetic,
        Divine,
        //...
        DamageTypeCount
    }

    //Temporary list of possible projectile effects (unused)
    public enum EffectType
    {
        None,
        Flaming,
        Exploding,
        Bouncing,
        Homing,
        Randomized,
    }

    public float baseDamage;
    public float caliber;
    public float caliberToLength;

    public AudioClip soundOnFire;
    public AudioClip soundOnHitWall;
    public AudioClip soundOnHitEnemy;
    public AudioClip soundOnReload;
    private AudioSource sfxSource;

    static public void DefaultAmmoType(AmmoTypeInfo info)
    {
        info.baseDamage = 10.0f;
        info.caliber = 0.2f + Random.value * 0.6f;
        info.caliberToLength = 2.0f + 4.0f * Random.value;
    }

    public float GetProjectileDamage(WeaponInfo weaponInfo, DamageType damageType)
    {
        //return (weaponInfo.baseDamage + baseDamage) * damageTypeFactor[damageType];
        return (weaponInfo.muzzleVelocity * caliber * 0.05f + baseDamage);
    }

    void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
    }

    void Start()
    {
    }

    void Update()
    {

    }
}
