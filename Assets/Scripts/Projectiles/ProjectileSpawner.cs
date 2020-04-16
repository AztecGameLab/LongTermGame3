using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    //Temporary, should come from weapon game object
    public ProjectileInfo.Type ammoType = ProjectileInfo.Type.Shotgun;

    //[SerializeField]
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
        ammoType = ProjectileInfo.Type.Shotgun;
        playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
        weaponInfo = gameObject.GetComponent<WeaponInfo>();
        weaponInfo.effectiveRange = 0.5f;
        weaponInfo.muzzleVelocity = 30.0f;
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
        //Scaling constants for unit compatibility
        float projScale = 0.5f;        //Projectile size scale

        Vector3 shootFrom = GetBarrel().position;

        SpawnProjectile(gameObject.transform.rotation, shootFrom, projScale, true);
    }

    public void StartPlayRecharge()
    {
        GameObject projectile = projectilePool.PeekNext(ammoType);
        ProjectileInfo info = projectile.GetComponent<ProjectileInfo>();
        AmmoTypeInfo ammoTypeInfo = info.GetComponent<AmmoTypeInfo>();

        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        AudioClip fireClip = ammoTypeInfo.soundOnReload;
        audioSource.PlayOneShot(fireClip);
    }

    public void StopPlayRecharge()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public GameObject SpawnProjectile(Quaternion trajectory, Vector3 shootFrom, float projScale, bool playFireClip)
    {
        float distanceScale = 10.0f;  //Scale to current value range in weaponInfo
        float velocityScale = 0.5f;  //1.5   //Muzzle velocity scale
        GameObject projectile = projectilePool.GetNext(ammoType);

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
        projectile.transform.rotation = trajectory * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        body.velocity = trajectory * direction;

        info.sqrMaxDistance = weaponInfo.effectiveRange * weaponInfo.effectiveRange * distanceScale;

        if (playFireClip)
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            AudioClip fireClip = ammoTypeInfo.soundOnFire;
            audioSource.PlayOneShot(fireClip);
        }

        return projectile;
    }
}
