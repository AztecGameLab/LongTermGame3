﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class RoomManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public UnityEvent doorOpen;
    bool open =false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool enemiesGone=true;
        foreach(GameObject enemy in enemies){
            if(enemy!=null){
                enemiesGone=false;
            }
        }
        if(enemiesGone){
            OpenSesame();
            
        }
    }
    public void OpenSesame(){
        if(doorOpen!=null){
        doorOpen.Invoke();
        open=true;
        }
    }
}
