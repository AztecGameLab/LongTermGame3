using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSniper : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref ProjectileSpawner.WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 1, 4);//10);
        stats.SetAimDrift(info.accuracy, 10.0f);
        stats.SetRPM(info.fireRate, 5.0f);
        stats.SetShotRecoil(info.recoil, 0.2f);

        //To do:  Base on stats
        stats.SetVelocity(info.muzzleVelocity, 100.0f);
        stats.SetRangePerStage(info.effectiveRange, 120.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 2.0f);

        stats.SetReloadMethod(0.25f, 0.55f, 0.95f, -15, 0f);
    }

    public override int GetDamage()
    {
        //To do:  Increase damage per distance to "full effect" distance
        //  Projectile can also accelerate to full effect
        return 100;
    }
}
