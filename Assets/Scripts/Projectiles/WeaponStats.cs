using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Final stats converted from 1-100 to required units for weapon mechanics and weapon type
//  Use helper functions from InitWeaponStats class method for each weapon type
public struct WeaponStats
{
    public int ammoMaxCount;

    public float aimDrift;
    public float sqrRangePerStage;
    public float velocity;
    public float reloadSeconds;
    public float roundsPerMinute;
    public float shotRecoil;

    public float reloadGunLowerEnd;     //End lowering gun, begin sound
    public float reloadArmReturnStart;  //Begin downward arm rotation
    public float reloadGunRaiseEnd;     //End raising gun
    public float reloadMaxTilt;
    public float reloadMaxOffsetZ;

    public void SetAmmoMaxCount(int ammoSize, int min, int max)
    {
        ammoMaxCount = Mathf.FloorToInt(Mathf.Lerp((float)min, (float)max + 0.99f, (float)ammoSize / 100.0f));

        Debug.Log($"Weapon Stat: AmmoSize={ammoSize} to AmmoMaxCount={ammoMaxCount}");
    }

    public void SetAimDrift(float accuracy, float min, float max)
    {
        float drift = (100.0f - Mathf.Min(accuracy, 100.0f));
        aimDrift = Mathf.Lerp(min, max, drift) / 100.0f;

        Debug.Log($"Weapon Stat: Accuracy={accuracy} to AimDrift={aimDrift}");
    }

    public void SetRPM(float fireRate, float min, float max)
    {
        roundsPerMinute = Mathf.Lerp(min, max, Mathf.Min(fireRate, 100.0f) / 100.0f);

        Debug.Log($"Weapon Stat: FireRate={fireRate} to RPM={roundsPerMinute}");
    }

    public void SetShotRecoil(float recoil, float min, float max)
    {
        float amount = (100.0f - Mathf.Min(recoil, 100.0f));
        shotRecoil = Mathf.Lerp(min, max, Mathf.Min(amount, 100.0f)) / 100.0f;

        Debug.Log($"Weapon Stat: Recoil={recoil} to ShotRecoil={shotRecoil}");
    }

    public void SetVelocity(float muzzleVelocity, float min, float max)
    {
        velocity = Mathf.Lerp(min, max, Mathf.Min(muzzleVelocity, 100.0f) / 100.0f);

        Debug.Log($"Weapon Stat: MuzzleVelocity={muzzleVelocity} to Velocity={velocity}");
    }

    public void SetRangePerStage(float effectiveRange, float min, float max)
    {
        float range = Mathf.Lerp(min, max, Mathf.Min(effectiveRange, 100.0f) / 100.0f);

        sqrRangePerStage = range * range;

        Debug.Log($"Weapon Stat: EffectiveRange={effectiveRange} to SqrRangePerStage={sqrRangePerStage}");
    }

    public void SetReloadSeconds(float reloadSpeed, float min, float max)
    {
        float reloadTime = (100.0f - Mathf.Min(reloadSpeed, 100.0f));
        reloadSeconds = Mathf.Lerp(min, max, reloadTime / 100.0f);

        Debug.Log($"Weapon Stat: ReloadSpeed={reloadSpeed} to ReloadSeconds={reloadSeconds}");
    }

    public void SetReloadMethod(float pullback, float hold, float final, float maxTilt, float offsetZ)
    {
        reloadGunLowerEnd = pullback;
        reloadArmReturnStart = pullback + hold;
        reloadGunRaiseEnd = final;
        reloadMaxTilt = maxTilt;
        reloadMaxOffsetZ = offsetZ;
    }

    //To do:  Handle damage dropoff and number of projectiles stats
}

