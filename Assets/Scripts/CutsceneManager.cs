﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public int cutsceneIndex;
    public Animation[] cutscenes;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LoadScene");
        }
    }

    public void NextCutscene()
    {
        if (cutsceneIndex + 1 < cutscenes.Length)
        {
            cutscenes[cutsceneIndex].gameObject.SetActive(false);
            cutscenes[cutsceneIndex + 1].gameObject.SetActive(true);
            cutsceneIndex++;
        }
        else
        {
            SceneManager.LoadScene("LoadScene");
        }


    }
}
