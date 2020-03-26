using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : MonoBehaviour
{
    public Vector3 startPosition;
    public WeaponInfo weaponInfo;

    void OnCollisionEnter(Collision collision)
    {
        //To do:  If hit enemy: calculate damage from weaponInfo, ammoTypeInfo, and enemy stats
        //  On all collision: set projectile to inactive
        //  'startPosition' is for determining effect of damage falloff
        GameObject target = collision.gameObject;

        AmmoTypeInfo ammoType = gameObject.GetComponent<AmmoTypeInfo>();
        AudioClip hitClip = null;

        if (target.tag == "Enemy")
        {
            TestTarget testTarget = target.GetComponent<TestTarget>();

            if (testTarget != null)
            {
                testTarget.ResetTarget();
            }

            hitClip = ammoType.soundOnHitEnemy;
        }
        else
        {
            hitClip = ammoType.soundOnHitWall;
        }

        TerminateWithSound(hitClip);
    }

    private void TerminateWithSound(AudioClip sound)
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(sound);

        //Vector3 scalePrior = gameObject.transform.localScale;
        //gameObject.transform.localScale = new Vector3(0, 0, 0);
        //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        StartCoroutine(DelaySetActive(sound.length));
    }

    IEnumerator DelaySetActive(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
        //gameObject.transform.localScale = localScale;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private void Start()
    {
    }

    private void Awake()
    {
    }

    void FixedUpdate()
    {
        if (weaponInfo != null)
        {
            float distanceScale = 10.0f;  //Scale to current value range in weaponInfo
            float sqrMaxDistance = weaponInfo.effectiveRange * weaponInfo.effectiveRange * distanceScale;
            float sqrTravelDist = (gameObject.transform.position - startPosition).sqrMagnitude;

            if (sqrTravelDist > sqrMaxDistance)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
