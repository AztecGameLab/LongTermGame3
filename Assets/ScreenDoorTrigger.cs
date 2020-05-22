using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenDoorTrigger : MonoBehaviour
{
    public UnityEvent doorOpen = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if(opening == false)
        {
            opening = true;
            doorCycle.Invoke();
            opening = false;
        }
        */
        doorOpen.Invoke();

    }

}
