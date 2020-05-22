using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStandard : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 6, 20);
        stats.SetAimDrift(info.accuracy, 5.0f, 5.0f);
        stats.SetRPM(info.fireRate, 100.0f, 100.0f);
        stats.SetShotRecoil(info.recoil, 10.0f, 10.0f);
        stats.SetVelocity(info.muzzleVelocity, 70.0f, 70.0f);
        stats.SetRangePerStage(info.effectiveRange, 70.0f, 70.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 1.2f, 1.2f);

        stats.SetReloadMethod(0.2f, 0.6f, 1.0f, 10.0f, 0.5f);
    }

}
