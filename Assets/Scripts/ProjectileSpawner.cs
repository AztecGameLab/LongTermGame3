using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float muzzleVelocity;
    public float caliber;
    public float caliberToLength = 2.0f;
    public float accuracy = 0.9f;
    public float targetDistance = 50.0f;

    public Vector3 shootFrom;

    public GameObject projectilePrefab;

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

    void Awake()
    {
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

    void Update()
    {
        //To do:  Move this script into the Weapon GameObject, then call SpawnProjectile from the weapon instead.
        if (Input.GetKeyDown("space"))
        {
            SpawnProjectile(null, null);
        }
    }

    public void SpawnProjectile(WeaponInfo weapon, AmmoTypeInfo ammoType)
    {
        GameObject projectile = projectilePool.GetNext();

        ProjectileInfo info = projectile.GetComponent<ProjectileInfo>();
        info.startPosition = shootFrom;
        info.weapon = weapon;
        info.ammoType = ammoType;

        Vector3 shootAt = new Vector3(0, targetDistance * 0.1f, targetDistance);
        float scaledErrorX = targetDistance * (1.0f - accuracy);
        float scaledErrorY = scaledErrorX * 0.5f;
        Vector3 target = shootAt + new Vector3(Random.Range(-scaledErrorX, scaledErrorX), Random.Range(-scaledErrorY, scaledErrorY), 0.0f);
        Quaternion rotation = Quaternion.LookRotation(target - shootFrom, Vector3.up);

        Vector3 scale = new Vector3(caliber, caliber * caliberToLength, caliber);
        projectile.transform.position = shootFrom;
        projectile.transform.localScale = scale;

        Rigidbody body = projectile.GetComponent<Rigidbody>();

        Vector3 direction = new Vector3(0, 0, muzzleVelocity);
        projectile.transform.rotation = rotation * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        body.velocity = rotation * direction;
    }
}
