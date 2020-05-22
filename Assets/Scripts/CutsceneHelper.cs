using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHelper : MonoBehaviour
{
    public void NextCutscene()
    {
        CutsceneManager.instance.NextCutscene();
    }
}
