using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ProceduralMapGenerator : MonoBehaviour
{
    //A new map is generated on play
    private void Awake()
    {
        Initialize();
    }
    //clears the map and generates a new map
    public void Initialize()
    {
        if (transform.childCount != 0)
            ClearMap();
        PlaceRooms(GenerateRoomData());
    }
    //edit this function to change the room generation algorithm
    private List<RoomData> GenerateRoomData()
    {
        List<RoomData> rooms = new List<RoomData>();
        RoomData testRoom = new RoomData();
        testRoom.position = Random.insideUnitSphere;
        testRoom.name = "testRoom1";
        var secondTestRoom = testRoom;
        secondTestRoom.name = "testRoom2";
        secondTestRoom.position = Random.insideUnitSphere;
        rooms.Add(testRoom);
        rooms.Add(secondTestRoom);
        return rooms;
    }
    //removes all children from this gameobject
    private void ClearMap()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if(child.gameObject!=gameObject)
                DestroyImmediate(child.gameObject);
        }
    }
    //creates a new object as a child of this script's transform according to the roomData provided
    private void PlaceRoom(RoomData room)
    {
        GameObject roomObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roomObj.transform.position = room.position;
        roomObj.name = room.name;
        roomObj.transform.parent = transform;
    }
    //iterates through each room and places them one at a time, 
    //waits a certain amount of time before placing each one
    private void PlaceRooms(List<RoomData> placements)
    {
        foreach (RoomData room in placements)
        {            
            PlaceRoom(room);
        }
    }
}
