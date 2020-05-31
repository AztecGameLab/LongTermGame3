using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAssault : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 15, 50);
        stats.SetAimDrift(info.accuracy, 40.0f, 10.0f);
        stats.SetRPM(info.fireRate, 180.0f, 480.0f);
        stats.SetShotRecoil(info.recoil, 30.0f, 5.0f);
        stats.SetVelocity(info.muzzleVelocity, 80.0f, 200.0f);
        stats.SetRangePerStage(info.effectiveRange, 12.0f, 40.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 3.5f, 2.0f);

        stats.SetReloadMethod(0.1f, 0.7f, 1.0f, 0, 1.0f);
    }
}
