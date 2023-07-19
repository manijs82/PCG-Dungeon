using System.Collections.Generic;
using System.IO;
using Mani;
using Mani.Graph;
using UnityEngine;

public class DungeonSaveLoadManager : MonoBehaviour
{
    private Dungeon generatedDungeon;
    
    private void Start()
    {
        Generator.OnDungeonGenerated += dungeon => generatedDungeon = dungeon;
    }

    public void Save()
    {
        DungeonSaveData data = new DungeonSaveData();

        data.dungeonParameters = generatedDungeon.dungeonParameters;

        var nodes = generatedDungeon.roomGraph.Nodes;
        var edges = generatedDungeon.roomGraph.Edges;
        
        data.roomBounds = new Bound[nodes.Count];
        data.roomsEnvironmentTypes = new EnvironmentType[nodes.Count];
        for (var i = 0; i < nodes.Count; i++)
        {
            var room = nodes[i].Value;
            data.roomBounds[i] = room.bound;
            data.roomsEnvironmentTypes[i] = room.environmentType;
        }

        data.connectionStartIndices = new int[edges.Count];
        data.connectionEndIndices = new int[edges.Count];
        for (int i = 0; i < edges.Count; i++)
        {
            data.connectionStartIndices[i] = nodes.IndexOf(edges[i].Start);
            data.connectionEndIndices[i] = nodes.IndexOf(edges[i].End);
        }

        var jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Saves/dungeon.json", jsonData);
    }

    public void Load()
    {
        var jsonData = File.ReadAllText(Application.dataPath + "/Saves/dungeon.json");
        var data = JsonUtility.FromJson<DungeonSaveData>(jsonData);
        
        List<Room> rooms = new List<Room>();
        for (var i = 0; i < data.roomBounds.Length; i++)
        {
            var bound = data.roomBounds[i];
            var environmentType = data.roomsEnvironmentTypes[i];
            
            rooms.Add(new Room(bound, environmentType));
        }

        Graph<Room> graph = new Graph<Room>();
        foreach (var room in rooms) graph.AddNode(new Node<Room>(room));
        for (int i = 0; i < data.connectionStartIndices.Length; i++)
        {
            var startNode = graph.Nodes[data.connectionStartIndices[i]];
            var endNode = graph.Nodes[data.connectionEndIndices[i]];
            graph.AddEdge(startNode, endNode, Vector2.Distance(startNode.Value.Center, endNode.Value.Center));
        }
        
        GetComponent<Generator>().SetDungeon(new Dungeon(data.dungeonParameters, rooms, graph));
    }
}