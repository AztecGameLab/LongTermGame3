using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStandard : ProjectileInfo
{
    static public void InitWeaponStats(WeaponInfo info, ref WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 6, 20);
        stats.SetAimDrift(info.accuracy, 12.0f, 3.0f);    
        stats.SetRPM(info.fireRate, 80.0f, 270.0f);     
        stats.SetShotRecoil(info.recoil, 50.0f, 5.0f);
        stats.SetVelocity(info.muzzleVelocity, 50.0f, 180.0f);
        stats.SetRangePerStage(info.effectiveRange, 8.0f, 20.0f);
        stats.SetReloadSeconds(info.reloadSpeed, 2.2f, 0.7f);

        stats.SetReloadMethod(0.2f, 0.6f, 1.0f, 10.0f, 0.5f);
    }

}
