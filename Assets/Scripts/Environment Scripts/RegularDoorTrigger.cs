using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularDoorTrigger : MonoBehaviour
{
    public UnityEvent doorCycle = new UnityEvent();

    //bool to check if door is opening or not
    private bool opening = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Runs door cycle
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
        doorCycle.Invoke();
        
    }

}
