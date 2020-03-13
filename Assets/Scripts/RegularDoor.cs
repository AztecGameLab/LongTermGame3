using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularDoor : Interactable
{
    public UnityEvent door = new UnityEvent();

    private Vector3 parentPosition;


    // Start is called before the first frame update
    void Start()
    {   
        parentPosition = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(parentPosition, Vector3.up, -30 * Time.deltaTime);
    }

    //Function to open the door
    public void openDoor()
    {
        while(transform.rotation.y > -130f)
        {
            transform.RotateAround(parentPosition, Vector3.up, -30 * Time.deltaTime);
        }
    }

    //Function to close the door
    public void closeDoor()
    {
        while (transform.rotation.y != 0)
        {
            transform.RotateAround(parentPosition, Vector3.up, 30 * Time.deltaTime);
        }
    }
    
}
