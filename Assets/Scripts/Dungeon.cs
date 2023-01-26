using System.Collections.Generic;
using Graph;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dungeon : Sample
{
    public Vector2Int roomCountRange = new(13, 16);
    public Vector2Int roomWidthRange = new(10, 20);
    public int width = 100;
    public int height = 100;
    
    public List<Room> rooms;
    public Graph<Room> roomGraph;
    public Grid<GridObject> grid;


    public Dungeon()
    {
        rooms = new List<Room>();
        SetRooms();
    }

    public Dungeon(Vector2Int roomCountRange, Vector2Int roomWidthRange, int width, int height)
    {
        this.roomCountRange = roomCountRange;
        this.roomWidthRange = roomWidthRange;
        this.width = width;
        this.height = height;
        rooms = new List<Room>();
        SetRooms();
    }

    public Dungeon(Dungeon d)
    {
        roomCountRange = d.roomCountRange;
        roomWidthRange = d.roomWidthRange;
        width = d.width;
        height = d.height;
        rooms = new List<Room>(d.rooms);
    }

    private void SetRooms()
    {
        int roomCount = Random.Range(roomCountRange.x, roomCountRange.y);
        for (int i = 0; i < roomCount; i++)
        {
            Vector2 point = new Vector2(Random.Range(0, width), Random.Range(0, height));
            Room room = new Room(point, Random.Range(roomWidthRange.x, roomWidthRange.y),
                Random.Range(roomWidthRange.x, roomWidthRange.y));

            rooms.Add(room);
        }
    }

    public void RemoveUnusedRooms()
    {
        var unusedRooms = roomGraph.RemoveUnconnectedVertices();
        foreach (var room in unusedRooms) 
            rooms.Remove(room.value);
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
        grid = new Grid<GridObject>(width, height, 1, (_, x, y) => new TileGridObject(x, y, CellType.Empty));

        foreach (var room in rooms)
        {
            for (int x = 0; x < room.Bound.w; x++)
            {
                for (int y = 0; y < room.Bound.h; y++)
                {
                    var isWall = x == 0 || y == 0 || x == room.Bound.w - 1 || y == room.Bound.h - 1;
                    grid.SetValue(x + room.Bound.x, y + room.Bound.y,
                        new TileGridObject(x + room.Bound.x, y + room.Bound.y,
                            isWall ? CellType.Wall : CellType.Ground));
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

            var gridStart = grid.GetValue(centerStart);
            var gridEnd = grid.GetValue(centerEnd);

            AStarAlgorithm astar = new AStarAlgorithm(grid, gridStart, gridEnd);
            astar.PathFindingSearch();
            foreach (var gridObject in astar.path)
            {
                var tile = (TileGridObject)gridObject;
                tile.Type = CellType.Ground;
                foreach (var neighbor in grid.Get9Neighbors(tile))
                {
                    var n = (TileGridObject)neighbor;
                    if (n.Type == CellType.Empty) n.Type = CellType.Wall;
                }
            }
        }
    }
}