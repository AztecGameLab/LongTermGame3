using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Image loadbar;

    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("ProceduralMap");

        while(gameLevel.progress < 1)
        {
            loadbar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }

    void Update()
    {
        print("yoooo");
    }
}
