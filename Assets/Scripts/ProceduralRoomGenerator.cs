using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRoomGenerator : MonoBehaviour
{
    private RoomData myRoom;
    public bool defaultRoomData=false;
    public RoomData testData;

    
    public GameObject doorPrefab;
    public Vector2 doorDimensions;
    Transform roomLocation;
    public Transform map;
    public float brightness;
    public GameObject[] potentialItemSpawns;
    public GameObject exitTeleporter;
    [Range(20, 60)]
    public int numObstacles = 50;
    public bool rotateObstacles;
    public GameObject wallTile;
    public GameObject cielingTile;
    public GameObject floorTile;
    // Start is called before the first frame update
    void Start()
    {
        if (defaultRoomData)
        {
            
            CreateRoom(testData);
        }
    }
    public GameObject CreateRoom(RoomData data)
    {
        myRoom = data;
        roomLocation = new GameObject(myRoom.name).transform;
        roomLocation.position = myRoom.bounds.center / 2;
        roomLocation.parent = map;
        //six surfaces for a basic room
        CreateSurface(Vector3.up, (int) myRoom.bounds.size.z, (int)myRoom.bounds.size.x, myRoom.bounds.extents.y);
        CreateSurface(Vector3.down, (int)myRoom.bounds.size.z, (int)myRoom.bounds.size.x, myRoom.bounds.extents.y);
        CreateSurface(Vector3.left, (int)myRoom.bounds.size.y, (int)myRoom.bounds.size.z, myRoom.bounds.extents.x);
        CreateSurface(Vector3.right, (int)myRoom.bounds.size.y, (int)myRoom.bounds.size.z, myRoom.bounds.extents.x);
        CreateSurface(Vector3.forward, (int)myRoom.bounds.size.y, (int)myRoom.bounds.size.x, myRoom.bounds.extents.z);
        CreateSurface(Vector3.back, (int)myRoom.bounds.size.y, (int)myRoom.bounds.size.x, myRoom.bounds.extents.z);
        CreateLight(data,roomLocation);
        if(data.final)
            data.objects.Add(AttemptSpawnObject(data,exitTeleporter , 0,roomLocation));
        else{
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                data.objects.Add(AttemptSpawnObject(data, potentialItemSpawns[Random.Range(0,potentialItemSpawns.Length)], 0,roomLocation)); //here you can randomize between power up, gun, key, enemy if you want with a helper method later
            }
            for (int i = 0; i < getVolume(data) / Random.Range(40, 50); i++) //obstacle number and obstacle spawning
            {
                data.obstacles.Add(AttemptSpawnObstacle(data, roomLocation)); //here you can randomize between power up, gun, key, enemy if you want with a helper method later
            }
        }
        return roomLocation.gameObject;
        //Floor();
        //Walls();
        //Ceiling();
    }
    public itemData AttemptSpawnObject(RoomData room, GameObject obj, int ySpawnOffset,Transform parent) // tries to spawn object in random position of a room
    {
        // find random 2d point on ceiling
        Vector3 roomCornerOffset = room.bounds.center - (room.bounds.size / 2);
        int randomCX = (int)Random.Range(1, room.bounds.size.x);
        int randomCY = (int)Random.Range(1, room.bounds.size.z);
        Vector3 ceilingPos = roomCornerOffset + new Vector3(randomCX, (float)(room.bounds.size.y - 1), randomCY); // make add from corner of position of room
        while (!validSpawnPosition(ceilingPos, room))
        {

            randomCX = (int)Random.Range(1, room.bounds.size.x);
            randomCY = (int)Random.Range(1, room.bounds.size.z);
            ceilingPos = roomCornerOffset + new Vector3(randomCX, (float)(room.bounds.size.y - 1), randomCY);
        }

        //  Ray ray = new Ray(ceilingPos, Vector3.down);
        //    Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 10);

        RaycastHit hit;
        Ray ray1 = new Ray(ceilingPos, Vector3.down);
        //  Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 100);
        Physics.Raycast(ray1, out hit, room.bounds.size.y);

        itemData temp = new itemData(obj); // make spawn off of prefab later TODOTDO
        temp.pos = hit.point + new Vector3(0, ySpawnOffset, 0);

        // temp.name =(room.name + " object: " + temp.transform.position);
        temp.name = "tempItem";
        //print(temp.name);
        temp.Spawn(parent);
        return temp;


        //when true, add gameObject to room objects list
        //if for some reason can't find a space, return null

    }

    public GameObject AttemptSpawnObstacle(RoomData room, Transform parent) // tries to spawn object in random position of a room
    {
        int spawnSide = Random.Range(0, 1);
        GameObject temp;
        // find random 2d point on ceiling
        Vector3 roomCornerOffset = room.bounds.center - (room.bounds.size / 2);
        int randomCX = (int)Random.Range(1, room.bounds.size.x);
        int randomCY = (int)Random.Range(1, room.bounds.size.z);

        Vector3 surfacePos;
        if(spawnSide == 0)
            surfacePos = roomCornerOffset + new Vector3(randomCX, (float)(room.bounds.size.y - 1), randomCY); // make add from corner of position of room ceiling pos
        else
            surfacePos = roomCornerOffset + new Vector3(randomCX, (float)(1), randomCY); // make add from corner of position of room floor pos


        //  Ray ray = new Ray(ceilingPos, Vector3.down);
        //    Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 10);

        RaycastHit hit;
        Ray ray1;
        if (spawnSide == 0)
            ray1 = new Ray(surfacePos, Vector3.down);
        else
            ray1 = new Ray(surfacePos, Vector3.up);
        //  Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 100);
        Physics.Raycast(ray1, out hit, room.bounds.size.y);

        int randomObstacle = (int)Random.Range(0, 3); //maybe add a weighted system for obstacles
        if (randomObstacle == 0)
        {
            temp = buildPillar(room, false,spawnSide);
            temp.transform.position = hit.point;
            if(rotateObstacles)
                temp.transform.Rotate(0, Random.Range(0, 361), 0); //rotates obstacles, has possibility of blocking doors so do not implement yet

        }
        else if (randomObstacle == 1)
        {
            temp = buildWall(room, (int)hit.point.x, spawnSide);
            temp.transform.position = hit.point;
            if (rotateObstacles)
                temp.transform.Rotate(0, Random.Range(0, 361), 0); //rotates obstacles, has possibility of blocking doors so do not implement yet


        }
        else
        {
            temp = GameObject.CreatePrimitive(PrimitiveType.Cube); // make spawn off of prefab later TODOTDO
            if (spawnSide == 0)
                temp.transform.position = hit.point + new Vector3(0, .5f, 0);

            else
                temp.transform.position = hit.point + new Vector3(0, -.5f, 0);

            if (rotateObstacles)
                temp.transform.Rotate(0, Random.Range(0, 361), 0); //rotates obstacles, has possibility of blocking doors so do not implement yet
        }

        // temp.name =(room.name + " object: " + temp.transform.position);
        temp.name = "tempObstacle";
        //print(temp.name);
        temp.transform.parent = parent;
        return temp;


        //when true, add gameObject to room objects list
        //if for some reason can't find a space, return null

    }


    public GameObject buildPillar(RoomData room, bool isWall, int side)
    {

        GameObject temp = new GameObject();
        if (!isWall)
        {
            for (int i = 0; i < Random.Range(2, room.bounds.size.y + 1); i++)
            {
                GameObject pillarObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if(side ==0)
                    pillarObj.transform.position = new Vector3(0, .5f + .5f * i, 0);
                else
                    pillarObj.transform.position = new Vector3(0, -.5f + -.5f * i, 0);

                pillarObj.transform.parent = temp.transform;
            }
        }
        else
        {
            for (int i = 0; i < room.bounds.size.y + 2; i++)
            {
                GameObject pillarObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (side == 0)
                    pillarObj.transform.position = new Vector3(0, .5f + .5f * i, 0);
                else
                    pillarObj.transform.position = new Vector3(0, -.5f + -.5f * i, 0);
                pillarObj.transform.parent = temp.transform;
            }
        }

        return temp;
    }

    public GameObject buildWall(RoomData room, int hitX, int side)
    {
        GameObject temp = new GameObject();
        int wallLength = (int)room.bounds.center.x + ((int)room.bounds.size.x / 2) - hitX;
        for (int i = 0; i < wallLength; i++)
        {
            GameObject wallObj = buildPillar(room, true, side);
            wallObj.transform.position = new Vector3(i * .5f, 0, 0);
            wallObj.transform.parent = temp.transform;
        }
        return temp;
    }

    public int findMinDim(RoomData room)
    {
        if (room.bounds.size.x < room.bounds.size.z)
        {
            return (int)room.bounds.size.x;
        }
        else
        {
            return (int)room.bounds.size.z;

        }
    }

    public int getVolume(RoomData room)
    {
        return (int)(room.bounds.size.x * room.bounds.size.y * room.bounds.size.z);
    }

    public bool validSpawnPosition(Vector3 source, RoomData room) //TODO make Vector3 at some point to spawn item
    {
        float MAX_DISTANCE_FROM_POINT = room.bounds.size.y + 10;
        RaycastHit hit;
        Ray ray = new Ray(source, Vector3.down);
        //  Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 100);
        if (Physics.Raycast(ray, out hit, MAX_DISTANCE_FROM_POINT))
        {
            if (hit.collider.name.Equals("surface"))
            {
                return true;
            }
        }
        return false;
    }
    public void CreateLight(RoomData data,Transform room)
    {
        Light light = new GameObject().AddComponent<Light>();
        light.transform.position = data.bounds.center;
        light.type = LightType.Point;
        light.range = data.bounds.extents.magnitude*brightness;
        light.gameObject.name = "light";
        light.intensity = brightness;
        light.transform.parent = room;
    }
    //creates a surface facing the <dir> direction sized <width> x <height> and <distance> away from the center
    private void CreateSurface(Vector3 dir , int width, int height, float distance)
    {
        //create an object to hold all the tiles in the wall
        GameObject surface = new GameObject("surface");
        surface.transform.parent = roomLocation;
        //move it <distance> out from center
        surface.transform.localPosition = roomLocation.position + (-dir) * (distance);
        //make it face dir
        surface.transform.rotation = Quaternion.LookRotation(-dir);
        if (dir == Vector3.up)
        {
            var col= surface.AddComponent<BoxCollider>();
            col.extents = new Vector3(height,width,0.01f)/2;
            surface.isStatic = true;
        }
        //spawn tiles
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject tile = TileDecision(dir,height,width,i,j);
                //check if a door should be here
                
                //if there was no door make a wall
                
                tile = Instantiate(tile, surface.transform);
                tile.transform.localPosition = new Vector3(i - (height / 2.0f) + 0.5f, j - width / 2.0f + 0.5f);
                
            }
        }
        
    }
    private GameObject TileDecision(Vector3 direction,int height, int width, int y,int x)
    {
        bool edge = (height - 1 == y)||(width-1==x)||(y==0)||(x==0);
        bool corner = ((height - 1 == y) && (width - 1 == x)) ||
                    ((0 == y) && (0 == x)) ||
                    ((0== y) && (width - 1 == x)) ||
                    ((height - 1 == y) && (0== x));
        if (direction == Vector3.down)
        {
            return cielingTile;
        }
        else if (Vector3.up== direction)
        {
            return floorTile;
        }
        else
        {
            return WallDecision(direction,y, x, edge, corner);
        }
        
    }
    
    private GameObject WallDecision(Vector3 direction,int y, int x, bool edge, bool corner)
    {
        foreach (DoorData d in myRoom.doors)
        {
            

            if (d.wall == direction && d.wallPosition == new Vector2Int(y, x))
            {
                return doorPrefab;
                
            }
        }
        return wallTile;
    }
    ////create the floor
    //private void Floor()
    //{
    //    for(int i = 0; i < myRoom.bounds.size.x;i++)
    //    {
    //        for (int j = 0; j < myRoom.bounds.size.z;j++)
    //        {
    //            GameObject tile = Instantiate(floorTile, transform);
    //            tile.transform.localPosition = -myRoom.bounds.extents+new Vector3(i, -0.5f, j);
    //            tile.transform.rotation = Quaternion.LookRotation(Vector3.down);
    //        }
    //    }
    //}

    //private void Walls()
    //{
    //    for (int i = 0; i < myRoom.bounds.size.y ; i++)
    //    {
    //        for (int j = 0; j < myRoom.bounds.size.z ; j++)
    //        {
    //            foreach (DoorData doors in myRoom.doors)
    //            {
    //                //left wall
    //                GameObject tile;
    //                if (!(doors.wallPosition.x == j && doors.wallPosition.y == i && doors.wallSide.Equals("left")))
    //                {
    //                    tile = Instantiate(floorTile, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(0.5f, i, j) + Vector3.right * (myRoom.bounds.size.x - 1);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.right);
    //                }
    //                else
    //                {
    //                    tile = Instantiate(doorPrefab, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(0.5f, i, j) + Vector3.right * (myRoom.bounds.size.x - 1);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.right);
    //                }

    //                //right wall
    //                if (!(doors.wallPosition.x == j && doors.wallPosition.y == i && doors.wallSide.Equals("right")))
    //                {
    //                    tile = Instantiate(floorTile, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(-0.5f, i, j);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.left);
    //                }
    //                else
    //                {
    //                    tile = Instantiate(doorPrefab, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(-0.5f, i, j);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.left);
    //                }

    //            }

    //        }
    //    }
    //    for (int i = 0; i < myRoom.bounds.size.y ; i++)
    //    {
    //        for (int j = 0; j < myRoom.bounds.size.x ; j++)
    //        {
    //            GameObject tile;
    //            //back wall
    //            foreach (DoorData doors in myRoom.doors)
    //            {
    //                if (!(doors.wallPosition.x == j && doors.wallPosition.y == i))
    //                {
    //                    tile = Instantiate(floorTile, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(j, i, 0.5f) + Vector3.forward * (myRoom.bounds.size.z - 1);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    //                }
    //                else
    //                {
    //                    tile = Instantiate(doorPrefab, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(j, i, 0.5f) + Vector3.forward * (myRoom.bounds.size.z - 1);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    //                }
    //                //forward wall
    //                if (!(doors.wallPosition.x == j && doors.wallPosition.y == i))
    //                {
    //                    tile = Instantiate(floorTile, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(j, i, -0.5f);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.back);
    //                }
    //                else
    //                {
    //                    tile = Instantiate(doorPrefab, transform);
    //                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(j, i, -0.5f);
    //                    tile.transform.rotation = Quaternion.LookRotation(Vector3.back);
    //                }
    //            }

    //        }
    //    }
    //}
    //private void Ceiling() {
    //    for (int i = 0; i < myRoom.bounds.size.x ; i++)
    //    {
    //        for (int j = 0; j < myRoom.bounds.size.z ; j++)
    //        {
    //            GameObject tile = Instantiate(floorTile, transform);
    //            tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(i, 0.5f, j)+Vector3.up*(myRoom.bounds.size.y-1);
    //            tile.transform.rotation = Quaternion.LookRotation(Vector3.up);
    //        }
    //    }
    //}

}
