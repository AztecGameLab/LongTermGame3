using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void SetAmmoMaxCount(int ammoSize, int minAmmo, int maxAmmo)
        {
            ammoMaxCount = Mathf.FloorToInt(Mathf.Lerp((float)minAmmo, (float)maxAmmo + 0.99f, (float)ammoSize / 100.0f));

            Debug.Log($"Weapon Stat: AmmoSize={ammoSize} to AmmoMaxCount={ammoMaxCount}");
        }

        public void SetAimDrift(float accuracy, float scale)
        {
            aimDrift = (100.0f - Mathf.Min(accuracy, 100.0f)) / (100.0f * scale);

            Debug.Log($"Weapon Stat: Accuracy={accuracy} to AimDrift={aimDrift}");
        }

        public void SetRPM(float fireRate, float scale)
        {
            roundsPerMinute = fireRate * scale;

            Debug.Log($"Weapon Stat: FireRate={fireRate} to RPM={roundsPerMinute}");
        }

        public void SetShotRecoil(float recoil, float scale)
        {
            shotRecoil = ((100.0f - recoil) * scale) / 100.0f;

            Debug.Log($"Weapon Stat: Recoil={recoil} to ShotRecoil={shotRecoil}");
        }

        public void SetVelocity(float muzzleVelocity, float scale)
        {
            //Temporary
            velocity = scale;

            Debug.Log($"Weapon Stat: MuzzleVelocity={muzzleVelocity} to Velocity={velocity}");
        }

        public void SetRangePerStage(float effectiveRange, float scale)
        {
            //Temporary
            sqrRangePerStage = scale * scale;

            Debug.Log($"Weapon Stat: EffectiveRange={effectiveRange} to SqrRangePerStage={sqrRangePerStage}");
        }

        public void SetReloadSeconds(float reloadSpeed, float scale)
        {
            //Temporary
            reloadSeconds = scale;

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

    public void InitWeaponStats(ProjectileInfo.Type _ammoType)
    {
        //To do:  Grab ammoType from weapon info (needs to be added)
        //  Then remove parameter...
        //ammoType = weaponInfo.ammoType;

        ammoType = _ammoType;

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

    void Awake()
    {
        if (projectilePool == null)
        {
            //ProjectilePool is a singleton
            projectilePool = new ProjectilePool();
            projectilePool.Create();
        }
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
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            AudioClip fireClip = ammoTypeInfo.soundOnFire;
            audioSource.PlayOneShot(fireClip);
        }

        return projectile;
    }
}
