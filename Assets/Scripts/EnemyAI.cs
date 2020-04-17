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

    //Health of the enemy 
    [SerializeField]
    float healthBar = 100f;
    float bulletDamage = 20f;
    private bool detonateAnim = false;

    public GameObject image;
    public GameObject player;

    private GameObject explode1;
    private GameObject explode2;
    void Start()
    {
        player = GameObject.Find("Player");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //playerRigidBody = playerPos.GetComponent<Rigidbody>();
        myNav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        explode1 = GameObject.Find("Explode1");
        explode2 = GameObject.Find("Explode2");
        explode2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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

        //If the player kills the enemy with a gun
        if (healthBar <= 0 && isExploded == 0)
        {
            isExploded++;
            death();
        }        
    }

    void playerFollower()
    {
        //Add the bomb movement animation here
        //Add the bomb audio animation here
        myNav.SetDestination(playerPos.position);
    }

    private void OnTriggerEnter(Collider other) //If the enemy collides with the player
    {
        if(other.tag == "Player")
        {
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
        //Add animation for detonation red/white here
        StartCoroutine(explosionCounter());     //Freezing the enemy to start the countdown
    }

    void Explode()
    {
        //Explosion particles and the destruction
        var exp = Enemy02.GetComponent<ParticleSystem>();
        exp.Emit(100);
        explode1.SetActive(false);
        explode2.SetActive(false);
        Destroy(image);             //Destroys the image 
        if (inRangeDuringExplosion == true && checkOne == false)
        {
            checkOne = true;
            Debug.Log("Boom");
            //Add explosion here
            //Add explosion audio here
            
            knockback();
            playerDamage();            
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

    //This is the enemy dying
    void death()
    {
        Destroy(Enemy02);
    }

    //Checks if the object that collided with it is a bullet
    //Works when the object has a rigidbody
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            doDamage();
        }
    }


    //Driver.TakeDamage damages the enemy




   










    //This is the enemy taking damage
    void doDamage()
    {
        healthBar = healthBar - bulletDamage;
        Debug.Log(healthBar); //TODO remove later
        if (healthBar <= 0)
        {
            death();
        }
       
    }

    //This does damage to the player
    void playerDamage()
    {
        // you can call either GetComponent<PlayerDriver> or GetComponent<Driver>
        //on the player GameObject, then call TakeDamage on that object to damage 
        // the player. 
        //player.GetComponent<PlayerDriver>.TakeDamage(damageToPlayer);
        Debug.Log("Damage done: " + damageToPlayer);
        //TakeDamage(damageToPlayer);       
    }

    IEnumerator explosionCounter()  //Adds the delay to the counter
    {
        explode1.SetActive(false);
        explode2.SetActive(true);
        yield return new WaitForSeconds(timerDuration);
        if(detonateAnim == false)
        {
            detonateAnim = true;
            Explode();
        }        
    }
}
