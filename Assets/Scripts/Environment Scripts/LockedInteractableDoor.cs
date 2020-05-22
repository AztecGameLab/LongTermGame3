using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedInteractableDoor : Interactable
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
        end = parent.rotation * Quaternion.Euler(0, 160, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public override void OnInteract(Driver d)
    {
        if(d.keyCount > 0)
        {
            StartCoroutine("coroutineOpenDoor");
            d.keyCount -= 1;
        }
        
    }
}
