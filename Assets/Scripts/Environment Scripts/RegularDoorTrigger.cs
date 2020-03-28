using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularDoorTrigger : MonoBehaviour
{
    public UnityEvent open = new UnityEvent();
    public UnityEvent close = new UnityEvent();

    //This is needed for the open and close since it's the start of the slerp
    private float timeCount = .0001f;
    bool opening = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(opening == false)
        {
            close.Invoke();
        }
    }



    private void OnTriggerStay(Collider other)
    {

        opening = true;
        open.Invoke();

    }

    private void OnTriggerExit(Collider other)
    {
        //close.Invoke();
        opening = false;
    }
}
