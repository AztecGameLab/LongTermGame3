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
        transform.position = myRoom.bounds.center;
        Floor();
        Walls();
        Ceiling();
    }
    //create the floor
    private void Floor()
    {
        for(int i = 0; i < myRoom.bounds.size.x;i++)
        {
            for (int j = 0; j < myRoom.bounds.size.z;j++)
            {
                GameObject tile = Instantiate(floorTile, transform);
                tile.transform.localPosition = -myRoom.bounds.extents+new Vector3(i, -0.5f, j);
                tile.transform.rotation = Quaternion.LookRotation(Vector3.down);
            }
        }
    }
    
    private void Walls()
    {
        Vector2 doorHoleLoc;
        doorHoleLoc.x = myRoom.bounds.center.x;
        doorHoleLoc.y = 0;
        for (int i = 0; i < myRoom.bounds.size.y ; i++)
        {
            for (int j = 0; j < myRoom.bounds.size.z ; j++)
            {
                //left wall
                GameObject tile;
                if (!(i == doorHoleLoc.y && j == doorHoleLoc.x))
                {
                    tile = Instantiate(floorTile, transform);
                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(0.5f, i, j) + Vector3.right * (myRoom.bounds.size.x - 1);
                    tile.transform.rotation = Quaternion.LookRotation(Vector3.right);
                } else
                {
                    tile = Instantiate(doorPrefab, transform);
                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(0.5f, i, j) + Vector3.right * (myRoom.bounds.size.x - 1);
                    tile.transform.rotation = Quaternion.LookRotation(Vector3.right);
                }
                //right wall
                if (!(i == doorHoleLoc.y && j == doorHoleLoc.x))
                {
                    tile = Instantiate(floorTile, transform);
                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(-0.5f, i, j);
                    tile.transform.rotation = Quaternion.LookRotation(Vector3.left);
                } else {
                    tile = Instantiate(doorPrefab, transform);
                    tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(-0.5f, i, j);
                    tile.transform.rotation = Quaternion.LookRotation(Vector3.left);
                }

            }
        }
        for (int i = 0; i < myRoom.bounds.size.y ; i++)
        {
            for (int j = 0; j < myRoom.bounds.size.x ; j++)
            {
                //back wall
                GameObject tile = Instantiate(floorTile, transform);
                tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(j, i, 0.5f) + Vector3.forward * (myRoom.bounds.size.z - 1);
                tile.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                //forward wall
                tile = Instantiate(floorTile, transform);
                tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(j, i, -0.5f);
                tile.transform.rotation = Quaternion.LookRotation(Vector3.back);

            }
        }
    }
    private void Ceiling() {
        for (int i = 0; i < myRoom.bounds.size.x ; i++)
        {
            for (int j = 0; j < myRoom.bounds.size.z ; j++)
            {
                GameObject tile = Instantiate(floorTile, transform);
                tile.transform.localPosition = -myRoom.bounds.extents + new Vector3(i, 0.5f, j)+Vector3.up*(myRoom.bounds.size.y-1);
                tile.transform.rotation = Quaternion.LookRotation(Vector3.up);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
