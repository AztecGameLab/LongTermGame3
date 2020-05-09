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

    // Start is called before the first frame update
    void Start()
    {
        sfxSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DamageSFX()
    {
        sfxSource.clip = playerDamageClips[Random.Range(0, playerDamageClips.Length)];
        //sfxSource.pitch = Random.Range(pitchMin, pitchMax);
        sfxSource.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerDriver>().TakeDamage(damage);
            DamageSFX();
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
