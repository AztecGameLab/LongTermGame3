using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ProceduralMapGenerator : MonoBehaviour
{
    private List<RoomData> rooms;
    public LayerMask m_LayerMask;
    public int maxSize;
    public int numAttempts;
    public bool isValidPlacementTest;

    public int xRange;
    public int yRange;
    public int zRange;
    [Range(0, 1)]
    public float branchChance = 1;
    public GameObject roomTemplate;
    Vector3[] directions =
    {
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back,
        Vector3.up,
        Vector3.down
        //add up and down     
    };

    //A new map is generated on play
    private void Awake()
    {
        Initialize();
    }

    //clears the map and generates a new map
    public void Initialize()
    {
        rooms = new List<RoomData>();
        if (transform.childCount != 0)
            ClearMap();
        GenerateRoomData(Vector3.zero, Vector3.zero);
    }
    //edit this function to change the room generation algorithm
    private void GenerateRoomData(Vector3 entrancePos, Vector3 direction)
    {
        if (rooms.Count >= maxSize)
            return;
        RoomData dumbRoom = new RoomData();
        Bounds b = new Bounds();
        b.size = GetRandomSize();
        b.center = entrancePos + Vector3.Scale(b.size / 2, direction);
        dumbRoom.bounds = b;
        dumbRoom.name = "room " + rooms.Count.ToString();
        if (CheckRoom(dumbRoom))
        {
            rooms.Add(dumbRoom);
            PlaceRoom(dumbRoom);
            var dir = directions[Random.Range(0, directions.Length)];
            GenerateRoomData(dumbRoom.bounds.center + Vector3.Scale(dir, dumbRoom.bounds.size / 2), dir);
        }
    }
    private bool CheckRoom(RoomData room)
    {
        room.bounds.Expand(-0.001f);
        foreach (RoomData placedRoom in rooms)
        {
            
            if (room.bounds.Intersects(placedRoom.bounds))
            {
                return false;
            }
           
        }
        room.bounds.Expand(0.001f);
        return true;
    }
    //removes all children from this gameobject
    private void ClearMap()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject != gameObject)
                DestroyImmediate(child.gameObject);
        }
    }
    //creates a new object as a child of this script's transform according to the roomData provided
    private void PlaceRoom(RoomData room)
    {
        GameObject roomObj = Instantiate(roomTemplate);
        roomObj.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        roomObj.transform.position = room.bounds.center;
        roomObj.name = room.name;
        roomObj.transform.localScale = room.bounds.size;
        roomObj.transform.parent = transform;

    }
    private Vector3 GetRandomSize()
    {
        int width = Random.Range(1, xRange);
        int height = Random.Range(1, yRange);
        int length = Random.Range(1, zRange);
        return new Vector3(width, height, length);
    }
}
    /*
    //creates a new object as a child of this scripts transform while returning the gameObject
    private GameObject tempPlaceRoom(RoomData room)
    {
        GameObject roomObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roomObj.transform.position = room.position;
        roomObj.name = room.name;
        roomObj.transform.localScale = room.size;
        roomObj.transform.parent = transform;
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

 /*   private bool attemptCreateRoom()
    {

    }
    
    private Vector3 GetRandomSize()
    {
        int width = Random.Range(1, xRange);
        int height = Random.Range(1, yRange);
        int length = Random.Range(1, zRange);
        return  new Vector3(width, height, length);
    }
    private RoomData CreateNewRoom(List<RoomData> roomList, RoomData room, int direction)
    {
        //for(int i = 0; i< numAttempts; i++)
        //{
            RoomData testRoom = new RoomData();
            GetRandomSize();
            testRoom.position = roomSpawnPosition(room, testRoom, direction); //TODO method that determines direction based off of int
            testRoom.name = "testRoom";
            GameObject tempObj2 = tempPlaceRoom(testRoom);
            roomList.Add(testRoom);//RMEOVEVEVEVE
            Collider[] hitColliders = Physics.OverlapBox(testRoom.position, testRoom.size / 2, Quaternion.identity, m_LayerMask);
                if (hitColliders.Length >0)// check if DOES NOT collide with other rooms
                {
                    print("fail");
                }
            if (hitColliders.Length <= 0)
            {
                testRoom.name = "roomFits";
                tempObj2.name = "roomFits";
                testRoom.type = "Standard";
               // roomList.Add(testRoom);
                //return true;
            }
        return testRoom;
           // DestroyImmediate(tempObj2);
        //}
        //return false;
       // return true;
    }

    private int oppositeDirection(int direction)
    {
        int opposite = direction % 2;
        if (opposite == 0)
        {
            return direction + 1;
        }
        else
        {
            return direction - 1;
        }
    }

    private Vector3 roomSpawnPosition(RoomData room,RoomData testRoom,int dir)
    {
        if(dir == 0)
        {
            return room.position + new Vector3(0, 0, (room.size.z / 2) + (testRoom.size.z/2));
        }else if (dir == 1)
        {
            return room.position + new Vector3(0, 0, -(room.size.z / 2) - (testRoom.size.z / 2));
        }
        else if (dir == 2)
        {
            return room.position + new Vector3((room.size.x / 2) + (testRoom.size.x / 2), 0, 0);
        }
        else
        {
            return room.position + new Vector3(-(room.size.x / 2) - (testRoom.size.x / 2), 0, 0);
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

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.name.Equals("testRoom"))
        {
            isValidPlacementTest = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.gameObject.name.Equals("testRoom"))
        {
            isValidPlacementTest = true;
        }
    }
}
*/

