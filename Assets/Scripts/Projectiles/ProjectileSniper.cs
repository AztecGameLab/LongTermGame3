using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSniper : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 1, 10);
        stats.SetAimDrift(info.accuracy, 0.0f, 2.0f);
        stats.SetRPM(info.fireRate, 60.0f, 60.0f);
        stats.SetShotRecoil(info.recoil, 15.0f, 15.0f);
        stats.SetVelocity(info.muzzleVelocity, 180.0f, 250.0f);
        stats.SetRangePerStage(info.effectiveRange, 200.0f, 200.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 1.8f, 1.8f);

        /*
        stats.SetAimDrift(info.accuracy, 10.0f);
        stats.SetRPM(info.fireRate, 5.0f);
        stats.SetShotRecoil(info.recoil, 0.2f);

        //To do:  Base on stats
        stats.SetVelocity(info.muzzleVelocity, 100.0f);
        stats.SetRangePerStage(info.effectiveRange, 120.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 2.0f);*/

        stats.SetReloadMethod(0.25f, 0.55f, 0.95f, -15, 0f);
    }

    public override int GetDamage()
    {
        //To do:  Increase damage per distance to "full effect" distance
        //  Projectile can also accelerate to full effect
        return 100;
    }
}
