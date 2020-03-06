using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ProceduralMapGenerator : MonoBehaviour
{
    private List<RoomData> rooms;
    public LayerMask m_LayerMask;
    public int maxSize; //estimated  Max Size
    public int numAttempts;
    public RoomData currRoom;

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
        //Vector3.up,
        //Vector3.down,
        Vector3.forward,
        Vector3.back,
        Vector3.zero
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
        GenerateRoomData(Vector3.zero, directions.Length -1);
    }


    //edit this function to change the room generation algorithm
    private void GenerateRoomData(Vector3 entrancePos, int direction)
    {
        if (rooms.Count >= maxSize)
            return;
        RoomData dumbRoom = new RoomData();
        if (!AttemptSpawnRoom(dumbRoom, entrancePos, direction))
            backTrack();
    }

    private bool AttemptSpawnRoom(RoomData room, Vector3 entrancePos, int direction)
    {
        bool roomFits = true;
        for (int i = 0; i < numAttempts; i++)
        {
            int newDirection = direction;
            //  int newDirection = getNewDirection(direction);
            roomFits = true;
            Bounds b = new Bounds();
            b.size = GetRandomSize();
            b.center = entrancePos + Vector3.Scale(b.size / 2, directions[newDirection]) + Vector3.Scale(b.size / 2, new Vector3(0, 1, 0));
            room.bounds = b;
            room.name = "room " + rooms.Count.ToString();
            print(room.name + ": Attempt " + i);
            if (CheckRoom(room))
            {
                rooms.Add(room);
                setPrevRoom(room);
                PlaceRoom(room);
                currRoom = room;

                var dir = Random.Range(0, directions.Length - 1); //make sure that dir doesn't doesn't corrospond to previous direction
                GenerateRoomData(room.bounds.center - Vector3.Scale(room.bounds.size / 2, new Vector3(0, 1, 0)) + Vector3.Scale(directions[dir], room.bounds.size / 2), dir);
                break;

            }
            roomFits = false;
        }
        return roomFits;
    }


    private void setPrevRoom(RoomData room)
    {
        if(rooms.Count > 1)
        {
            room.prev = rooms[rooms.Count - 2];
        }
        else
        {
            room.prev = null; //might cause errors, might need an "error Room"
        }
    }

    private void backTrack()
    {
        if (currRoom.prev != null)
        {
            var dir1 = Random.Range(0, directions.Length - 1);
            GenerateRoomData(currRoom.prev.bounds.center - Vector3.Scale(currRoom.prev.bounds.size / 2, new Vector3(0, 1, 0)) + Vector3.Scale(directions[dir1], currRoom.prev.bounds.size / 2), dir1);
        }
    }

    private int getNewDirection(int direction) //attempts to find a direction that doesn't go backwards
    {if(direction == directions.Length - 1)
        {
            return direction;
        }
        int opposite = direction % 2;
        if (opposite == 0)
        {
            opposite = direction + 1;
        }
        else
        {
            opposite = direction - 1;
        }
        int newDirection = Random.Range(0, directions.Length-1);
        while (newDirection.Equals(opposite))
        {
           newDirection = Random.Range(0, directions.Length-1);
        }
        return newDirection;
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
 * 
 *      private void GenerateRoomData(Vector3 entrancePos, int direction)
    {
        if (rooms.Count >= maxSize)
            return;
        RoomData dumbRoom = new RoomData();
        if (!AttemptSpawnRoom(dumbRoom, entrancePos, direction))
        {
            if (currRoom.prev != null)
            {
                print("going backwards");
                var dir1 = Random.Range(0, directions.Length - 1);
                GenerateRoomData(currRoom.prev.bounds.center - Vector3.Scale(currRoom.prev.bounds.size / 2, new Vector3(0, 1, 0)) + Vector3.Scale(directions[dir1], currRoom.prev.bounds.size / 2), dir1);
            }
        }
            
    }

    private bool AttemptSpawnRoom(RoomData room, Vector3 entrancePos, int direction)
    {
        bool roomFits = true;
        for(int i =0; i < numAttempts; i++)
        {
            int newDirection = direction;
          //  int newDirection = getNewDirection(direction);
            roomFits = true;
            Bounds b = new Bounds();
            b.size = GetRandomSize();
            b.center = entrancePos + Vector3.Scale(b.size / 2, directions[newDirection]) + Vector3.Scale(b.size / 2, new Vector3(0, 1, 0));
            room.bounds = b;
            room.name = "room " + rooms.Count.ToString();
            print(room.name + ": Attempt " + i);
            if (CheckRoom(room))
            {
                rooms.Add(room);
                setPrevRoom(room);
                PlaceRoom(room);
                currRoom = room;

                var dir = Random.Range(0, directions.Length - 1); //make sure that dir doesn't doesn't corrospond to previous direction
                GenerateRoomData(room.bounds.center - Vector3.Scale(room.bounds.size/2, new Vector3(0,1,0)) + Vector3.Scale(directions[dir], room.bounds.size / 2), dir); 
                break;
        
            }
            roomFits = false;
        }
        return roomFits;
    }


        private void GenerateRoomData(Vector3 entrancePos, int direction)
    {
        if (rooms.Count >= maxSize)
            return;
        RoomData dumbRoom = new RoomData();
        AttemptSpawnRoom(dumbRoom, entrancePos, direction);
    }
    //IMPLEMENTATION INCONSISTENT
    private bool AttemptSpawnRoom(RoomData room, Vector3 entrancePos, int direction)
    {
        bool roomFits = true;
        for(int i =0; i < numAttempts; i++)
        {
            int newDirection = direction;
          //  int newDirection = getNewDirection(direction);
            roomFits = true;
            Bounds b = new Bounds();
            b.size = GetRandomSize();
            b.center = entrancePos + Vector3.Scale(b.size / 2, directions[newDirection]) + Vector3.Scale(b.size / 2, new Vector3(0, 1, 0));
            room.bounds = b;
            room.name = "room " + rooms.Count.ToString();
            print(room.name + ": Attempt " + i);
            if (CheckRoom(room))
            {
                rooms.Add(room);
                setPrevRoom(room);
                PlaceRoom(room);
                currRoom = room;

                var dir = Random.Range(0, directions.Length - 1); //make sure that dir doesn't doesn't corrospond to previous direction
                GenerateRoomData(room.bounds.center - Vector3.Scale(room.bounds.size/2, new Vector3(0,1,0)) + Vector3.Scale(directions[dir], room.bounds.size / 2), dir); 
                break;
        
            }
            roomFits = false;
        }
        if (room.prev != null)
        {
            print("going backwards");
            var dir1 = Random.Range(0, directions.Length - 1);
            GenerateRoomData(room.prev.bounds.center - Vector3.Scale(room.prev.bounds.size / 2, new Vector3(0, 1, 0)) + Vector3.Scale(directions[dir1], room.prev.bounds.size / 2), dir1);
        }
        return roomFits;
    }//IMPLEMENTATION INCONSISTENT


 *     private bool AttemptSpawnRoom(RoomData room, Vector3 entrancePos, int direction)
    {
        bool roomFits = true;
        for(int i =0; i < numAttempts; i++)
        {
            int newDirection = getNewDirection(direction);
            roomFits = true;
            Bounds b = new Bounds();
            b.size = GetRandomSize();
            b.center = entrancePos + Vector3.Scale(b.size / 2, directions[newDirection]);
            room.bounds = b;
            room.name = "room " + rooms.Count.ToString();
            print(room.name + ": Attempt " + i);
            if (CheckRoom(room))
            {
                rooms.Add(room);
                PlaceRoom(room);
                var dir = Random.Range(0, directions.Length-1); //make sure that dir doesn't doesn't corrospond to previous direction
                GenerateRoomData(room.bounds.center + Vector3.Scale(directions[dir], room.bounds.size / 2), dir); //implement numAttemps
                break;
            }
            roomFits = false;
        }
        return roomFits;
    }
 * 
 * 
 *     private void GenerateRoomData(Vector3 entrancePos, Vector3 direction)
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
        var dir = directions[Random.Range(0, directions.Length)]; //make sure that dir doesn't doesn't corrospond to previous direction
        GenerateRoomData(dumbRoom.bounds.center + Vector3.Scale(dir, dumbRoom.bounds.size / 2), dir); //implement numAttemps
    }
}
 * 
 * 
 * 
 * 
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


            */



