using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : Driver
{
    public float range;
    public float speed;
    public float timeStart;

    Vector3 startPosition;
 
    void Start()
    {
        timeStart = Time.time;
        startPosition = transform.position;
    }

    protected override void OnDeath()
    {
        StartCoroutine(OnResetTarget());
    }

    private IEnumerator OnResetTarget()
    {
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        mr.enabled = false;

        yield return new WaitForSeconds(0.5f);

        timeStart = Time.time;
        SetTargetPosition();
        mr.enabled = true;
    }

    void FixedUpdate()
    {
        SetTargetPosition();
    }

    void SetTargetPosition()
    {
        Vector3 direction = new Vector3(range, 0, 0);

        float timeOffset = Time.time - timeStart;
        transform.position = startPosition + direction * Mathf.Sin(timeOffset * speed);

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        Quaternion q = Quaternion.LookRotation(transform.position - camera.transform.position, Vector3.up);
        transform.rotation = q;
    }
}
