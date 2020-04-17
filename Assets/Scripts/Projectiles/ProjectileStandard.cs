using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStandard : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref ProjectileSpawner.WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 6, 10);//20);
        stats.SetAimDrift(info.accuracy, 10.0f);
        stats.SetRPM(info.fireRate, 5.0f);
        stats.SetShotRecoil(info.recoil, 0.1f);

        //To do:  Base on stats
        stats.SetVelocity(info.muzzleVelocity, 50.0f);
        stats.SetRangePerStage(info.effectiveRange, 60f);
        stats.SetReloadSeconds(info.reloadSpeed, 1.2f);

        stats.SetReloadMethod(0.2f, 0.6f, 1.0f, 10.0f, 0.5f);
    }

}
