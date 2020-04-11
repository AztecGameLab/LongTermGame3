using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShotgun : ProjectileInfo
{
    const int maxSplits = 3;
    int splitCurrent;

    protected override bool OnMaxDistance()
    {
        splitCurrent++;

        if (splitCurrent < maxSplits)
        {
            //Split projectile
            return false;
        }
        else
        {
            return true;
        }
    }
}
