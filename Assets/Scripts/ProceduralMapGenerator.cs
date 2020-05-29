using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//[ExecuteInEditMode]
public class ProceduralMapGenerator : MonoBehaviour
{
    private List<RoomData> rooms;
    public LayerMask m_LayerMask;
    public int maxSize; //estimated  Max Size
    public int maxItems;
    public int numAttempts;
    public RoomData currRoom;

    public GameObject objectTemplate;

    public GameObject optimizationHitbox;
    private Bounds hitBox;

    public int zUpperBound;
    public int zLowerBound;
    public int yUpperBound;
    public int yLowerBound;
    public int xUpperBound;
    public int xLowerBound;

    [Range(0, 1)]
    public float branchChance = 1;
    public GameObject roomTemplate;
    public ProceduralRoomGenerator roomGenerator;
    public bool isGenerateInterior;
    public NavMeshSurface surface;
    public float MapScale;
    public static float _mapScale;
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
    int safetyBreakCondition = 0;
    //A new map is generated on play
    private void Awake()
    {
        hitBox.size = GetHitboxBounds();
        Initialize();
    }

    //clears the map and generates a new map
    public void Initialize()
    {
        _mapScale = MapScale;
        rooms = new List<RoomData>();
        if (transform.childCount != 0)
            ClearMap();
        
        AttemptSpawnRoom(null);
        PlaceRooms();
        transform.localScale = Vector3.one * _mapScale;

        foreach (RoomData room in rooms)
        {
            room.bounds.center *= _mapScale;
            room.bounds.extents *= _mapScale;
        }
        surface.BuildNavMesh();
        //foreach(RoomData room in rooms)
        //{
        //    for(int i =0; i < Random.Range(1, maxItems); i++)
        //    {
        //        room.objects.Add(AttemptSpawnObject(room, objectTemplate, 0)); //here you can randomize between power up, gun, key, enemy if you want with a helper method later
        //    }
        //}
    }

    //Right now, Update is used to constantly check whether to spawn rooms or not, maybe instead we can create a method that detects if the character
    // changes rooms, then update what rooms are in scope or not (rather than constantly checking), but right now everything resides in Update
    public void Update()
    {

        hitBox.center = optimizationHitbox.transform.position;
        foreach (RoomData room in rooms)
        {
            if (hitBox.Intersects(room.bounds))
            {
                room.roomObj.SetActive(true);
            }
            else
            {
                room.roomObj.SetActive(false);
            }

        }
    }


    private bool AttemptSpawnRoom(RoomData prevRoom)
    {
        if (rooms.Count >= maxSize){
           prevRoom.final=true;
            return true;
        }

        RoomData roomTemp = new RoomData();
        roomTemp.doors = new List<DoorData>();
        roomTemp.objects = new List<itemData>();
        roomTemp.obstacles = new List<GameObject>();
        bool roomFits = true;
        if (rooms.Count == 0)
        {
            roomTemp.start = true;
        }
        Vector3[] shuffledDirections = ShuffleDirection();
        //This loop needs to iterate over every possible direction a new room could be in. Preferably, it would also try a variety of sizes for each direction
        for (int i = 0; i < shuffledDirections.Length; i++)
        {
            int newDirection = i;
            roomFits = true;
            Bounds b = new Bounds();
            b.size = GetRandomSize();
            if (prevRoom != null)
            {
                b.center = prevRoom.bounds.center - Vector3.Scale(prevRoom.bounds.size / 2, new Vector3(0, 1, 0)) + Vector3.Scale(shuffledDirections[i], prevRoom.bounds.size / 2) + Vector3.Scale(b.size / 2, shuffledDirections[i]) + Vector3.Scale(b.size / 2, new Vector3(0, 1, 0)); // TODO
            }
            else
            {
                b.center = Vector3.zero;
            }
            roomTemp.bounds = b;
            roomTemp.name = "room " + rooms.Count.ToString();
           // print(room.name + ": Attempt " + i);
            if (CheckRoom(roomTemp))
            {
                roomTemp.prev = prevRoom;
                rooms.Add(roomTemp);
               


                if (roomTemp.prev != null) //backDoor references a door being created in the previous room that leads to the room that has just been created
                {

                    DoorData currDoor = new DoorData(); //currDoor references the door being made in the current room leading to the previous room
                    currDoor.wall = shuffledDirections[i]; //MIGHT BE BACKWARDS
                    currDoor.wallPosition = new Vector2Int(halfWallLength(roomTemp, shuffledDirections[i]), 0);
                    currDoor.spawn=false;
                    roomTemp.doors.Add(currDoor);

                    DoorData backDoor = new DoorData();
                    backDoor.wall = -shuffledDirections[i];
                    backDoor.wallPosition = new Vector2Int(halfWallLength(roomTemp.prev, shuffledDirections[i]), 0);
                    backDoor.spawn=true;
                    roomTemp.prev.doors.Add(backDoor);
                }


                currRoom = roomTemp;

                if (!AttemptSpawnRoom(roomTemp))
                    backTrack();
                break;

            }
            roomFits = false;
        }
        return roomFits;
    }

