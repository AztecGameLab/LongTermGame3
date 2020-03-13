using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularDoorTrigger : MonoBehaviour
{
    public UnityEvent open = new UnityEvent();
    public UnityEvent close = new UnityEvent();
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
        open.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        close.Invoke();
    }
}
