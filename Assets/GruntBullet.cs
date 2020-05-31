using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntBullet : MonoBehaviour
{

    [SerializeField]
    private float damage = 25f;

    private AudioSource sfxSource;
    public AudioClip[] playerDamageClips;
    public float pitchMin, pitchMax;

    public Transform playerPos;

    float startTime;
    public float enemyCollisionGracePeriod = 1f;

    private const float maxExistTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        //So many stray foo fighters, harmless until one blocked my doorway
        if ((Time.time - startTime) > maxExistTime)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(playerPos.transform.position - gameObject.transform.position);
    }
    private void DamageSFX()
    {
        sfxSource.clip = playerDamageClips[Random.Range(0, playerDamageClips.Length)];
        sfxSource.pitch = Random.Range(pitchMin, pitchMax);
        sfxSource.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerDriver>().TakeDamage(damage);
            DamageSFX();
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Enemy" && Time.time - startTime > enemyCollisionGracePeriod)
        {
            Destroy(gameObject);
        }
        else if (Time.time - startTime > enemyCollisionGracePeriod)
        {
            Destroy(gameObject);
        }
    }
}
