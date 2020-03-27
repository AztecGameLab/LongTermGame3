using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGruntDriver : MonoBehaviour
{
    [SerializeField]
    private float fireRate = 2f;

    [SerializeField]
    private float alertRadius = 30f;

    [SerializeField]
    private float fieldOfView = 120f;

    [SerializeField]
    private float alertCountdown = 1f;

    private bool isAlerted;

    private Vector3 playerVector;

    private float timeLastShot;

    Transform player;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerDriver>().transform;
        agent = GetComponent<NavMeshAgent>();
        
        isAlerted = false;
        timeLastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        playerVector = player.position - transform.position;

        if (!isAlerted)
            CheckForAlert();

        if (isAlerted && playerVector.magnitude > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
        }

        if (playerVector.magnitude <= agent.stoppingDistance)
        {
            Vector3 direction = new Vector3(playerVector.x, 0, playerVector.z).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
        if (isAlerted && (Time.time - timeLastShot >= (1 / fireRate)) && CheckLineOfSight())
            Fire();
    }
    void CheckForAlert()
    {
        if (!isAlerted && playerVector.magnitude <= alertRadius && CheckLineOfSight())
        {
            alertCountdown -= Time.deltaTime;
        }
        if(alertCountdown <= 0f)
        {
            isAlerted = true;
        }
    }
     bool CheckLineOfSight()
    {
        //layermask that ignores the player layer
        int layerMask = ~(1 << 12);
        return !Physics.Raycast(transform.position, playerVector, playerVector.magnitude, layerMask) && Vector3.Angle(transform.forward, playerVector) <= 0.5 * fieldOfView;
    }
     void Fire()
    {

    }
}
