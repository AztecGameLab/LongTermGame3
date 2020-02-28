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
    public EffectType effectType;
    public float[] damageTypeFactor;

    public float GetProjectileDamage(WeaponInfo weaponInfo, DamageType damageType)
    {
        //return (weaponInfo.baseDamage + baseDamage) * damageTypeFactor[damageType];
        return 0;
    }

    public void SetDamageTypeFactor(DamageType damageType, float factor)
    {
        damageTypeFactor[(int)damageType] = factor;
    }

    void Awake()
    {
        if (damageTypeFactor == null)
        {
            damageTypeFactor = new float[(int)DamageType.DamageTypeCount];
        }

        for (int i = 0; i < (int)DamageType.DamageTypeCount; i++)
        {
            damageTypeFactor[i] = 1.0f;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
