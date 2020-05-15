using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SFX : MonoBehaviour
{
    private AudioSource sfxSource;
    public AudioClip[] uiSFX;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        sfxSource = GetComponent<AudioSource>();
    }

    public void PlayFirstSFX()
    {
        sfxSource.PlayOneShot(uiSFX[0]);
    }

    public void PlaySecondSFX()
    {
        StartCoroutine(SecondSFX());
    }

    IEnumerator SecondSFX()
    {
        sfxSource.PlayOneShot(uiSFX[1]);
        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);
    }
}
