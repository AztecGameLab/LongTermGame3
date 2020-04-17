using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAssault : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref ProjectileSpawner.WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 10, 15);// 100);
        stats.SetAimDrift(info.accuracy, 2.5f);
        stats.SetRPM(info.fireRate, 10.0f);
        stats.SetShotRecoil(info.recoil, 0.2f);

        //To do:  Base on stats
        stats.SetVelocity(info.muzzleVelocity, 80.0f);
        stats.SetRangePerStage(info.effectiveRange, 90.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 3.2f);

        stats.SetReloadMethod(0.1f, 0.7f, 1.0f, 0, 1.0f);
    }
}
