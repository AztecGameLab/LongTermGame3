using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSniper : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 1, 8);
        stats.SetAimDrift(info.accuracy, 5.0f, 0.0f);
        stats.SetRPM(info.fireRate, 20.0f, 80.0f);
        stats.SetShotRecoil(info.recoil, 100.0f, 20.0f);
        stats.SetVelocity(info.muzzleVelocity, 120.0f, 250.0f);
        stats.SetRangePerStage(info.effectiveRange, 100.0f, 500.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 3.0f, 1.0f);

        stats.SetReloadMethod(0.25f, 0.55f, 0.95f, -15, 0f);
    }

    public override int GetDamage()
    {
        //To do:  Increase damage per distance to "full effect" distance
        //  Projectile can also accelerate to full effect
        return 500;
    }
}
