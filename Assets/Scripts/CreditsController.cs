using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    public Text groupTxt;
    public Text namesTxt;

    private float nextActionTime = 0.0f;
    public float period = 10f;

    private String[] group = {"Creative Director","Art", "Audio","General Programmers", "Procedural Programmers",""};
    private String[] namesByGroup =
        {"Kain Shepard\n",
        "Brittany Hughes\n Daniel Kenton\n Elizabeth Newgard\n Joseph Hartman\n Katrina Javier\n Mitchell Kim\n Mustafa Alrubaiee\n Naseem Tahbaz\n",
        "Byron Beasley\n",
        "Alexander Miller\n Archer Hovey\n Lauryn Jefferson\n Kyle McLain Kane\n Jacob Golden-Needham\n Ryan Wright\n Timothy Lieu\n",
        "Carter Andrews\n Christian Hong\n Dan Blackford\n Jacob Barger\n Logan Wang\n",
        ""};
    
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        InvokeRepeating("NextSlide", 5.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextSlide()
    {
        if (index < group.Length-1)
        {
            index++;
            UpdateText();
        }
    }

    public void UpdateText()
    {
        groupTxt.text = group[index];
        namesTxt.text = namesByGroup[index];
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
