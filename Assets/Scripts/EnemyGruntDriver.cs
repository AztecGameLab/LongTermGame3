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

    //where the enemy is going
    private Vector3 move;

    //where the enemy is looking
    private Vector3 look;

    private bool isAlerted;

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
        if (!isAlerted && Vector3.Distance(player.position, transform.position) <= alertRadius)
        {
            isAlerted = true;
        }
        if (isAlerted)
        {
            agent.SetDestination(player.position);
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
        
    }
}
