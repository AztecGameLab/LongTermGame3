using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Driver
{
    public Transform playerPos;
    public GameObject Enemy02;
    //public static Rigidbody playerRigidBody;
    UnityEngine.AI.NavMeshAgent myNav;

    [SerializeField]
    private float checkRate = 0.01f;
    [SerializeField]
    private float nextCheck;
    private float currentDifX = 999;
    private float currentDifZ = 999;
    private bool checkOne = false;
    [SerializeField]
    private bool inRangeDuringExplosion = false;

    //TODO REMOVE AND PLACE STATIC VARIABLES AFTER TESTING
    [SerializeField]
    private float agroRange = 1500;        //How far until the player agros the enemy
    [SerializeField]
    private float damageToPlayer = 50;   
    [SerializeField]
    private float timerDuration = 3;        //How long the bomb will blow up
    int isExploded = 0;

    //Knockback for the player
    [SerializeField]
    float radius = 10.0f;
    [SerializeField]
    float power = 1000.0f;

    //How many blinks the enemy does per sec
    public float timerBlink = .2f;

    private bool detonateAnim = false;
    bool isVisible = false;
    public GameObject image;
    public GameObject player;
    RaycastHit hit;
    private GameObject explode1;
    private GameObject explode2;
    public GameObject explode3;
    public ParticleSystem pe;

    bool explosion = false;
    void Start()
    {
        health = 50;    //Setting the enemy health
        player = GameObject.Find("Player");
        explode1 = GameObject.Find("Explode1");
        explode2 = GameObject.Find("Explode2");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
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
    }

    //Some code used from https://www.reddit.com/r/Unity2D/comments/61e1wn/enemy_line_of_sight_raycast2d_help/
    void FixedUpdate()
    {
        //precompute raysettings
        Vector3 start = transform.position;
        Vector3 direction = player.transform.position - transform.position;

        float distance = 1.5f;

        //draw ray in editor
        Debug.DrawRay(start, direction * distance, Color.red, 2f, false);

        RaycastHit sighttest;

        if (Physics.Raycast(start, direction, out sighttest,  100))
        {
            //Checks if nothing is in the way of the player
            if (sighttest.transform.CompareTag("Player"))
            {
                isVisible = true;
            }
            else
            {
                isVisible = false;
            }
        }

    }

    //Follows the player
    void playerFollower()
    {
        myNav.SetDestination(playerPos.position);
    }

    private void OnTriggerEnter(Collider other) //If the enemy collides with the player
    {
        if(other.tag == "Player" && explosion == false && isVisible == true)
        {
            explosion = true;
            inRangeDuringExplosion = true;
            detonate();
        }       
    }

    private void OnTriggerExit(Collider other)  //Player leaves the range of the enemy
    {
        inRangeDuringExplosion = false;
    }

    //Starts the detonation sequence
    void detonate()
    {
        myNav.speed = 0;
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

        //Destroys the image 
        Destroy(image);             
        if (inRangeDuringExplosion == true && checkOne == false && isVisible == true)
        {
            checkOne = true;

            //Add explosion audio here
            
            knockback();            
            doDamage();       //Damages the player     
        }
                     
        Destroy(Enemy02, 3);        //Destroys the enemy gameobject after death
    }

    //TODO doesnt work now 
    //This function is to knock the player back after explosion
    void knockback()    
    {
        //Vector3 explosionPos = transform.position;
        //playerRigidBody.AddExplosionForce(power, explosionPos, radius, 3.0F);
    }

    //The Enemy Fucking Dies
    protected override void OnDeath()
    {
        isExploded++;
        Destroy(Enemy02);
    }

    //This is the enemy dealing damage
    void doDamage()
    {
        Debug.Log("Damage done: " + damageToPlayer);
        TakeDamage(10f);      
    }

    IEnumerator explosionCounter()  //Adds the delay to the counter
    {
        //Switching between the billboards

        explode3.SetActive(true);
        pe.Play();

        for (int i = 0; i < 20; i++)
        {
            explode1.SetActive(false);            
            explode2.SetActive(true);
            yield return new WaitForSeconds(timerBlink);
            explode1.SetActive(true);
            explode2.SetActive(false);
            yield return new WaitForSeconds(timerBlink);
        }

        //yield return new WaitForSeconds(timerDuration);
        if(detonateAnim == false)
        {
            //Calls the exploding sequence
            explode3.SetActive(false);
            detonateAnim = true;
            Explode();
        }        
    }
}
