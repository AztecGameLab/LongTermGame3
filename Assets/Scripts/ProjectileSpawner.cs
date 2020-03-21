using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float targetDistance = 50.0f;                    //Distance to crosshair
    public Vector2 weaponOffsetXY = new Vector2(0, 0);      //Offset from weapon to crosshair center

    [SerializeField]
    private WeaponInfo weaponInfo;
    [SerializeField]
    private AmmoTypeInfo ammoTypeInfo;

    private float lastFireTime;

    class ProjectilePool
    {
        public int current;
        public int size;
        public GameObject[] data;

        public ProjectilePool(int _size)
        {
            size = _size;
            current = 0;
            data = new GameObject[size];
        }

        public GameObject GetNext()
        {
            GameObject result = data[current];
            result.SetActive(true);
            current = (++current % size);

            return result;
        }
    }

    static ProjectilePool projectilePool;
    static GameObject projectilePrefab;

    //Temporary for current code in WeaponSpawner
    //public void InitializeThis()
    //{
      //  Awake();
        //Start();
    //}

    void Awake()
    {
        projectilePrefab = (GameObject)Resources.Load("Projectile");

        if (projectilePool == null)
        {
            projectilePool = new ProjectilePool(100);
        }

        for (int i = 0; i < projectilePool.size; i++)
        {
            GameObject p = Instantiate(projectilePrefab);
            p.SetActive(false);

            projectilePool.data[i] = p;
        }
    }

    void Start()
    {
        weaponInfo = gameObject.GetComponent<WeaponInfo>();
        ammoTypeInfo = gameObject.GetComponent<AmmoTypeInfo>();

        if (ammoTypeInfo == null)
        {
            ammoTypeInfo = gameObject.AddComponent<AmmoTypeInfo>();
            AmmoTypeInfo.DefaultAmmoType(ammoTypeInfo);
        }

        lastFireTime = 0;
    }

    public void OnWeaponTrigger()
    {
        float fireTime = Time.time;
        float fireSpan = fireTime - lastFireTime;
        float minFireSpan = 60.0f / weaponInfo.fireRate;  //Fire rate as RPM

        minFireSpan /= 10.0f;  //Scale to current spawned weapon values

        if (fireSpan >= minFireSpan)
        {
            lastFireTime = fireTime;
            SpawnProjectile();
        }
    }

    private Transform GetBarrel()
    {
        Transform result = gameObject.transform;

        if (result.childCount > 0) result = result.GetChild(0);
        if (result.childCount > 0) result = result.GetChild(0);

        return result;
    }

    private void SpawnProjectile()
    {
        GameObject projectile = projectilePool.GetNext();

        //Scaling constants for unit compatibility
        float projScale = 0.5f;         //Projectile scale
        float velocityScale = 2.0f;    //Muzzle velocity scale
        
        Vector3 shootFrom = GetBarrel().position;
        Vector3 shootAt = shootFrom + gameObject.transform.rotation * new Vector3(weaponOffsetXY.x, weaponOffsetXY.y, targetDistance);

        float scaledErrorX = targetDistance * (1.0f - weaponInfo.accuracy);
        float scaledErrorY = scaledErrorX * 0.5f;

        //~Disable accuracy for now
        scaledErrorX = 0;
        scaledErrorY = 0;

        Vector3 target = shootAt + new Vector3(Random.Range(-scaledErrorX, scaledErrorX), Random.Range(-scaledErrorY, scaledErrorY), 0.0f);
        Quaternion rotation = Quaternion.LookRotation(target - shootFrom, Vector3.up);

        ProjectileInfo info = projectile.GetComponent<ProjectileInfo>();
        info.startPosition = shootFrom;
        info.weaponInfo = weaponInfo;
        info.ammoTypeInfo = ammoTypeInfo;

        //Projectile dimensions
        float projDiameter = ammoTypeInfo.caliber * projScale;
        float projLength = projDiameter * ammoTypeInfo.caliberToLength;

        projectile.transform.position = shootFrom;
        projectile.transform.localScale = new Vector3(projDiameter, projLength, projDiameter);

        Rigidbody body = projectile.GetComponent<Rigidbody>();

        Vector3 direction = new Vector3(0, 0, weaponInfo.muzzleVelocity * velocityScale);
        projectile.transform.rotation = rotation * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        body.velocity = rotation * direction;
    }
}
