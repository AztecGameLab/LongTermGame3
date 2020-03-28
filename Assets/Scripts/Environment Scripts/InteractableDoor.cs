using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{
    private Transform parent;
    private Vector3 parentPosition;

    //VARIABLES NEEDED TO OPEN AND CLOSE DOORS
    //start rotation and the end rotation values. 
    //float is the start SLERP interpolation between start and end (0 and 140 degrees)
    private Quaternion start;
    private Quaternion end;
    private float doorAngle = 0f;



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
        
    }

    public override void OnInteract(Driver d)
    {
        //In this specific part, I can also make it so that they can choose to open the door only, or open and close separately. Etc
        StartCoroutine("coroutineDoorCycle");
    }

    //coroutine to open door
    IEnumerator coroutineOpenDoor()
    {
        //keeps animating door opening til it's at open state(140 degrees)
        while (doorAngle < 1)
        {
            parent.rotation = Quaternion.Slerp(start, end, doorAngle);
            doorAngle += Time.deltaTime / 2;

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
            doorAngle -= (Time.deltaTime / 2);

            yield return null;
        }

        yield break;
    }

    //coroutine for the opening and closing of a door
    IEnumerator coroutineDoorCycle()
    {
        //opens the dooor
        yield return StartCoroutine("coroutineOpenDoor");
        //leaves it open for 5 seconds
        yield return new WaitForSeconds(5f);
        //closes the door
        StartCoroutine("coroutineCloseDoor");

        //terminates coroutine
        yield break;
    }
}
