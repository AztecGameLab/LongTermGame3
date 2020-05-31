using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour
{
    public float degreesPerSecond;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, (Time.time - startTime) * degreesPerSecond, 0));
    }
}
