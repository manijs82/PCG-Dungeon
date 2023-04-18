
using System.Collections.Generic;
using Graph;
using UnityEngine;

[System.Serializable]
public class DungeonParameters : SampleParameters
{
    public Vector2Int roomCountRange;
    public Vector2Int roomWidthRange;
    public int width;
    public int height;
}

public class Dungeon : Sample
{
    public List<Room> rooms;
    public Graph<Room> roomGraph;
    public Grid<GridObject> grid;
    public DungeonParameters dungeonParameters;

    public Dungeon(int roomCount = -1)
    {
        rooms = new List<Room>();
        AddRandomRooms();
        optimalFitnessValue = rooms.Count - 1;
    }

    public Dungeon(SampleParameters parameters)
    {
        var parms = parameters as DungeonParameters;
        dungeonParameters = new DungeonParameters
        {
            roomCountRange = parms.roomCountRange,
            roomWidthRange = parms.roomWidthRange,
            width = parms.width,
            height = parms.height
        };
        AddRandomRooms();
        optimalFitnessValue = rooms.Count - 1;
        Debug.Log(optimalFitnessValue);
    }

    public Dungeon(Dungeon d)
    {
        dungeonParameters = d.dungeonParameters;
        rooms = new List<Room>(d.rooms);
        roomGraph = d.roomGraph;
    }

    private void AddRandomRooms(int count = -1)
    {
        rooms = new List<Room>();
        int roomCount = count;
        if(roomCount == -1)
            roomCount = Random.Range(dungeonParameters.roomCountRange.x, dungeonParameters.roomCountRange.y);
        for (int i = 0; i < roomCount; i++)
        {
            Vector2 point = new Vector2(Random.Range(0, dungeonParameters.width), Random.Range(0, dungeonParameters.height));
            Room room = new Room(point, Random.Range(dungeonParameters.roomWidthRange.x, dungeonParameters.roomWidthRange.y),
                Random.Range(dungeonParameters.roomWidthRange.x, dungeonParameters.roomWidthRange.y));

            rooms.Add(room);
        }
    }

    public void RemoveUnusedRooms()
    {
        var unusedRooms = roomGraph.RemoveUnconnectedVertices();
        foreach (var room in unusedRooms) 
            rooms.Remove(room.value);
    }

    public void MakeGridOutOfRooms()
    {
        grid = new Grid<GridObject>(dungeonParameters.width, dungeonParameters.height, 1,
            (_, x, y) => new TileGridObject(x, y, CellType.Empty));

        foreach (var room in rooms)
        {
            room.InitializeGrid();
            grid.PlaceGridOnGrid(room.bound.x, room.bound.y, room.grid);
        }

        AddPathsToConnectedRooms();
    }

    private void AddPathsToConnectedRooms()
    {
        if(roomGraph == null) return;
        foreach (var connection in roomGraph.connections)
        {
            var room1 = connection.start.value;
            var room2 = connection.end.value;
            
            AddDoorsBetweenRooms(room1, room2, out Vector2 door1, out Vector2 door2);

            var startTile = (TileGridObject) grid.GetValue(door1);
            var endTile = (TileGridObject) grid.GetValue(door2);
            
            startTile.Type = CellType.Door;
            endTile.Type = CellType.Door;

            AStarAlgorithm astar = new AStarAlgorithm(grid, startTile, endTile);
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
            
            startTile.Type = CellType.Door;
            endTile.Type = CellType.Door;
        }
    }

    private void AddDoorsBetweenRooms(Room room1, Room room2, out Vector2 door1, out Vector2 door2)
    {
        door1 = room1.bound.ClosestPointInside(room2.Center);
        door1 += (room1.Center - door1).normalized / 5;
        door2 = room2.bound.ClosestPointInside(room1.Center);
        door2 += (room2.Center - door2).normalized / 5;
        room1.doors.Add(door1 - room1.startPoint);
        room2.doors.Add(door2 - room2.startPoint);
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

    public override float Evaluate()
    {
        return Evaluator.EvaluateDungeonBasedOnLineBetweenRooms(this);
    }
}