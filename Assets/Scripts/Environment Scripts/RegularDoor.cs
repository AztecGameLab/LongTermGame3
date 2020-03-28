using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularDoor : Interactable
{
    public UnityEvent door = new UnityEvent();

    //essentially just the fields of the parent object or the "Hinge"
    private Transform parent;
    private Vector3 parentPosition;


    //variables needed for opening and closing door. These are the start rotation and the end rotation values. float is the speed at which the door moves
    private Quaternion start;
    private Quaternion end;
    private float doorAngle = .0001f;


    // Start is called before the first frame update
    void Start()
    {
        //initialization of the fields of the parent object
        parent = transform.parent;
        parentPosition = parent.position;

        //initialization of the rotation values needed to open and close doors
        start = parent.rotation;
        end = parent.rotation * Quaternion.Euler(0, 140, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(parentPosition, Vector3.up, -30 * Time.deltaTime);
    }

    //Function to open the door
    public void openDoor()
    {

        parent.rotation = Quaternion.Slerp(start, end, doorAngle);
        doorAngle += Time.deltaTime;
        
    }

    //Function to close the door
    public void closeDoor()
    {
        parent.rotation = Quaternion.Slerp(start, end, doorAngle);
        doorAngle -= Time.deltaTime;
    }

    public void moveDoor(float angle)
    {
        if (transform.rotation.y < 85f)
        {
            transform.RotateAround(parentPosition, Vector3.up, 3);
        }
    }
    
}
