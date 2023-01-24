using System.Collections.Generic;
using Graph;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dungeon : Sample
{
    public Vector2Int RoomCountRange = new(8, 13);
    public Vector2Int RoomWidthRange = new(6, 9);

    public List<Room> rooms;
    public Graph<Room> roomGraph;
    public Grid<GridObject> grid;

    private int width = 50;

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

    public void SetGrid()
    {
        grid = new Grid<GridObject>(50, 50, 1, (_, x, y) => new TileGridObject(x, y, CellType.Empty));

        foreach (var room in rooms)
        {
            for (int x = 0; x < room.Bound.w; x++)
            {
                for (int y = 0; y < room.Bound.h; y++)
                {
                    if (x == 0 || y == 0 || x == room.Bound.w - 1 || y == room.Bound.h - 1)
                        grid.SetValue(x + room.Bound.x, y + room.Bound.y, new TileGridObject(x, y, CellType.Wall));
                    else
                        grid.SetValue(x + room.Bound.x, y + room.Bound.y, new TileGridObject(x, y, CellType.Ground));
                }
            }
        }

        SetPaths();
    }

    private void SetPaths()
    {
        foreach (var connection in roomGraph.connections)
        {
            var centerStart = connection.start.value.Center;
            var centerEnd = connection.end.value.Center;

            var gridStart = grid.GetValue((int)centerStart.x, (int)centerStart.y);
            var gridEnd = grid.GetValue((int)centerEnd.x, (int)centerEnd.y);
            
            Debug.Log(centerStart);
            Debug.Log(centerEnd);
            Debug.Log(gridStart.x + " " + gridStart.y);
            Debug.Log(gridEnd.x + " " + gridEnd.y);
            
            BreadthFirstSearch astar = new BreadthFirstSearch(grid, gridStart, gridEnd);
            astar.PathFindingSearch();
            foreach (var gridObject in astar.path)
            {
                TileGridObject tile = (TileGridObject)gridObject;
                tile.Type = CellType.Ground;
            }
        }
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
}