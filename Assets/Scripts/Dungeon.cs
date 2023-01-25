using System.Collections.Generic;
using Graph;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dungeon : Sample
{
    public Vector2Int RoomCountRange = new(8, 12);
    public Vector2Int RoomWidthRange = new(7, 15);

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