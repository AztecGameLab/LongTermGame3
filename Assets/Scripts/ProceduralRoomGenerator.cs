using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRoomGenerator : MonoBehaviour
{
    private RoomData myRoom;
    public bool defaultRoomData=false;
    public RoomData testData;
    public GameObject floorTile;
    public GameObject doorPrefab;
    public Vector2 doorDimensions;
    Transform roomLocation;
    // Start is called before the first frame update
    void Start()
    {
        if (defaultRoomData)
        {
            
            CreateRoom(testData);
        }
    }
    public void CreateRoom(RoomData data)
    {
        myRoom = data;
        roomLocation = new GameObject(myRoom.name).transform;
        roomLocation.position = myRoom.bounds.center / 2;
        //six surfaces for a basic room
        CreateSurface(Vector3.up, myRoom.bounds.size.z, myRoom.bounds.size.x, myRoom.bounds.extents.y);
        CreateSurface(Vector3.down, myRoom.bounds.size.z, myRoom.bounds.size.x, myRoom.bounds.extents.y);
        CreateSurface(Vector3.left, myRoom.bounds.size.y, myRoom.bounds.size.z, myRoom.bounds.extents.x);
        CreateSurface(Vector3.right, myRoom.bounds.size.y, myRoom.bounds.size.z, myRoom.bounds.extents.x);
        CreateSurface(Vector3.forward, myRoom.bounds.size.y, myRoom.bounds.size.x, myRoom.bounds.extents.z);
        CreateSurface(Vector3.back, myRoom.bounds.size.y, myRoom.bounds.size.x, myRoom.bounds.extents.z);
        //Floor();
        //Walls();
        //Ceiling();
    }
    //creates a surface facing the <dir> direction sized <width> x <height> and <distance> away from the center
    private void CreateSurface(Vector3 dir, float width, float height, float distance)
    {
        //create an object to hold all the tiles in the wall
        GameObject surface = new GameObject("surface");
        surface.transform.parent = roomLocation;
        //move it <distance> out from center
        surface.transform.localPosition = roomLocation.position + (-dir) * (distance);
        //make it face dir
        surface.transform.rotation = Quaternion.LookRotation(-dir);
        //spawn tiles
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject tile = null;
                //check if a door should be here
                foreach (DoorData d in myRoom.doors)
                {
                    if (d.wall == dir && d.wallPosition == new Vector2Int(i, j))
                    {
                        //if so spawn a door
                        tile = Instantiate(doorPrefab, surface.transform);
                        tile.transform.localPosition = new Vector3(i - (height / 2.0f) + 0.5f, j - width / 2.0f + 0.5f);
                    }
                }
                //if there was no door make a wall
                if (!tile)
                {
                    tile = Instantiate(floorTile, surface.transform);
                    tile.transform.localPosition = new Vector3(i - (height / 2.0f) + 0.5f, j - width / 2.0f + 0.5f);
                }
            }
        }
        
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