    private int halfWallLength(RoomData room, Vector3 direction)
    {
        if(direction == Vector3.left || direction == Vector3.right)
        {
            return (int)room.bounds.size.z / 2;
        }
        else
        {
            return (int)room.bounds.size.x / 2;
        }
    }

    private void backTrack()
    {
        if (currRoom.prev != null)
        { RoomData temp = currRoom.prev;
            while (!AttemptSpawnRoom(temp))
            {
                temp = temp.prev;
            }
        }
        else
        {
            print("CATASTROPHIC FAILURE: backtracked to start ");
        }
    }

    private Vector3[] ShuffleDirection() //attempts to find a direction that doesn't go backwards
    {
        Vector3[] temp = directions;
        int n = temp.Length - 1;
        for (int i = temp.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, n);
            Vector3 t = temp[r];
            temp[r] = temp[i];
            temp[i] = t;
        }
        return temp;
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
        if (isGenerateInterior)
        {
            room.roomObj=roomGenerator.CreateRoom(room);
                //foreach (SpawnEntity obj in room.objects)
                //{
                //    //temporarily spawns cubes in place of items
                //    GameObject roomObj = obj.Spawn();
                //    roomObj.transform.parent = GameObject.Find(room.name).transform;
                //}
        }
        else
        {
            GameObject roomObj = Instantiate(roomTemplate);
            roomObj.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            roomObj.transform.position = room.bounds.center;
            roomObj.name = room.name;
            roomObj.transform.localScale = room.bounds.size;
            roomObj.transform.parent = transform;
        }
    }

    private void PlaceRooms()
    {
        foreach(RoomData room in rooms)
        {
            PlaceRoom(room);
            //foreach(DoorData door in room.doors)
            //{
            //   // print(room.name + " " + room.bounds.size + " " + " door: " + door.wall + " " + door.wallPosition);
            //}
        }
    }
    private Vector3 GetRandomSize()
    {
        int width = Random.Range(xLowerBound, xUpperBound);
        if (width % 2 == 0)
            width ++;
        int height = Random.Range(yLowerBound, yUpperBound);
        ///if (height % 2 == 0)
         //   height++;
        int length = Random.Range(zLowerBound, zUpperBound);
        if (length % 2 == 0)
            length++;
        return new Vector3(width, height, length);
    }
    private Vector3 GetHitboxBounds()
    {
        float width = optimizationHitbox.transform.localScale.x;
        float height = optimizationHitbox.transform.localScale.y;
        float length = optimizationHitbox.transform.localScale.z;
        return new Vector3(width, height, length);
    }
    

}
/*
 * 
 *     private void PlaceRoom(RoomData room)
    {
        if (isGenerateInterior)
        {
            roomGenerator.CreateRoom(room);
                foreach (itemData obj in room.objects)
                {
                    //temporarily spawns cubes in place of items
                    //GameObject roomObj = Instantiate(obj.obj);
                    //roomObj.GetComponent<MeshRenderer>().material.color = obj.typeColor;
                  //  roomObj.transform.position = obj.pos;

                    GameObject roomObj = obj.Spawn();
                    roomObj.transform.parent = GameObject.Find(room.name).transform;
                }
        }
        else
        {
            GameObject roomObj = Instantiate(roomTemplate);
            roomObj.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            roomObj.transform.position = room.bounds.center;
            roomObj.name = room.name;
            roomObj.transform.localScale = room.bounds.size;
            roomObj.transform.parent = transform;
        }
    }
 * 
 *    public bool AttemptSpawnObject(RoomData room, GameObject obj) // tries to spawn object in random position of a room
    {
        //make 2D array of ceiling
        //shuffle 2D array
        //while isValidSpawn not true, iterate through 2d array
        //when true, add gameObject to room objects list
        //return true
        //if for some reason can't find a space, return false
        return false;

    }
 *  
 *        if(Input.GetMouseButtonDown(0)) {
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit physicsHit;
             if(Physics.Raycast(ray, out physicsHit, clickMaxDist)) {
                 NavMeshHit navmeshHit;
                 int walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
                 if(NavMesh.SamplePosition(physicsHit.point, out navmeshHit, 1.0f, walkableMask)) {
                     Agent.SetDestination(navmeshHit.position);
                 }
             }
         }
 * 
 * 
 * 
 *     public void Initialize()
    {
        rooms = new List<RoomData>();
        if (transform.childCount != 0)
            ClearMap();
        
        AttemptSpawnRoom(null);
        PlaceRooms();
        surface.BuildNavMesh();
    }

 * 
 *
 *    private int getNewDirection(int direction) //attempts to find a direction that doesn't go backwards
    {
        if (direction == directions.Length - 1)
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
 * 
 *     private bool AttemptSpawnRoom(RoomData room, Vector3 entrancePos, int direction)
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



