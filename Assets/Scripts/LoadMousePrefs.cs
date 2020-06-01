using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMousePrefs : MonoBehaviour
{
    void Start()
    {
        //Using just one script for two possible control updates
        Slider sensitivity = gameObject.GetComponent<Slider>();
        Toggle invertY = gameObject.GetComponent<Toggle>();

        if (sensitivity != null)
        {
            sensitivity.value = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
        }
        else if (invertY != null)
        {
            float invertMouseY = PlayerPrefs.GetFloat("InvertMouseY", 1.0f);

            invertY.isOn = (invertMouseY < 0.0f);
        }
    }

    void Update()
    {
        
    }
}