/*
 * 
 * 
 * 
 * 
 * 
 *     private List<RoomData> GenerateRoomData()
    {
       // print("GENERATING NEW ROOMS");
        List<RoomData> rooms = new List<RoomData>();
        //generateStartRoom(rooms);
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
        foreach (DoorData door1 in testRoom.doors)
        {
            attemptCreateRoom(rooms,door1,testRoom);
        }
        //testing attemptRoom

        //rooms.Add(secondTestRoom);
        return rooms;
    }



    *         private List<RoomData> GenerateRoomData(int startSize, List<RoomData> rooms, RoomData room)
    {
        if (startSize == 0)
        {
            rooms = new List<RoomData>();
            generateStartRoom(rooms);
            room = rooms[rooms.Count - 1];
            startSize++;
            foreach (DoorData door1 in room.doors)
            {
                if (attemptCreateRoom(rooms, door1, room))
                    GenerateRoomData(startSize+1, rooms, rooms[rooms.Count - 1]); 
            }
        }
        else if (startSize < maxSize)
        {


            //testing attemptRoom DELETE THIS TO SEE DOORS ACTUALLY SPAWN 
            foreach (DoorData door1 in room.doors)
            {
                if (attemptCreateRoom(rooms, door1, room))
                    GenerateRoomData(startSize + 1, rooms, rooms[rooms.Count - 1]);
            }
        }
        return rooms;
    }





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

        private Vector3 posNearDoor(DoorData door, Vector3 size)
    {
        int rand = Random.Range(0, 4);
        if(rand < 1)
        {
            return new Vector3(door.position.x + size.x / 2, door.position.y + size.y / 2, door.position.z);
        }
        else if (rand < 2)
        {
            return new Vector3(door.position.x - size.x / 2, door.position.y + size.y / 2, door.position.z);
        }
        else if (rand < 3)
        {
            return new Vector3(door.position.x, door.position.y + size.y / 2, door.position.z + size.z / 2);
        }
        else
        {
            return new Vector3(door.position.x, door.position.y + size.y / 2, door.position.z - size.z / 2);
        }

    }

            */
//instiates all doors for respective room
/* private void createDoors(RoomData room, GameObject roomObj)
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

        //randomly tries to place rooms next to doors
    private Vector3 posNearDoor(DoorData door, Vector3 size)
    {
        int rand = Random.Range(0, 101);
        if (rand < 25)
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

 */

/*
 *     private void GenerateRoomData(int startSize, List<RoomData> rooms, RoomData room, int direction)
{
    if (startSize == 0)
    {
        rooms = new List<RoomData>();
        generateStartRoom(rooms);
        room = rooms[rooms.Count - 1];
        startSize++;
        for(int i=0; i < 4; i++)
        {
                GenerateRoomData(startSize, rooms, room, i);
        }
    }
    else if (startSize < maxSize)
    {
            //if (attemptCreateRoom(rooms, room, direction))
            if (CreateNewRoom(rooms, room, direction))
            {
            for (int i = 0; i < 4; i++)
            {
               // if (i != direction) to make more efficient, make it so it doesn't try to spawn in direction room just came from
                    GenerateRoomData(startSize+1, rooms, rooms[rooms.Count - 1], i);

            }
        }
    }
}
*/