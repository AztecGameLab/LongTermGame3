using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTypeInfo : MonoBehaviour
{
    //Temporary list of possible damage types
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

    //Temporary list of possible projectile effects
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
    public EffectType effectType;
    public float[] damageTypeFactor;

    static public void DefaultAmmoType(AmmoTypeInfo info)
    {
        info.baseDamage = 10.0f;
        info.caliber = 0.2f + Random.value * 0.6f;
        info.caliberToLength = 2.0f + 4.0f * Random.value;
        info.effectType = EffectType.None;
    }

    public float GetProjectileDamage(WeaponInfo weaponInfo, DamageType damageType)
    {
        //return (weaponInfo.baseDamage + baseDamage) * damageTypeFactor[damageType];
        return (weaponInfo.muzzleVelocity * caliber * 0.05f + baseDamage) * damageTypeFactor[(int)damageType];
    }

    public void ResetDamageTypeFactor()
    {
        if ((damageTypeFactor == null) || (damageTypeFactor.Length == 0))
        {
            damageTypeFactor = new float[(int)DamageType.DamageTypeCount];
        }

        for (int i = 0; i < (int)DamageType.DamageTypeCount; i++)
        {
            damageTypeFactor[i] = 1.0f;
        }
    }

    public void SetDamageTypeFactor(DamageType damageType, float factor)
    {
        damageTypeFactor[(int)damageType] = factor;
    }

    void Awake()
    {
    }

    void Start()
    {
        ResetDamageTypeFactor();
    }

    void Update()
    {

    }
}
