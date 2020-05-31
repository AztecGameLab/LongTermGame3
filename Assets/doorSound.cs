using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSound : MonoBehaviour
{
    AudioSource audioSource;

    bool played;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void DoorOpenSound()
    {
        if(played)
            return;

        audioSource.Play();
        played = true;
    }
}
