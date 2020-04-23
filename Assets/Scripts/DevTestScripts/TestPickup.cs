using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPickup : MonoBehaviour
{
    float angularVelocity;

    // Start is called before the first frame update
    void Start()
    {
        angularVelocity = 120.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = angularVelocity * Time.time;



        Debug.Log(angle);

        gameObject.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}
