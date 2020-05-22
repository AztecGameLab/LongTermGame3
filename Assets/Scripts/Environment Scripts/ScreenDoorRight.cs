using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDoorRight : MonoBehaviour
{
    //Need a boolean for whether the door is opening/cycling or closed
    private bool opening = false;


    //VARIABLES NEEDED TO OPEN AND CLOSE DOORS
    //start rotation and the end rotation values. 
    //float is the start SLERP interpolation between start and end (0 and 140 degrees)
    //distance scales how far the doors move when sliding open
    private Vector3 start;
    private Vector3 end;
    private float doorPosition = 0f;
    private float distance = 2f;

    //List of coroutines needed. Edit: technically not needed anymore because calling with quotations
    private IEnumerator closeDoor;
    private IEnumerator openDoor;
    private IEnumerator doorCycle;

    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        end = transform.position + (transform.right*distance);

        //initialization of coroutines. Edit: technically not needed anymore because calling with quotations
        openDoor = coroutineOpenDoor();
        closeDoor = coroutineCloseDoor();
        doorCycle = coroutineDoorCycle();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown("e"))
        {
            doorOpen();
        }
        */
    }

    // COROUTINE TO OPEN THE DOOR. THIS IS THE ONE THAT WILL BE MAINLY USED
    IEnumerator coroutineOpenDoor()
    {
        //keeps animating door opening til it's completely open
        while (doorPosition < 1)
        {
            transform.position = Vector3.Lerp(start, end, doorPosition);
            doorPosition += Time.deltaTime / 2;

            yield return null;
        }

    }

    //coroutine to close the door
    IEnumerator coroutineCloseDoor()
    {
        //keeps animating door closing til it's at close state(0 degrees)
        while (doorPosition > 0f)
        {
            transform.position = Vector3.Lerp(start, end, doorPosition);
            doorPosition -= (Time.deltaTime / 2);

            yield return null;
        }

        yield break;
    }


    // Won't really be used now since we only need to open the door
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
    //The method to open the door and the reference for the trigger's unityEvent
    public void doorOpen()
    {
        //Calls coroutine to open the door
        StartCoroutine("coroutineOpenDoor");
    }

    public void doorClose()
    {
        //Calls coroutine to close the door
        StartCoroutine("coroutineCloseDoor");
    }
}
