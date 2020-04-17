﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShotgun : ProjectileInfo
{
    //To do:  MaxSplits and split behavior from projectileCount
    const int maxSplits = 3;
    int splitCurrent;

    static public void InitWeaponStats(WeaponInfo info, ref ProjectileSpawner.WeaponStats stats)
    {
        stats.SetAmmoMaxCount(info.ammoSize, 1, 6);
        stats.SetAimDrift(info.accuracy, 3.0f);
        stats.SetRPM(info.fireRate, 5.0f);
        stats.SetShotRecoil(info.recoil, 1.0f);

        //To do:  Base on stats
        stats.SetVelocity(info.muzzleVelocity, 15.0f);
        stats.SetRangePerStage(info.effectiveRange, 1.6f);
        stats.SetReloadSeconds(info.reloadSpeed, 2.0f);

        stats.SetReloadMethod(0.2f, 0.4f, 0.9f, 50.0f, 0);
    }

    public override void ResetState()
    {
        splitCurrent = 0;
        base.ResetState();
    }

    void SpawnNew(ProjectileSpawner spawner, Quaternion t, float scale)
    {
        GameObject p = (GameObject)spawner.SpawnProjectile(t, transform.localPosition, scale, false);
        ProjectileShotgun ps = p.GetComponent<ProjectileShotgun>();
        ps.splitCurrent = splitCurrent;
        ps.sqrMaxDistance = sqrMaxDistance;

    }

    protected override bool OnMaxDistance()
    {
        splitCurrent++;

        if (splitCurrent < maxSplits)
        {
            //Split projectile
            GameObject playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
            GameObject envelope = playerWeapon.transform.GetChild(0).gameObject;
            GameObject weapon = envelope.transform.GetChild(0).gameObject;

            ProjectileSpawner spawner = weapon.GetComponent<ProjectileSpawner>();

            Rigidbody body = GetComponent<Rigidbody>();
            Quaternion t0 = transform.rotation * Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            Quaternion t1 = t0 * Quaternion.Euler(0.0f, 30.0f / splitCurrent, 0.0f);
            Quaternion t2 = t0 * Quaternion.Euler(0.0f, -30.0f / splitCurrent, 0.0f);
            Quaternion t3 = t0 * Quaternion.Euler(10.0f / splitCurrent, 0, 0);
            Quaternion t4 = t0 * Quaternion.Euler(-10.0f / splitCurrent, 0, 0);

            float scale = 0.5f - (0.01f * splitCurrent);
            float scaleLast = scale + 0.01f;
            float scaleRatio = scale / scaleLast;

            sqrMaxDistance *= 3;

            SpawnNew(spawner, t1, scale);
            SpawnNew(spawner, t2, scale);
            SpawnNew(spawner, t3, scale);
            SpawnNew(spawner, t4, scale);

            transform.localScale *= scaleRatio;
            return false;
        }
        else
        {
            return true;
        }
    }
}