using System.Collections.Generic;
using System.Linq;
using ManisDataStructures;
using ManisDataStructures.Graph;
using UnityEngine;

public static class MST
{
    public static Graph<Room> GetMST(Graph<Room> graph)
    {
        if (graph.Nodes.Count <= 2) return graph;
        var o = new Graph<Room>();
        var nodes = new List<Node<Room>>(graph.Nodes);
        var mst = new List<Node<Room>>();
        
        var startNode = nodes[0];
        mst.Add(startNode);
        nodes.Remove(startNode);

        while (nodes.Count != 0)
        {
            var connections = graph.GetAllConnectedEdges(mst[^1]).ToList();
            connections = connections.OrderBy(c => c.Cost).ToList();
            Edge<Room> nextConnection = null;

            foreach (var connection in connections)
            {
                if (mst.Contains(connection.GetOtherNode(mst[^1]))) continue;
                nextConnection = connection;
                break;
            }
            if(nextConnection == null) break;
            var nextRoom = nextConnection.GetOtherNode(mst[^1]);
            nodes.Remove(nextRoom);
            mst.Add(nextRoom);
            
            o.AddEdge(nextConnection.GetOtherNode(nextRoom), nextRoom);
        }

        foreach (var connection in graph.Edges)
        {
            float r = Random.value;
            if (r < 0.333f) 
                o.AddEdge(connection.Start, connection.End);
        }

        return o;
    }
}