using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemData 
{
    public GameObject obj;
    public string name;
    public string typeName;
    public int type;
    public Color typeColor;
    public Vector3 pos;
    public enum itemType
    {
        Enemy,
        Gun,
        Pickup
    }
    public itemData(GameObject obj, string name)
    {
        this.obj = obj;
        this.name = name;
        type = getType();
        typeColor = getItemColor();
    }
    public itemData(GameObject obj)
    {
        this.obj = obj;
        this.name = obj.name;
        type = getType();
        typeColor = getItemColor();
    }
    public itemData()
    {
        type = (int)Random.Range(0, 3);
        typeColor = getItemColor();
    }


    public Color getItemColor()
    {
        if(type == 0) // Enemy
        {
            return Color.red;
        }else if (type == 1) // Powerup
        {
            return Color.green;
        }
        else // Gun
        {
            return Color.blue;
        }
    }


    //can be used to weight spawning of certain types of spawnableItems
    public int getType()
    {
        int temp = Random.Range(0, 100);
        if (temp < 50)
            return 0;
        if (temp < 80)
            return 1;
        return 2;
    }

    public GameObject Spawn(Transform parent)
    {
        GameObject roomObj = GameObject.Instantiate(obj,parent);
        //roomObj.GetComponent<MeshRenderer>().material.color = typeColor;
        roomObj.transform.position = pos;
        roomObj.transform.Rotate(0, Random.Range(0,361), 0); //randomly orientates object
        roomObj.transform.localScale /= ProceduralMapGenerator._mapScale;
        return roomObj;

        /*
        if (!obj)
        {
            GameObject roomObj = GameObject.Instantiate(obj);
            roomObj.GetComponent<MeshRenderer>().material.color = typeColor;
            roomObj.transform.position = pos;
            return roomObj;
        }
        else //load random items for resource folders
        {
            if (type == 0) // Enemy
            {
                GameObject roomObj = GameObject.Instantiate(Resources.Load("Assets/Prefab/spawnableItems/RandomEnemy"));
            }
            else if (type == 1) // Powerup
            {
                GameObject roomObj = GameObject.Instantiate(Resources.Load("Assets/Prefab/spawnableItems/RandomPowerUp"));
            }
            else // Gun
            {
                GameObject roomObj = GameObject.Instantiate(Resources.Load("Assets/Prefab/spawnableItems/RandomGun")); // might be a seperate script from gun team
            }
            roomObj.transform.position = pos;
            return roomObj;
        }     
        */

    }

    public GameObject Spawn(Transform parent, Vector3 target)  //overloaded Spawn with specific rotation  Quaternion targetRotation Quaternion targetRotation
    {
        //Debug.Log(target.x + "," + target.y + "," + target.z);
        GameObject roomObj = GameObject.Instantiate(obj, parent);
        //roomObj.GetComponent<MeshRenderer>().material.color = typeColor;
        roomObj.transform.position = pos; 
        Vector3 target1 = new Vector3(target.x,roomObj.transform.position.y *ProceduralMapGenerator._mapScale, target.z);


        roomObj.transform.rotation = Quaternion.LookRotation(roomObj.transform.position * ProceduralMapGenerator._mapScale - target1);
        roomObj.transform.Rotate(new Vector3(0, 180, 0));
        roomObj.transform.localScale /= ProceduralMapGenerator._mapScale;

        return roomObj;
    }




}
