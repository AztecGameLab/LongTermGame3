using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseButton;
    bool flagDelete = false;

    public AudioMixer masterMixer;

    // Update is called once per frame
    void Update()
    {
        //Making sure the player only pauses in the right scenes
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused == true)
            {
                Resume();
                MusicLowPassOff();
            }
            else
            {
                freeze();
                MusicLowPassOn();
            }
        }
    }

    public void MusicLowPassOn()
    {
        masterMixer.SetFloat("LowpassLvl", 0);
    }

    public void MusicLowPassOff()
    {
        masterMixer.SetFloat("LowpassLvl", -80);
    }

    //Resumes the player from the pause menu
    public void Resume()
    {
        pauseButton.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        //Locks the mouse again
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Pauses the game
    void freeze()
    {
        pauseButton.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;

        //Unlocks the mouse in order to click menu options
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //Returns to the main menu and they restart everything for the player to begin again
    public void returnToMainScreen()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }

    //Quit game here
    public void doquit()
    {
        // save any game data here
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

