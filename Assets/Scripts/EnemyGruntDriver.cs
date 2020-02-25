using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGruntDriver : Driver
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float fireRate = 2f;

    [SerializeField]
    float alertRadius = 30f;

    //distance at which the ai will stop moving towards the player and only shoot
    [SerializeField]
    float stopRadius = 10f;

    //need a way to get the player position
    Transform player;

    //where the enemy is going
    Vector3 move;

    //where the enemy is looking
    Vector3 look;

    bool isAlerted;
    bool shouldShoot;

    float timeLastShot;


    // Start is called before the first frame update
    void Start()
    {
        //need a way to get player transform. don't want to do a Find() bc that's slow

        isAlerted = false;
        shouldShoot = false;
        timeLastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //checks whether player is within alert range, and alerts if it is
        if (Vector3.Distance(transform.position, player.position) <= alertRadius && !isAlerted)
        {
            isAlerted = true;
        }
        if (isAlerted)
        {
            //find movement and look vectors, checks if it is at stopping distance
            look = (player.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, player.position) > stopRadius)
            {
                move = look * moveSpeed;
                
                //so the grunt doesn't fly. only using this value bc it was found in the PlayerDriver script and wanted to be consistent
                move.y = -5f;
            }

            //checks if ai should shoot weapon. GetPrimaryWeapon() returns shouldShoot.
            if (Time.time - timeLastShot >= (1 / fireRate))
                shouldShoot = true;
            else
                shouldShoot = false;
        }
    }
    public override Quaternion GetHorizontalLook()
    {
        //I'm not sure how to separate look into a horizontal and vertical quaternion. Maybe just using this for both works
        return Quaternion.LookRotation(look, Vector3.up);
    }

    public override bool GetMeleeWeapon()
    {
        return false;
    }

    public override Vector3 GetMovement()
    {
        return move;
    }

    public override bool GetPrimaryWeapon()
    {
        return shouldShoot;
    }

    public override bool GetSecondaryWeapon()
    {
        return false;
    }

    public override Quaternion GetVerticalLook()
    {
        //not sure if this would work
        return Quaternion.LookRotation(look, Vector3.up);
    }

    public override bool interact()
    {
        return false;
    }
}
