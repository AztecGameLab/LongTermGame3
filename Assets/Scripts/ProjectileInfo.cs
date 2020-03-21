﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : MonoBehaviour
{
    public Vector3 startPosition;
    public WeaponInfo weaponInfo;
    public AmmoTypeInfo ammoTypeInfo;

    void OnCollisionEnter(Collision collision)
    {
        //To do:  If hit enemy: calculate damage from weaponInfo, ammoTypeInfo, and enemy stats
        //  On all collision: set projectile to inactive
        //  'startPosition' is for determining effect of damage falloff
        GameObject target = collision.gameObject;

        if (target.tag == "Enemy")
        {
            TestTarget testTarget = target.GetComponent<TestTarget>();

            if (testTarget != null)
            {
                testTarget.ResetTarget();
            }
        }

        gameObject.SetActive(false);
    }

    private void Start()
    {
    }

    private void Awake()
    {
    }

    void FixedUpdate()
    {
        float distanceScale = 4.0f;  //Scale to current value range in weaponInfo
        float sqrMaxDistance = weaponInfo.effectiveRange * weaponInfo.effectiveRange * distanceScale;
        float sqrTravelDist = (gameObject.transform.position - startPosition).sqrMagnitude;

        if (sqrTravelDist > sqrMaxDistance)
        {
            gameObject.SetActive(false);
        }
    }
}
