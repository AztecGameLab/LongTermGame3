using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAssault : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 12, 50);
        stats.SetAimDrift(info.accuracy, 5.0f, 20.0f);
        stats.SetRPM(info.fireRate, 300.0f, 400.0f);
        stats.SetShotRecoil(info.recoil, 5.0f, 20.0f);
        stats.SetVelocity(info.muzzleVelocity, 70.0f, 70.0f);
        stats.SetRangePerStage(info.effectiveRange, 70.0f, 70.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 2.8f, 2.8f);

        stats.SetReloadMethod(0.1f, 0.7f, 1.0f, 0, 1.0f);
    }
}
