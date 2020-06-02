using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DifficultyManager : MonoBehaviour
{
    public static int enemyCount=2;
    public int startingCount;
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
        print("SCENE LOADED:"+scene.name+" enemy count: "+enemyCount);
        if(scene.name=="ProceduralMap"){
            enemyCount++;
        }
        else{
            enemyCount=2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
