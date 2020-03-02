using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct RoomData 
{
    public Bounds bounds;
    public string name;
    public List<RoomData> connections;
    public string type; //defines roomType e.g. Start, Boss, Key, Goal, etc.
    //public List<DoorData> doors;
    //public Enemy[] enemy

}
