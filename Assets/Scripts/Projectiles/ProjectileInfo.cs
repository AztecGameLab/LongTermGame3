using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : MonoBehaviour
{
    public GameObject hitPs;
    public Transform hitPosition;
    public Vector3 startPosition;
    public WeaponInfo weaponInfo;
    public float sqrMaxDistance;

    public enum Type
    {
        Standard = 0,
        Shotgun = 1,
        Sniper = 2,
        Assault = 3,
        Count
    }

    private void Start()
    {
    }

    private void Awake()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitPosition != null)
            Instantiate(hitPs, hitPosition.position, Quaternion.identity);
        else
            Debug.LogWarning("Missing particle system position");

        //  On all collision: set projectile to inactive
        GameObject target = collision.gameObject;

        AmmoTypeInfo ammoType = gameObject.GetComponent<AmmoTypeInfo>();
        AudioClip hitClip = null;

        if (target.tag == "Enemy")
        {
            Driver testTarget = target.GetComponent<Driver>();

            if (testTarget != null)
            {
                testTarget.TakeDamage(GetDamage());
            }

            hitClip = ammoType.soundOnHitEnemy;
        }
        else
        {
            hitClip = ammoType.soundOnHitWall;
        }

        TerminateWithSound(hitClip);
    }

    private void FixedUpdate()
    {
        if (weaponInfo != null)
        {
            float sqrTravelDist = (gameObject.transform.position - startPosition).sqrMagnitude;

            if (sqrTravelDist > sqrMaxDistance)
            {
                if (OnMaxDistance())
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    startPosition = gameObject.transform.position;
                }
            }
        }
    }

    //Return true to remove projectile, false to reset distance and continue
    virtual protected bool OnMaxDistance()
    {
        return true;
    }

    virtual public int GetDamage()
    {
        //To do:  If hit enemy: calculate damage from weaponInfo, ammoTypeInfo, and enemy stats
        //  'startPosition' is for determining effect of damage falloff
        return 100;
    }

    virtual public void ResetState()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    void TerminateWithSound(AudioClip sound)
    {
        if (sound != null)
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(sound);
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        StartCoroutine(DelaySetActive(sound.length));
    }

    IEnumerator DelaySetActive(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}
