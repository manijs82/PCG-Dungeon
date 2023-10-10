using System;
using System.Collections.Generic;
using System.Linq;
using Mani;
using Mani.Graph;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Dungeon : Sample
{
    public event Action OnMakeGrids;
    
    public List<Room> rooms;
    public Room startRoom;
    public Room endRoom;
    public Graph<Room> roomGraph;
    public DungeonGrid<GridObject> grid;
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
        dungeonParameters = parms;
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
            Room room = new Room(Vector2.zero, Generator.dungeonRnd.Next(dungeonParameters.roomWidthRange.x, dungeonParameters.roomWidthRange.y),
                Generator.dungeonRnd.Next(dungeonParameters.roomWidthRange.x, dungeonParameters.roomWidthRange.y));
            Vector2 point = new Vector2Int(Generator.dungeonRnd.Next(0, dungeonParameters.width - room.bound.w),
                Generator.dungeonRnd.Next(0, dungeonParameters.height - room.bound.h));;
            room.ChangePosition(point);

            rooms.Add(room);
        }
    }

    public void MakeGrid()
    {
        grid = new DungeonGrid<GridObject>(dungeonParameters.width, dungeonParameters.height, 1,
            (_, x, y) => new TileGridObject(x, y, CellType.Empty));
        
        new River(this).Decorate(grid);

        new RoomGrid(roomGraph).Decorate(grid);

        new Hallway(roomGraph).Decorate(grid);
        
        foreach (var gridObject in grid.GridObjects)
        {
            if (gridObject is not RiverTileObject && gridObject is not RoomTileObject &&
                gridObject is not HallwayTileObject)
            {
                grid.backgroundTiles.Add((TileGridObject) gridObject);
            }
        }
        
        OnMakeGrids?.Invoke();
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

    public Node<Room> GetRandomCornerRoom()
    {
        int cornerIndex = Generator.dungeonRnd.Next(0, 3);
        switch (cornerIndex)
        {
            case 0:
                return GetClosestRoomToPos(bound.BottomLeft);
            case 1:
                return GetClosestRoomToPos(bound.BottomRight);
            case 2:
                return GetClosestRoomToPos(bound.TopLeft);
            case 3:
                return GetClosestRoomToPos(bound.TopRight);
        }
        return GetClosestRoomToPos(bound.BottomLeft);
    }
    
    public Node<Room> GetRoomContainingPoint(Vector2 point)
    {
        return roomGraph.Nodes.FirstOrDefault(node => Bound.Inside(point, node.Value.bound));
    }

    public void RemoveUnusedRooms()
    {
        var unusedRooms = roomGraph.RemoveNodesWithoutNeighbors();
        foreach (var room in unusedRooms) 
            rooms.Remove(room.Value);
    }

    public void RemoveRoomsCollidingWithSpline(Spline spline, int thickness, bool reconnectNeighbors = false)
    {
        var nodesToRemove = new List<Node<Room>>(); 

        int division = (int) spline.GetLength() / 4;
        for (int i = 0; i < division; i++)
        {
            float t = i / (division - 1f);
            spline.Evaluate(t, out float3 position, out float3 tangent, out float3 upVector);

            for (int j = -thickness; j <= thickness; j++)
            {
                float3 pointAlongTangent = position + tangent * j;
                var node = GetRoomContainingPoint(new Vector2(pointAlongTangent.x, pointAlongTangent.y));
                if(node == null) continue;
                if(nodesToRemove.Contains(node)) continue;
            
                nodesToRemove.Add(node);
            }
        }
        
        foreach (var node in nodesToRemove)
        {
            if (node.Neighbors.Count >= 2 && reconnectNeighbors)
            {
                roomGraph.AddEdge(node.Neighbors[0], node.Neighbors[1]);
            }
            roomGraph.RemoveNode(node);
        }
    }

    public override void Mutate()
    {
        for (var i = 0; i < rooms.Count; i++)
        {
            Vector2 newStartPoint;
            if (DoesCollideWithOtherRooms(rooms[i]))
            {
                // if it is colliding with other rooms select random position
                newStartPoint = new Vector2Int(Generator.dungeonRnd.Next(0, dungeonParameters.width - rooms[i].bound.w),
                    Generator.dungeonRnd.Next(0, dungeonParameters.height - rooms[i].bound.h));
            }
            else
            {
                // select a closer position to the center
                newStartPoint = rooms[i].startPoint + (bound.Center - rooms[i].Center).normalized * 2;
            }
            Room newRoom = new Room(rooms[i]);
            newRoom.ChangePosition(newStartPoint);
            if(!DoesCollideWithOtherRooms(newRoom, rooms[i]))
            {
                // move the room if the newly selected position isn't colliding with another room
                rooms[i] = newRoom;
            }
        }
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
    
    private bool IsOutsideDungeon(Room room)
    {
        return !Bound.Inside(room.bound,
            new Bound(0, 0, dungeonParameters.width, dungeonParameters.height));
    }

    public override float Evaluate()
    {
        return Evaluator.EvaluateDungeonBasedOnDistanceFromCenter(this);
    }
}