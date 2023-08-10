using System;
using System.Collections.Generic;
using System.Linq;
using Mani;
using Mani.Graph;
using UnityEngine;

public class Dungeon : Sample
{
    public event Action OnMakeGrids;
    
    public List<Room> rooms;
    public Room startRoom;
    public Room endRoom;
    public Graph<Room> roomGraph;
    public Grid<GridObject> grid;
    public DungeonParameters dungeonParameters;
    public Bound bound;

    public Dungeon(int roomCount = -1)
    {
        rooms = new List<Room>();
        AddRandomRooms();
        optimalFitnessValue = rooms.Count / 2f;
    }

    public Dungeon(DungeonParameters parameters, List<Room> rooms, Graph<Room> graph)
    {
        this.rooms = rooms;
        roomGraph = graph;
        dungeonParameters = parameters;
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
        bound = new Bound(0, 0, dungeonParameters.width, dungeonParameters.height);
        AddRandomRooms();
        optimalFitnessValue = rooms.Count / 2f;
    }

    public Dungeon(Dungeon d)
    {
        dungeonParameters = d.dungeonParameters;
        rooms = new List<Room>(d.rooms);
        roomGraph = d.roomGraph;
        bound = d.bound;
        optimalFitnessValue = d.optimalFitnessValue;
    }

    private void AddRandomRooms(int count = -1)
    {
        rooms = new List<Room>();
        int roomCount = count;
        if(roomCount == -1)
            roomCount = Generator.dungeonRnd.Next(dungeonParameters.roomCountRange.x, dungeonParameters.roomCountRange.y);
        for (int i = 0; i < roomCount; i++)
        {
            Vector2 point = new Vector2(Generator.dungeonRnd.Next(0, dungeonParameters.width), Generator.dungeonRnd.Next(0, dungeonParameters.height));
            Room room = new Room(point, Generator.dungeonRnd.Next(dungeonParameters.roomWidthRange.x, dungeonParameters.roomWidthRange.y),
                Generator.dungeonRnd.Next(dungeonParameters.roomWidthRange.x, dungeonParameters.roomWidthRange.y));

            rooms.Add(room);
        }
    }

    public void RemoveUnusedRooms()
    {
        var unusedRooms = roomGraph.RemoveNodesWithoutNeighbors();
        foreach (var room in unusedRooms) 
            rooms.Remove(room.Value);
    }

    public void MakeGridOutOfRooms()
    {
        grid = new Grid<GridObject>(dungeonParameters.width, dungeonParameters.height, 1,
            (_, x, y) => new TileGridObject(x, y, CellType.Empty));

        foreach (var roomNode in roomGraph.Nodes)
        {
            roomNode.Value.InitializeGrid();
            grid.PlaceGridOnGrid(roomNode.Value.bound.x, roomNode.Value.bound.y, roomNode.Value.grid);
        }

        AddPathsToConnectedRooms();
    }

    private void AddPathsToConnectedRooms()
    {
        if(roomGraph == null) return;
        foreach (var connection in roomGraph.Edges)
        {
            var room1 = connection.Start.Value;
            var room2 = connection.End.Value;
            
            AddDoorsBetweenRooms(room1, room2, out RoomTileObject door1, out RoomTileObject door2);
            
            if (door1 == null || door2 == null) return;
            
            door1.Type = CellType.Door;
            door2.Type = CellType.Door;
            
            AStarAlgorithm astar = new AStarAlgorithm(grid, door1, door2);
            astar.PathFindingSearch();
            foreach (var gridObject in astar.path)
            {
                var tile = (TileGridObject)gridObject;
                tile.Type = CellType.Ground;
                foreach (var neighbor in grid.Get8Neighbors(tile))
                {
                    var n = (TileGridObject)neighbor;
                    if (n.Type == CellType.Empty) n.Type = CellType.Wall;
                }
            }
            
            SetAsDoor(door1);
            SetAsDoor(door2);
        }
        
        OnMakeGrids?.Invoke();
    }

    private void AddDoorsBetweenRooms(Room room1, Room room2, out RoomTileObject door1, out RoomTileObject door2)
    {
        var door1Pos = room1.bound.ClosestPointInside(room2.Center);
        door1Pos += (room1.Center - door1Pos).normalized / 5;
        var door2Pos = room2.bound.ClosestPointInside(room1.Center);
        door2Pos += (room2.Center - door2Pos).normalized / 5;

        door1 = (RoomTileObject) grid.GetValue(door1Pos);
        door2 = (RoomTileObject) grid.GetValue(door2Pos);
    }

    private void SetAsDoor(TileGridObject tile)
    {
        tile.Type = CellType.Door;

        var walls = grid.Get4Neighbors(tile, true).Where(t => ((TileGridObject)t).Type == CellType.Wall && t is RoomTileObject).ToList();
        var secondDoor = (TileGridObject) walls[Generator.dungeonRnd.Next(0, walls.Count - 1)];
        secondDoor.Type = CellType.Door;
    }

    public Node<Room> GetClosestRoomToPos(Vector2 pos)
    {
        float shortestDist = float.PositiveInfinity;
        Node<Room> closestRoom = null;

        foreach (var node in roomGraph.Nodes)
        {
            float dist = Vector2.Distance(pos, node.Value.Center);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                closestRoom = node;
            }
        }

        return closestRoom;
    }

    public override void Mutate()
    {
        for (var i = 0; i < rooms.Count; i++)
        {
            Vector2 newStartPoint;
            if (DoesCollideWithOtherRooms(rooms[i]))
            {
                newStartPoint = new Vector2Int(Generator.dungeonRnd.Next(rooms[i].bound.w / 2, dungeonParameters.width - rooms[i].bound.w / 2),
                    Generator.dungeonRnd.Next(rooms[i].bound.h / 2, dungeonParameters.height - rooms[i].bound.h / 2));
            }
            else
            {
                newStartPoint = rooms[i].startPoint + (bound.Center - rooms[i].Center).normalized * 2;
            }
            Room newRoom = new Room(rooms[i]);
            newRoom.ChangePosition(newStartPoint);
            if(!DoesCollideWithOtherRooms(newRoom, rooms[i]))
                rooms[i] = newRoom;
        }
        /* for (var i = 0; i < rooms.Count; i++)
        {
            Vector2 startPointOffset = new Vector2(Random.Range(-2, 3), Random.Range(-2, 3));
            Room newRoom = new Room(rooms[i]);
            newRoom.ChangePosition(newRoom.startPoint + startPointOffset);
            rooms[i] = newRoom;
        } */
    }

    private bool DoesCollideWithOtherRooms(Room room, Room ignoreRoom = null)
    {
        foreach (var r in rooms)
        {
            if (Bound.Collide(room.bound, r.bound) && r != room && r != ignoreRoom)
                return true;
        }

        return false;
    }

    public override float Evaluate()
    {
        return Evaluator.EvaluateDungeonBasedOnDistanceFromCenter(this);
    }
}