using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemData 
{
    public GameObject obj;
    public string name;
    public string type;
    public Vector3 pos;

    public itemData(GameObject obj)
    {
        this.obj = obj;
        this.name = obj.name;
    }
    public itemData(GameObject obj, string name)
    {
        this.obj = obj;
        this.name = name;
    }
}
