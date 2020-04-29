using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoomData 
{
    public Bounds bounds;
    public string name;
    public List<RoomData> connections;
    public List<DoorData> doors;
    public string type; //defines roomType e.g. Start, Boss, Key, Goal, etc.
    public RoomData prev;
    public List<itemData> objects;
    public GameObject roomObj;
    public List<GameObject> obstacles;

    public bool final;
    //public List<DoorData> doors;
    //public Enemy[] enemy

}
