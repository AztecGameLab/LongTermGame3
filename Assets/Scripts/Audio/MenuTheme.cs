using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTheme : MonoBehaviour
{
    public AudioClip menuClip;
    private AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        StartCoroutine(MenuLoop());
    }

    IEnumerator MenuLoop()
    {
        yield return new WaitForSecondsRealtime(24.548f);

        // make sure the loop volume starts at 0
        musicSource.volume = 1;
        musicSource.clip = menuClip;
        musicSource.Play();
        musicSource.loop = true;
    }
}
