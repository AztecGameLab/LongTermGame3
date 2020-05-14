using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    private AudioSource sfxSource;
    public AudioClip[] uiSFX;
    private Scene currentScene;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        sfxSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyComponent();
    }

    private void DestroyComponent()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "ProceduralMap")
        {
            Destroy(gameObject);
        }
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
