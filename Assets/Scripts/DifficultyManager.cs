using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DifficultyManager : MonoBehaviour
{
    public static int enemyCount=2;
    public int startingCount;
    public Material roofEmiss;
    public Gradient roofGrad;
    void OnEnable()
    {
        if(FindObjectsOfType<DifficultyManager>().Length!=1){
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        enemyCount=startingCount;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //print("SCENE LOADED:"+scene.name+" enemy count: "+enemyCount);
        if(scene.name=="ProceduralMap"){
            enemyCount++;
        }
        else if(scene.name=="LoadScene"){
            ;
        }
        else{
            enemyCount=2;
        }
        roofEmiss.SetColor("_EmissionColor", 1.5f * roofGrad.Evaluate((enemyCount-2)/10.0f));
        print("EC: " + enemyCount + ": " + ((enemyCount-2)/10.0f));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
