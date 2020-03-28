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

    //List of coroutines needed
    private IEnumerator closeDoor;
    private IEnumerator openDoor;
    private IEnumerator doorCycle;

    // Start is called before the first frame update
    void Start()
    {
        //initialization of the fields of the parent object
        parent = transform.parent;
        parentPosition = parent.position;

        //initialization of the rotation values needed to open and close doors
        start = parent.rotation;
        end = parent.rotation * Quaternion.Euler(0, 140, 0);

        //initialization of coroutines
        openDoor = coroutineOpenDoor();
        closeDoor = coroutineCloseDoor();
        doorCycle = coroutineDoorCycle();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(parentPosition, Vector3.up, -30 * Time.deltaTime);
    }

    //coroutine to open the door
    IEnumerator coroutineOpenDoor()
    {
        while (doorAngle < 1)
        {
            parent.rotation = Quaternion.Slerp(start, end, doorAngle);
            doorAngle += Time.deltaTime/5;

            yield return null; 
        }

    }

    //coroutine to close the door
    IEnumerator coroutineCloseDoor()
    {
        while (doorAngle > 0f)
        {
            parent.rotation = Quaternion.Slerp(start, end, doorAngle);
            doorAngle -= (Time.deltaTime/5);

            yield return null;
        }

        yield break;
    }



    IEnumerator coroutineDoorCycle()
    {
        yield return StartCoroutine("coroutineOpenDoor");

        yield return new WaitForSeconds(5f);

        StartCoroutine("coroutineCloseDoor");
        /*
        while(doorAngle > 0f)
        {
            parent.rotation = Quaternion.Slerp(start, end, doorAngle);
            doorAngle -= Time.deltaTime;

            yield return null;
        }
        */


        yield break;
    }
    public void doorCycle1()
    {
        
        StartCoroutine("coroutineDoorCycle");
    }
}
