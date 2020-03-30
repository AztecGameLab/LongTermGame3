using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularDoor : MonoBehaviour
{

    //essentially just the fields of the parent object or the "Hinge"
    private Transform parent;
    private Vector3 parentPosition;

    //Need a boolean for whether the door is opening/cycling or closed
    private bool opening  = false;


    //VARIABLES NEEDED TO OPEN AND CLOSE DOORS
    //start rotation and the end rotation values. 
    //float is the start SLERP interpolation between start and end (0 and 140 degrees)
    private Quaternion start;
    private Quaternion end;
    private float doorAngle = 0f;

    //List of coroutines needed. Edit: technically not needed anymore because calling with quotations
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

        //initialization of coroutines. Edit: technically not needed anymore because calling with quotations
        openDoor = coroutineOpenDoor();
        closeDoor = coroutineCloseDoor();
        doorCycle = coroutineDoorCycle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //coroutine to open the door
    IEnumerator coroutineOpenDoor()
    {
        //keeps animating door opening til it's at open state(140 degrees)
        while (doorAngle < 1)
        {
            parent.rotation = Quaternion.Slerp(start, end, doorAngle);
            doorAngle += Time.deltaTime/2;

            yield return null; 
        }

    }

    //coroutine to close the door
    IEnumerator coroutineCloseDoor()
    {
        //keeps animating door closing til it's at close state(0 degrees)
        while (doorAngle > 0f)
        {
            parent.rotation = Quaternion.Slerp(start, end, doorAngle);
            doorAngle -= (Time.deltaTime/2);

            yield return null;
        }

        yield break;
    }



    IEnumerator coroutineDoorCycle()
    {
        //opens the dooor
        yield return StartCoroutine("coroutineOpenDoor");
        //leaves it open for 5 seconds
        yield return new WaitForSeconds(5f);
        //closes the door
        yield return StartCoroutine("coroutineCloseDoor");


        //terminates coroutine
        yield break;
    }
    //The method to open and close the door and the reference for the trigger's unityEvent
    public void doorCycle1()
    { 
        //Calls coroutine to open and close the door
        StartCoroutine("coroutineDoorCycle");
    }
}
