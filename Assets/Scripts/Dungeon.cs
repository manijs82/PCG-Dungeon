using System.Collections.Generic;
using Graph;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dungeon : Sample
{
    public Vector2Int RoomCountRange = new(8, 13);
    public Vector2Int RoomWidthRange = new(6, 9);

    private CellType[,] cells;
    private int width = 50;
    public List<Room> rooms;
    public Graph<Room> roomGraph;

    public Dungeon()
    {
        rooms = new List<Room>();
        SetRooms();
    }

    public Dungeon(Dungeon d)
    {
        width = d.width;
        rooms = new List<Room>(d.rooms);
    }

    private void SetRooms()
    {
        int roomCount = Random.Range(RoomCountRange.x, RoomCountRange.y);
        for (int i = 0; i < roomCount; i++)
        {
            Vector2 point = new Vector2(Random.Range(0, width), Random.Range(0, width));
            Room room = new Room(point, Random.Range(RoomWidthRange.x, RoomWidthRange.y),
                Random.Range(RoomWidthRange.x, RoomWidthRange.y));

            rooms.Add(room);
        }
    }

    public override void Mutate()
    {
        for (var i = 0; i < rooms.Count; i++)
        {
            Vector2 startPointOffset = new Vector2(Random.Range(-2, 3), Random.Range(-2, 3));
            Room newRoom = new Room(rooms[i]);
            newRoom.startPoint += startPointOffset;
            rooms[i] = newRoom;
        }
    }
}