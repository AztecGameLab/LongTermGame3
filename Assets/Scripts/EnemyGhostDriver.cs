using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGhostDriver : Driver
{
    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float fireRate = 2f;

    [SerializeField]
    private float alertRadius = 30f;

    //distance at which the ai will stop moving towards the player and only shoot
    [SerializeField]
    private float meleeRange = 2f;

    //where the enemy is going
    private Vector3 move;

    //where the enemy is looking
    private Vector3 look;
    private float lookHorizontal;
    private float lookVertical;

    private bool isAlerted;
    private bool shouldMelee;

    private float timeLastShot;


    // Start is called before the first frame update
    void Start()
    {
        //need a way to get player transform. don't want to do a Find() bc that's slow

        isAlerted = false;
        shouldMelee = false;
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
            //find movement and look vectors, moves ghost towards player
            look = (player.position - transform.position).normalized;
            move = look * moveSpeed;

            //look = transform.InverseTransformDirection(look);
            Quaternion lookQuat = Quaternion.LookRotation(look, Vector3.up);
            lookHorizontal = lookQuat.eulerAngles.y;
            lookVertical = lookQuat.eulerAngles.z;
            

            //checks if ai should shoot weapon. GetPrimaryWeapon() returns shouldShoot.
            if ((Time.time - timeLastShot >= (1 / fireRate)) && Vector3.Distance(transform.position, player.position) <= meleeRange)
                shouldMelee = true;
            else
                shouldMelee = false;
        }
    }
    public override float GetHorizontalLook()
    {
        return lookHorizontal;
    }

    public override bool GetMeleeWeapon()
    {
        return shouldMelee;
    }

    public override Vector3 GetMovement()
    {
        return move;
    }

    public override bool GetPrimaryWeapon()
    {
        return false;
    }

    public override bool GetSecondaryWeapon()
    {
        return false;
    }

    public override float GetVerticalLook()
    {
        return lookVertical;
    }

    public override bool interact()
    {
        return false;
    }
}
