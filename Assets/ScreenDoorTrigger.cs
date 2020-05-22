using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenDoorTrigger : MonoBehaviour
{
    public UnityEvent doorOpen = new UnityEvent();
    public UnityEvent doorClose = new UnityEvent();

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
        if(other.tag == "Player")
        {
            doorOpen.Invoke();
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            doorClose.Invoke();
        }
    }

}
