using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    int AGL;
    public GameObject easterEggCanvas;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            AGL = 1;
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            if(AGL == 1)
                AGL = 2;
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            if(AGL == 2)
            {
                easterEggCanvas.SetActive(!easterEggCanvas.activeSelf);
                AGL = 0;
            }
        }
        else if(Input.anyKeyDown)
        {
            AGL = 0;
        }
    }
}
