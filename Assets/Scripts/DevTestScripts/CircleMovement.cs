using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    Vector3 startpos;
    public float speed = 1;
    public float radius = 5;

    void Start()
    {
        startpos = transform.position;
    }


    void Update()
    {
        transform.position = startpos + new Vector3(Mathf.Cos(Time.time * speed * 2 * Mathf.PI) * radius, 0, Mathf.Sin(Time.time * speed * 2 * Mathf.PI) * radius);
        transform.eulerAngles = new Vector3(0, -Time.time * 360 * speed, 0);
    }
}
