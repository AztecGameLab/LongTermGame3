using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularDoor : Interactable
{
    public UnityEvent door = new UnityEvent();

    private Vector3 parentPosition;
    private Quaternion originalRotation;


    // Start is called before the first frame update
    void Start()
    {   
        parentPosition = transform.parent.position;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(parentPosition, Vector3.up, -30 * Time.deltaTime);
    }

    //Function to open the door
    public void openDoor()
    {
        for (int i = 0; i < 85; i+=3)
        {
            transform.RotateAround(parentPosition, Vector3.up, 3);
            print(transform.rotation.y);
        }

        //InvokeRepeating("moveDoor(85f)", .5f, .1f);
        //while(transform.rotation.y < 85f)
        //{
            //transform.RotateAround(parentPosition, Vector3.up, 3);
        //}
    }

    //Function to close the door
    public void closeDoor()
    {
        for (int i = 0; i < 85; i += 3)
        {
            transform.RotateAround(parentPosition, Vector3.up, -3);
            print(transform.rotation.y);
        }

        //while (transform.rotation.y != 0)
        //{
        //    transform.RotateAround(parentPosition, Vector3.up, -30 * Time.deltaTime);
        //}
    }

    public void moveDoor(float angle)
    {
        if (transform.rotation.y < 85f)
        {
            transform.RotateAround(parentPosition, Vector3.up, 3);
        }
    }
    
}
