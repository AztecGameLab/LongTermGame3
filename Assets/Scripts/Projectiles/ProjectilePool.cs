using UnityEngine;
using System.Collections;

public class ProjectilePool
{
    class PoolData
    {
        public int current;
        public int size;
        public GameObject[] data;

        public PoolData(int _size)
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

        public void Load(string resourceName)
        {
            GameObject prefab = (GameObject)Resources.Load(resourceName);

            for (int i = 0; i < size; i++)
            {
                GameObject p = Object.Instantiate(prefab);

                p.SetActive(false);

                data[i] = p;
            }
        }
    }

    PoolData[] pools;

    public void Create()
    {
        string[] resources = new string[] { "Projectile_Standard", "Projectile_Shotgun", "Projectile_Sniper", "Projectile_Assault" };

        int count = (int)ProjectileInfo.Type.Count;
        pools = new PoolData[count];

        pools[(int)ProjectileInfo.Type.Standard] = new PoolData(40);
        pools[(int)ProjectileInfo.Type.Shotgun] = new PoolData(40);
        pools[(int)ProjectileInfo.Type.Sniper] = new PoolData(40);
        pools[(int)ProjectileInfo.Type.Standard] = new PoolData(80);

        for(int i = 0; i < count; i++)
        {
            pools[i].Load(resources[i]);
        }
    }

    public GameObject GetNext(ProjectileInfo.Type type)
    {
        return pools[(int)type].GetNext();        
    }
}
