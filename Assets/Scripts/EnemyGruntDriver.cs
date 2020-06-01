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

    Vector3 originOffset;

    public AudioClip enemyWeaponClip;
    private AudioSource sfxSource;
    public float pitchMin = 0.8f, pitchMax = 1.0f;

    public ParticleSystem deathEffect;
    
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        
        isAlerted = false;
        timeLastShot = Time.time;
        fireRight = true;

        health = 500;

        originOffset = new Vector3(0, 1, 0);

        sfxSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(firePointRight.position, player.position);
        //Debug.DrawLine(firePointLeft.position, player.position);
        playerVector = player.transform.position - (transform.position + originOffset);
        //print(player.position);
        //print(firePointLeft.position);
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
        bool isSighted = false;
        Vector3 start = transform.position+originOffset;

        RaycastHit lineOfSight;
        if (Physics.Raycast(start, playerVector, out lineOfSight, playerVector.magnitude))
        {
            //Checks if nothing is in the way of the player
            if (lineOfSight.transform.CompareTag("Player"))
            {
                isSighted = true;
            }
            else
            {
                isSighted = false;
            }
        }
        return isSighted;
    }
    private void PlayWeaponSFX()
    {
        sfxSource.pitch = Random.Range(pitchMin, pitchMax);
        sfxSource.volume = 0.8f;
        sfxSource.clip = enemyWeaponClip;
        sfxSource.Play();
    }
    void Fire()
    {
        if (fireRight)
        {
            
            GameObject newBullet = Instantiate(bullet, firePointRight.position, Quaternion.identity);
            Vector3 dir = transform.forward; //(player.position - newBullet.transform.position + new Vector3(0, -1, 0)).normalized;
            newBullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
            newBullet.GetComponent<GruntBullet>().playerPos = player;
            Debug.DrawRay(firePointRight.position, dir);
            fireRight = false;
            // shoot sfx
            PlayWeaponSFX();
        }
        else
        {
            Vector3 dir = transform.forward; //(player.position - firePointLeft.position + new Vector3(0,-1,0)).normalized;
            GameObject newBullet = Instantiate(bullet, firePointLeft.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
            newBullet.GetComponent<GruntBullet>().playerPos = player;
            Debug.DrawRay(firePointLeft.position, dir);
            fireRight = true;
            // shoot sfx
            PlayWeaponSFX();
        }
    }
    protected override void OnDeath()
    {
        Debug.Log("Going to DeathAnimation");
        DeathAnimation();
        Destroy(gameObject);
       
    }
    public void DeathAnimation()
    {
        Debug.Log("In DeathAnimation");
        Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.main.startLifetime.constant);
    }
}
