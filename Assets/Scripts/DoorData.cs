using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct DoorData
{
    public Vector3 wall;//represents the direction of the wall the door should be on. 
    public Vector2Int wallPosition;//represents the tile of the wall the door is on, this will be the lower left corner of the door
                                   // this position is measured from the lower left corner of the wall
                                   
    /*
    public Vector3 position;
    public RoomData room1;
    public RoomData room2;
    public string name;

    public DoorData( RoomData room1)
    {
        name = "Door";
        this.room1 = room1;
        this.room2 = new RoomData();
        room2.type = "EMPTY";
        position = new Vector3();
        setDoorPos();
    }

    //TODO modular door position, e.g. not in the middle of the room, maybe elevated door for stairs, etc.
    //As of right now, only sets Door positions to the middle of a random wall of a room
    private void setDoorPos()
    {
        int random = Random.Range(1, 101);
        if (random > 50)
            position = new Vector3(getRandomXWall(), room1.position.y - (room1.size.y)/2, room1.position.z);
        else
            position = new Vector3(room1.position.x, room1.position.y - (room1.size.y)/2, getRandomZWall());
    }
    // either gets the the left wall x position of the room or the right wall position of the room
    private float getRandomXWall()
    {
        int random = Random.Range(1, 101);
        if (random > 50)
            return room1.position.x + (room1.size.x)/2;
        return room1.position.x - (room1.size.x)/2;
    }
    //either gets the front wall z position of the room or the back wall position of the room
    private float getRandomZWall()
    {
        int random = Random.Range(1, 101);
        if (random > 50)
            return room1.position.z + (room1.size.z)/2;
        return room1.position.z - (room1.size.z)/2;
    }
    */
}
