using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntBullet : MonoBehaviour
{

    [SerializeField]
    private float damage = 25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerDriver>().TakeDamage(damage);
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
