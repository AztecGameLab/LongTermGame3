using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGruntDriver : Driver
{
    [SerializeField]
    private Transform firePointLeft;

    [SerializeField]
    private Transform firePointRight;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletSpeed = 10f;

    [SerializeField]
    private float fireRate = 2f;

    [SerializeField]
    private float alertRadius = 30f;

    [SerializeField]
    private float targetPlayerDistance = 7;

    [SerializeField]
    private float fieldOfView = 120f;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float alertCountdown = 1f;

    private bool isAlerted;

    private Vector3 playerVector;

    private float timeLastShot;

    private bool fireRight;

    Transform player;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindObjectOfType<PlayerDriver>().transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        
        isAlerted = false;
        timeLastShot = Time.time;
        fireRight = true;

        health = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(firePointRight.position, player.position);
        //Debug.DrawLine(firePointLeft.position, player.position);
        playerVector = player.position - transform.position;

        //print(player.position);
        print(firePointLeft.position);
        if (!isAlerted)
            CheckForAlert();

        //moves the grunt toward the player if far away
        if (isAlerted && playerVector.magnitude > targetPlayerDistance)
        {
            agent.SetDestination(player.position);
            agent.updateRotation = true;
        }
        //moves the grunt away from the player if too close
        else if(isAlerted && playerVector.magnitude < targetPlayerDistance)
        {
            agent.SetDestination(player.position + (-1 * playerVector.normalized * targetPlayerDistance));
            agent.updateRotation = false;
        }

        //turns towards the player
        if (isAlerted && playerVector.magnitude <= targetPlayerDistance)
        {
            //grunt snaps to player in radius
            /*
            Vector3 direction = new Vector3(playerVector.x, 0, playerVector.z).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
            */

            //grunt has a turning speed
            Vector3 direction = transform.forward.normalized + new Vector3(playerVector.x-transform.forward.x, 0, playerVector.z - transform.forward.z).normalized * Time.deltaTime * rotationSpeed;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;

        }
        if (isAlerted && (Time.time - timeLastShot >= (1 / fireRate)) && CheckLineOfSight())
        {
            Fire();
            timeLastShot = Time.time;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        GameObject otherObject = other.gameObject;

        if (otherObject.tag.Equals("PlayerWeapon"))
        {
            //need a way to get the damage value of player's  bullet
            //TakeDamage(otherObject.GetComponent<PlayerWeapon>().damage);
        }
    }
    void CheckForAlert()
    {
        if (!isAlerted && playerVector.magnitude <= alertRadius && CheckLineOfSight())
        {
            alertCountdown -= Time.deltaTime;
        }
        if(!isAlerted && alertCountdown <= 0f)
        {
            isAlerted = true;
        }
    }
     bool CheckLineOfSight()
    {
        bool isSighted;
        //layermask that ignores the player layer
        int layerMask = LayerMask.GetMask("Player");
        isSighted = Physics.Raycast(transform.position, playerVector, playerVector.magnitude, layerMask) && Vector3.Angle(transform.forward, playerVector) <= 0.5 * fieldOfView;
        //print(isSighted);
        return isSighted;
    }
    void Fire()
    {
        if (fireRight)
        {
            
            GameObject newBullet = Instantiate(bullet, firePointRight.position, Quaternion.identity);
            Vector3 dir = (player.position - newBullet.transform.position).normalized;
            newBullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
            Debug.DrawRay(firePointRight.position, dir);
            fireRight = false;
        }
        else
        {
            Vector3 dir = (player.position - firePointLeft.position).normalized;
            GameObject newBullet = Instantiate(bullet, firePointLeft.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
            Debug.DrawRay(firePointLeft.position, dir);
            fireRight = true;
        }
    }
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
