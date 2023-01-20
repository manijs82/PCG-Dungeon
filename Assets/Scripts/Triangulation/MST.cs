using System.Collections.Generic;
using System.Linq;
using Graph;
using UnityEngine;

public static class MST
{
    public static Graph<Room> GetMST(Graph<Room> graph)
    {
        var o = new Graph<Room>();
        var nodes = new List<Node<Room>>(graph.vertices);
        var mst = new List<Node<Room>>();
        
        var startNode = nodes[0];
        mst.Add(startNode);
        nodes.Remove(startNode);

        while (nodes.Count != 0)
        {
            var connections = graph.GetConnectedConnections(mst[^1]);
            connections = connections.OrderBy(c => c.cost).ToList();
            Connection<Room> nextConnection = null;

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
            
            o.AddConnection(nextConnection.GetOtherNode(nextRoom), nextRoom);
        }

        foreach (var connection in graph.connections)
        {
            float r = Random.value;
            if (r < 0.333f) 
                o.AddConnection(connection.start, connection.end);
        }

        return o;
    }
}