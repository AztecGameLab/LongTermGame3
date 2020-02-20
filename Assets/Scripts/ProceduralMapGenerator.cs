using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ProceduralMapGenerator : MonoBehaviour
{
    public int maxSize;
    public int numAttempts;

    //A new map is generated on play
    private void Awake()
    {
        Initialize();
    }
    //clears the map and generates a new map
    public void Initialize()
    {
        maxSize = 0;
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
        testRoom.type = "Start";
        int width = Random.Range(1, 5);
        int height = Random.Range(1, 5);
        int length = Random.Range(1, 5);
        testRoom.size = new Vector3(width, height, length);
        int numDoors = Random.Range(1, 5); //finds random door number
        testRoom.doors = new List<DoorData>();
        fillDoors(numDoors, testRoom);
 


        //var secondTestRoom = testRoom;
        //secondTestRoom.name = "testRoom2";
        //secondTestRoom.position = Random.insideUnitSphere;


        rooms.Add(testRoom);

        //testing attemptRoom DELETE THIS TO SEE DOORS ACTUALLY SPAWN 
        foreach(DoorData door1 in testRoom.doors)
        {
            attemptCreateRoom(rooms,door1,testRoom);
        }
        //testing attemptRoom

        //rooms.Add(secondTestRoom);
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
        roomObj.transform.localScale = room.size;
        roomObj.transform.parent = transform;
        createDoors(room, roomObj);

    }
    //creates a new object as a child of this scripts transform with less roomData than normal e.g. no doors, name, etc.
    private GameObject tempPlaceRoom(RoomData room)
    {
        GameObject roomObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roomObj.transform.position = room.position;
        roomObj.transform.localScale = room.size;
        roomObj.transform.parent = transform;
        roomObj.name = room.name;
        return roomObj;
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

    //instiates all doors for respective room
    private void createDoors(RoomData room, GameObject roomObj)
    {
        foreach (DoorData door in room.doors)
        {
            GameObject doorObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            doorObj.transform.position = door.position;
            doorObj.transform.parent = roomObj.transform;
            doorObj.name = door.name;
        }
    }

    private void fillDoors(int numDoors, RoomData testRoom)
    {
        //creates doors and checks for door collision
        for (int i = 0; i < numDoors; i++)
        {
            testRoom.doors.Add(new DoorData(testRoom));
            for (int t = 0; t < i; t++)
            {
                if (testRoom.doors[i].position.Equals(testRoom.doors[t].position))
                    testRoom.doors.RemoveAt(i);
            }
        }
    }

    //fillDoors, assumes that one door is already placed
    private void fillNewRoomDoors(int numDoors, RoomData testRoom)
    {
        //creates doors and checks for door collision
        for (int i = 1; i < numDoors; i++)
        {
            testRoom.doors.Add(new DoorData(testRoom));
            for (int t = 0; t < i; t++)
            {
                if (testRoom.doors[i].position.Equals(testRoom.doors[t].position))
                    testRoom.doors.RemoveAt(i);
            }
        }
    }

    //attemps to create room at given door with a certain number of trials (each trial checks if colliding with existing room)
    //if exceeds number of trials, deletes door and results in dead end
    private bool attemptCreateRoom(List<RoomData> roomList, DoorData door, RoomData room)
    {
        for(int i = 0; i< numAttempts; i++)
        {
            bool passedAlltests = true;
            RoomData testRoom = new RoomData();
            int width = Random.Range(1, 5);
            int height = Random.Range(1, 5);
            int length = Random.Range(1, 5);
            testRoom.size = new Vector3(width, height, length);
            testRoom.position = posNearDoor(door, testRoom.size);
            testRoom.name = "testRoom";
            GameObject tempObj2 = tempPlaceRoom(testRoom);
            foreach (RoomData existRoom in roomList)
            {
                //kinda jank IDK how to do this efficiently yet
                GameObject tempObj1 = tempPlaceRoom(existRoom);

                if (!validPlacement(tempObj1,tempObj2))// check if DOES NOT collide with other rooms
                {
                    passedAlltests = false;
                }
                DestroyImmediate(tempObj1);
            }
            if (passedAlltests)
            {
                testRoom.type = "Standard";
                door.room2 = testRoom;
                int numDoors = Random.Range(2, 4); //finds random door number
                testRoom.doors = new List<DoorData>();
                testRoom.doors.Add(door);
                fillNewRoomDoors(numDoors, testRoom);
                roomList.Add(testRoom);
                DestroyImmediate(tempObj2);
                return true;
            }
            DestroyImmediate(tempObj2);
        }
        
        room.doors.Remove(door);
        return false;
    }

    //randomly tries to place rooms next to doors
    private Vector3 posNearDoor(DoorData door, Vector3 size)
    {
        int rand = Random.Range(0, 101);
        if(rand < 25)
        {
            return new Vector3(door.position.x + size.x / 2, door.position.y + size.y / 2, door.position.z + size.z / 2);
        }
        else if (rand < 50)
        {
            return new Vector3(door.position.x - size.x / 2, door.position.y + size.y / 2, door.position.z + size.z / 2);
        }
        else if (rand < 75)
        {
            return new Vector3(door.position.x + size.x / 2, door.position.y + size.y / 2, door.position.z - size.z / 2);
        }
        else
        {
            return new Vector3(door.position.x - size.x / 2, door.position.y + size.y / 2, door.position.z - size.z / 2);
        }

    }

    //finds largest vector within a given Vector3
    private float vector3Max(Vector3 vector)
    {
        if (vector.x >= vector.y && vector.x >= vector.z)
            return vector.x;
        else if (vector.y >= vector.x && vector.y >= vector.z)
            return vector.y;
        else
            return vector.z;
    }

    private float maxFloat(float float1, float float2)
    {
        if (float1 > float2)
            return float1;
        else
            return float2;
    }

    private bool validPlacement(GameObject obj1, GameObject obj2)
    {
        if (Vector3.Distance(obj1.transform.position, obj2.transform.position) > (maxFloat(vector3Max(obj1.transform.localScale), vector3Max(obj2.transform.localScale)))/2)
            return true;
        return false;
    }
}
