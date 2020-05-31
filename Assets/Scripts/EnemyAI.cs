using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyAI : Driver
{
    PlayerDriver playerScript;
    public Transform playerPos;
    public GameObject Enemy02;
    public Collider rigid;

    //Navmesh
    UnityEngine.AI.NavMeshAgent myNav;
    private float checkRate = 0.01f;
    private float nextCheck;

    //Raycast
    public bool isVisible = false;

    //Ranges
    public float killRange = 10f;
    public float agroRange = 20;        //How far until the player agros the enemy
    public float detonationRange = 10f;

    //Explosions
    private float damageToPlayer = 20;
    private float timerDuration = .15f;        //How long the bomb will blow up
    bool explosion = false;
    public bool isDead = false;
    private bool checkOne = false;

    //Visuals
    private float timerBlink = .05f;
    private bool detonateAnim = false;

    public GameObject image;
    public GameObject explode1; //Light explosion off
    public GameObject explode2; //Light explosion on
    public GameObject explode3; //Explosion particle effects on
    public ParticleSystem pe;
    public ParticleSystem pe1;
    public GameObject dot;

    //Shadows
    public GameObject shadow;
    public GameObject Crater;

    private AudioSource sfxSource;

    void Start()
    {
        health = 300;    //Setting the enemy health
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = playerPos.GetComponent<PlayerDriver>();
        myNav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        explode3.SetActive(false);
        explode2.SetActive(false);

        sfxSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;
                playerFollower();   //Called to follow the player 
        }

        ExplosionEnter();
    }


    void FixedUpdate()
    {
        //precompute raysettings
        Vector3 start = transform.position;
        Vector3 direction = player.transform.position - transform.position;

        float distance = 1.5f;

        //draw ray in editor
        //Debug.DrawRay(start, direction * distance, Color.red, 2f, false);

        RaycastHit sighttest;

        if (Physics.Raycast(start, direction, out sighttest, 500))
        {
            //Checks if nothing is in the way of the player
            if (sighttest.transform.CompareTag("Player"))
            {                
                isVisible = true;
                if (!explosion && explosion == false)
                {
                    myNav.speed = 3.5f;
                }
            }
            else
            {
                isVisible = false;
                myNav.speed = 0;
            }
        }

    }

    //Follows the player
    void playerFollower()
    {
        myNav.SetDestination(playerPos.position);
    }

    void ExplosionEnter()
    {
        if (isVisible && Distance() < detonationRange && !isDead)
        {
            explosion = true;
            myNav.isStopped = true;

            sfxSource.Play();

            //Add audio for detonation beeping here 
            if (!isDead)
                StartCoroutine(explosionCounter());     //Freezing the enemy to start the countdown
        }
    }

    void Explode()
    {
        //Explosion particles and the destruction
        if (!isDead)
        {
            isDead = true;
            shadow.SetActive(false);
            var exp = Enemy02.GetComponent<ParticleSystem>();
            
            exp.Emit(100);
            Destroy(rigid);
            dot.SetActive(false);
            Instantiate(Crater,Enemy02.transform.position, Quaternion.identity);
            if (Distance() < killRange && checkOne == false && isVisible == true)
            {
                checkOne = true;
                //Add explosion audio here       
                doDamage();       //Damages the player     
            }
        }

        Destroy(Enemy02, 3);        //Destroys the enemy gameobject after death
    }

    //Used to calculate distance between the player and the enemy for explosion
    //purposes
    private float Distance()
    {       
        return Vector3.Distance(transform.position, player.transform.position);
    }

    //The Enemy Dies
    protected override void OnDeath()
    {
        if (!isDead)
        {
            //Add particle death here to spray out particles
            Destroy(rigid);
            isDead = true;
            shadow.SetActive(false);
            myNav.isStopped = true;
            image.SetActive(false);
            dot.SetActive(false);
            StartCoroutine(DeathAnim());
            pe1.Emit(100);
        }
    }

    //This is the enemy dealing damage
    void doDamage()
    {
        playerScript.TakeDamage(damageToPlayer);
    }

    IEnumerator explosionCounter()  //Adds the delay to the counter
    {
        //Switching between the billboards
        if (!isDead)
        {
            explode3.SetActive(true);
            pe.Play();

            for (int i = 0; i < 15; i++)
            {
                explode1.SetActive(false);
                explode2.SetActive(true);
                yield return new WaitForSeconds(timerBlink);
                explode1.SetActive(true);
                explode2.SetActive(false);
                yield return new WaitForSeconds(timerBlink);
                if (isDead)
                    i = 15;
            }

            //yield return new WaitForSeconds(timerDuration);
            if (detonateAnim == false)
            {
                //Calls the exploding sequence
                explode3.SetActive(false);
                detonateAnim = true;
                image.SetActive(false);
                Explode();
            }
        }
    }

    IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(1);
        Destroy(Enemy02);
    }
}
