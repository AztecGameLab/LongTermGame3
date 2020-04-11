using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    private WeaponInfo weaponInfo;

    private GameObject playerWeapon;
    private float lastFireTime;


    static ProjectilePool projectilePool;
    static GameObject projectilePrefab;

    void Awake()
    {
        if (projectilePool == null)
        {
            projectilePool = new ProjectilePool();
            projectilePool.Create();
        }
    }

    void Start()
    {
        playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
        weaponInfo = gameObject.GetComponent<WeaponInfo>();
        lastFireTime = 0;
    }

    public void OnWeaponTrigger()
    {
        float fireTime = Time.time;
        float fireSpan = fireTime - lastFireTime;
        float minFireSpan = 60.0f / weaponInfo.fireRate;  //Fire rate as RPM

        minFireSpan /= 5.0f;  //Scale to current spawned weapon values

        if (fireSpan >= minFireSpan)
        {
            lastFireTime = fireTime;

            if (playerWeapon != null)
            {
                playerWeapon.GetComponent<PlayerWeapon>().OnFireWeapon();
            }
            else
            {
                //Temporary to not break test scenes
                SpawnProjectile();
            }
        }
    }

    private Transform GetBarrel()
    {
        Transform result = gameObject.transform;

        if (result.childCount > 0) result = result.GetChild(0);
        if (result.childCount > 0) result = result.GetChild(0);

        return result;
    }

    public void SpawnProjectile()
    {
        GameObject projectile = projectilePool.GetNext(ProjectileInfo.Type.Standard);

        //Scaling constants for unit compatibility
        float projScale = 0.5f;        //Projectile size scale
        float velocityScale = 1.5f;    //Muzzle velocity scale
        
        Vector3 shootFrom = GetBarrel().position;
        ProjectileInfo info = projectile.GetComponent<ProjectileInfo>();
        info.startPosition = shootFrom;
        info.weaponInfo = weaponInfo;

        AmmoTypeInfo ammoTypeInfo = info.GetComponent<AmmoTypeInfo>();

        //Projectile dimensions
        float projDiameter = ammoTypeInfo.caliber * projScale;
        float projLength = projDiameter * (ammoTypeInfo.caliberToLength * 2.0f);

        projectile.transform.position = shootFrom;
        projectile.transform.localScale = new Vector3(projDiameter, projLength, projDiameter);

        Rigidbody body = projectile.GetComponent<Rigidbody>();

        Vector3 direction = new Vector3(0, 0, weaponInfo.muzzleVelocity * velocityScale);
        projectile.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        body.velocity = gameObject.transform.rotation * direction;

        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        AudioClip fireClip = ammoTypeInfo.soundOnFire;
        audioSource.PlayOneShot(fireClip);
    }
}
