using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSlerp : MonoBehaviour
{
    private Transform parent;
    Quaternion start;
    Quaternion end;
    private float timeCount = .0001f;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        start = parent.rotation;
        end = parent.rotation * Quaternion.Euler(0, 140, 0);
    }


    // Update is called once per frame
    void Update()
    {
        parent.rotation = Quaternion.Slerp(start, end, timeCount);
        timeCount = timeCount + Time.deltaTime;
    }
    

}
