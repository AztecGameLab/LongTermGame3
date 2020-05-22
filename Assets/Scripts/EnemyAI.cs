using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Driver
{
    PlayerDriver playerScript;
    public Transform playerPos;
    public GameObject Enemy02;
    UnityEngine.AI.NavMeshAgent myNav;

    [SerializeField]
    private float checkRate = 0.01f;
    [SerializeField]
    private float nextCheck;
    private float currentDifX = 999;
    private float currentDifZ = 999;
    private bool checkOne = false;
    public float killRange = 16f;
    //TODO REMOVE AND PLACE STATIC VARIABLES AFTER TESTING
    [SerializeField]
    private float agroRange = 1500;        //How far until the player agros the enemy
    [SerializeField]
    private float damageToPlayer = 50;
    [SerializeField]
    private float timerDuration = .15f;        //How long the bomb will blow up
    bool explosion = false;
    bool isVisible = false;
    bool isDead = false;

    //Visuals
    private float timerBlink = .05f;
    private bool detonateAnim = false;
    public GameObject Crater;
    public GameObject image;
    public GameObject explode1;
    public GameObject explode2;
    public GameObject explode3;
    public ParticleSystem pe;
    public ParticleSystem pe1;

    void Start()
    {
        health = 50;    //Setting the enemy health
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = playerPos.GetComponent<PlayerDriver>();
        myNav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        explode3.SetActive(false);
        explode2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Calculates the difference between player and the enemy
        currentDifX = Mathf.Abs(playerPos.transform.position.x - transform.position.x);
        currentDifZ = Mathf.Abs(playerPos.transform.position.z - transform.position.z);

        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;
            if (currentDifX <= agroRange && currentDifZ <= agroRange) //this is the range to check the agro distance
            {
                playerFollower();   //Called to follow the player 
            }
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

        if (Physics.Raycast(start, direction, out sighttest, 100))
        {
            //Checks if nothing is in the way of the player
            if (sighttest.transform.CompareTag("Player"))
            {
                isVisible = true;
                if (!explosion && explosion == false)
                {
                    myNav.speed = 7.5f;
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
        if (isVisible && Distance() < 10)
        {
            explosion = true;
            myNav.isStopped = true;
            detonate();
        }
    }

    // //If the enemy collides with the player
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.tag == "Player" && explosion == false && isVisible == true)
    //     {
    //         explosion = true;
    //         myNav.isStopped = true;
    //         detonate();
    //     }
    // }

    //Starts the detonation sequence
    void detonate()
    {
        //Add audio for detonation beeping here 
        StartCoroutine(explosionCounter());     //Freezing the enemy to start the countdown
    }

    void Explode()
    {
        //Explosion particles and the destruction
        var exp = Enemy02.GetComponent<ParticleSystem>();
        exp.Emit(100);

        explode1.SetActive(false);
        explode2.SetActive(false);
        Crater.gameObject.transform.localScale += new Vector3(3, 3, 3);
        //Destroys the image 
        Destroy(image);

        if (Distance() < killRange && checkOne == false && isVisible == true)
        {
            checkOne = true;
            //Add explosion audio here       
            doDamage();       //Damages the player     
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
        if(!isDead)
            return;

        isDead = true;
        
        //Add particle death here to spray out particles
        StartCoroutine(DeathAnim());
        pe1.Emit(100);
    }

    //This is the enemy dealing damage
    void doDamage()
    {
        playerScript.TakeDamage(damageToPlayer);
    }

    IEnumerator explosionCounter()  //Adds the delay to the counter
    {
        //Switching between the billboards

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
        }

        //yield return new WaitForSeconds(timerDuration);
        if (detonateAnim == false)
        {
            //Calls the exploding sequence
            explode3.SetActive(false);
            detonateAnim = true;
            Explode();
        }
    }

    IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(1);
        Destroy(Enemy02);
    }
}
