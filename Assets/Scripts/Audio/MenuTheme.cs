using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTheme : MonoBehaviour
{
    public AudioClip[] menuClips;
    public AudioClip[] uiSFX;
    public AudioSource musicSource1;
    public AudioSource musicSource2;
    public AudioSource sfxSource;
    private Scene currentScene;
    private bool playMenuMusic;
    private static MenuTheme instance;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        musicSource1.PlayOneShot(menuClips[0]);
        StartCoroutine(Layer_1());
        StartCoroutine(Layer_2());
    }

    private void Update()
    {
        DestroyComponent();
        PlayerLayer2();
    }

    private void DestroyComponent()
    {
        currentScene = SceneManager.GetActiveScene();
        if (instance == this)
        {
            if (currentScene.name == "ProceduralMap")
            {
                Destroy(gameObject);
            }
        }
    }

    private void PlayerLayer2()
    {
        if (currentScene.name == "Prologue")
        {
            musicSource2.volume = 1.0f;
        }
    }

    IEnumerator Layer_1()
    {
        yield return new WaitForSecondsRealtime(24.548f);
        musicSource1.volume = 1;
        musicSource1.clip = menuClips[1];
        musicSource1.Play();
        musicSource1.loop = true;
    }
    IEnumerator Layer_2()
    {
        yield return null;
        // make sure the loop volume starts at 0
        musicSource2.volume = 0;
        musicSource2.clip = menuClips[2];
        musicSource2.Play();
        musicSource2.loop = true;
    }

    public void PlayFirstSFX()
    {
        sfxSource.PlayOneShot(uiSFX[0]);
    }

    public void PlaySecondtSFX()
    {
        sfxSource.PlayOneShot(uiSFX[1]);
    }
}
