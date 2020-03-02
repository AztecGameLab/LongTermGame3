using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    public float range;
    public float speed;

    Vector3 startPosition;
 
    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(range, 0, 0);

        float timeOffset = Time.time;
        transform.position = startPosition + direction * Mathf.Sin(timeOffset * speed);
    }
}
