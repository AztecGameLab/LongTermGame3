using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
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
    [SerializeField]
    private float yPos; // Likely 1 story
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

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //playerRigidBody = playerPos.GetComponent<Rigidbody>();
        myNav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
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

        if (inRangeDuringExplosion == true && checkOne == false)
        {
            checkOne = true;
            //Add explosion here
            //Add explosion audio here            
            knockback();
            playerDamage();            
        }
               
        Destroy(image);             //Destroys the image 
        Destroy(Enemy02, 3);        //Destroys the enemy gameobject after death
    }

    //This function is to knock the player back after explosion
    void knockback()    
    {
        Vector3 explosionPos = transform.position;
        //playerRigidBody.AddExplosionForce(power, explosionPos, radius, 3.0F);
    }

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

    void doDamage()
    {
        healthBar = healthBar - bulletDamage;
        Debug.Log(healthBar); //TODO remove later
    }

    //TODO be written later, player damage
    void playerDamage()
    {
        Debug.Log(damageToPlayer);
    }

    IEnumerator explosionCounter()  //Adds the delay to the counter
    {
        yield return new WaitForSeconds(timerDuration);
        if(detonateAnim == false)
        {
            detonateAnim = true;
            Explode();
        }        
    }
}
