using System;
using Mani;
using Mani.Geometry;
using Mani.Graph;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

public static class RoomDecorator
{
    public static DecorationVolume GenerateRoomObjects(Room room, DecorationVolumeHierarchy volumeHierarchy)
    {
        var clone = Object.Instantiate(volumeHierarchy);
        var rootPdv = clone.volumeHierarchy.Root.Value;
        rootPdv.Init(room);

        return rootPdv;
    }

    public static void GenerateRoomTypes(Dungeon dungeon)
    {
        Node<Room> startNode;
        Node<Room> endNode;
        switch (dungeon.dungeonParameters.roomTypeLayout)
        {
            case RoomTypeLayout.LinePathWithBranches:
                startNode = dungeon.GetClosestRoomToPos(Vector2.zero);
                endNode = dungeon.GetClosestRoomToPos(new Vector2(dungeon.dungeonParameters.width, dungeon.dungeonParameters.height));
                var path =  dungeon.roomGraph.DijkstraShortestPath(startNode, endNode);
        
                Graph<Room> copy = new Graph<Room>(dungeon.roomGraph);
                foreach (var node in path)
                {
                    node.Value.environmentType = EnvironmentType.Room;
                    copy.RemoveNode(node);
                }

                foreach (var island in copy.GetIslands(false, false))
                {
                    EnvironmentType islandType = Generator.dungeonRnd.Next(0, 10) > 5 ? EnvironmentType.Set : EnvironmentType.SetTwo;
                    foreach (var node in island.Nodes)
                    {
                        node.Value.environmentType = islandType;
                    }
                }

                dungeon.startRoom = startNode.Value;
                dungeon.endRoom = endNode.Value;
                //dungeon.roomGraph = copy;
                break;
            case RoomTypeLayout.GrassOnly:
                startNode = dungeon.GetClosestRoomToPos(Vector2.zero);
                endNode = dungeon.GetClosestRoomToPos(new Vector2(dungeon.dungeonParameters.width, dungeon.dungeonParameters.height));
                dungeon.startRoom = startNode.Value;
                dungeon.endRoom = endNode.Value;
                break;
            case RoomTypeLayout.Village:
                float caveRadius = Generator.dungeonRnd.Next(dungeon.dungeonParameters.width / 4,
                    dungeon.dungeonParameters.width / 3);
                Circle caveCircle = new Circle(dungeon.bound.GetRandomPointInside(), caveRadius);
                foreach (var roomNode in dungeon.GetRoomsCollidingWithCircle(caveCircle))
                {
                    roomNode.Value.environmentType = EnvironmentType.Set;
                }
                
                startNode = dungeon.GetClosestRoomToPos(Vector2.zero);
                endNode = dungeon.GetClosestRoomToPos(new Vector2(dungeon.dungeonParameters.width, dungeon.dungeonParameters.height));
                dungeon.startRoom = startNode.Value;
                dungeon.endRoom = endNode.Value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}