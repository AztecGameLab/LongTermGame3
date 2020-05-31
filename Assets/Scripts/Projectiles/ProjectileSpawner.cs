using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProjectileSpawner : MonoBehaviour
{
    //[SerializeField]
    private WeaponInfo weaponInfo;          //From containing GameObject

    private GameObject playerWeapon;        //For equipped weapon only
    private float lastFireTime;             //Timestamp of last fired shot
    private ProjectileInfo.Type ammoType;   //Type of projectile to spawn

    static ProjectilePool projectilePool;   //Pools instantiated prefabs for each projectile type

    [System.NonSerialized]
    public WeaponStats weaponStats;         //Normalized per each weapon type from generated weaponInfo

    private AudioMixer masterMixer;
    private AudioSource sfxSource;

    public void InitWeaponStats()
    {
        string type = weaponInfo.reciverType;

        ammoType = (type == "Sniper") ? ProjectileInfo.Type.Sniper :
            (type == "Automatic") ? ProjectileInfo.Type.Assault :
            (type == "Shotgun") ? ProjectileInfo.Type.Shotgun : ProjectileInfo.Type.Standard;

        if (ammoType == ProjectileInfo.Type.Sniper)
        {
            ProjectileSniper.InitWeaponStats(weaponInfo, ref weaponStats);
        }
        else if (ammoType == ProjectileInfo.Type.Shotgun)
        {
            ProjectileShotgun.InitWeaponStats(weaponInfo, ref weaponStats);
        }
        else if (ammoType == ProjectileInfo.Type.Assault)
        {
            ProjectileAssault.InitWeaponStats(weaponInfo, ref weaponStats);
        }
        else
        {
            ProjectileStandard.InitWeaponStats(weaponInfo, ref weaponStats);
        }
    }

    static public void ResetProjectilePool()
    {
        projectilePool = null;    
    }

    void Awake()
    {
        if (projectilePool == null)
        {
            //ProjectilePool is a singleton
            projectilePool = new ProjectilePool();
            projectilePool.Create();
        }

        //audio
        sfxSource = gameObject.AddComponent<AudioSource>();
        masterMixer = Resources.Load<AudioMixer>("Audio/Master") as AudioMixer;
        string SFXMixerGroup = "SFX";
        sfxSource.outputAudioMixerGroup = this.masterMixer.FindMatchingGroups(SFXMixerGroup)[0];
    }

    void Start()
    {
        Debug.Log("Spawner Start");
        ammoType = ProjectileInfo.Type.Shotgun;
        playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
        weaponInfo = gameObject.GetComponent<WeaponInfo>();
        lastFireTime = 0;
    }

    public void OnWeaponTrigger()
    {
        float fireTime = Time.time;
        float fireSpan = fireTime - lastFireTime;
        float minFireSpan = 60.0f / weaponStats.roundsPerMinute;

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

        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        AudioClip fireClip = ammoTypeInfo.soundOnReload;
        //audioSource.PlayOneShot(fireClip);
        sfxSource.PlayOneShot(fireClip);
    }

    public void StopPlayRecharge()
    {
        /*AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Stop();*/
        sfxSource.Stop();
    }

    public GameObject SpawnProjectile(Quaternion trajectory, Vector3 shootFrom, float projScale, bool playFireClip)
    {
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

        Vector3 direction = new Vector3(0, 0, weaponStats.velocity);
        projectile.transform.rotation = trajectory * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        body.velocity = trajectory * direction;

        info.sqrMaxDistance = weaponStats.sqrRangePerStage;

        if (playFireClip)
        {
            //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            AudioClip fireClip = ammoTypeInfo.soundOnFire;
            //audioSource.PlayOneShot(fireClip);
            sfxSource.PlayOneShot(fireClip);
        }

        return projectile;
    }
}
