using System.Collections.Generic;
using Graph;
using UnityEngine;

public static class MST
{
    public static Graph<Room> GetMST(Graph<Room> graph)
    {
        var o = new Graph<Room>();
        var current = new List<Node<Room>>(graph.vertices);
        var final = new List<Node<Room>>();
        
        var startNode = current[0];
        final.Add(startNode);
        current.Remove(startNode);

        while (current.Count != 0)
        {
            var connections = graph.GetConnectedConnections(final[^1]);
            Connection<Room> nextConnection = null;

            foreach (var connection in connections)
            {
                if (final.Contains(connection.GetOtherNode(final[^1]))) continue;
                nextConnection = connection;
                break;
            }
            if(nextConnection == null) break;
            var nextRoom = nextConnection.GetOtherNode(final[^1]);
            current.Remove(nextRoom);
            final.Add(nextRoom);
            
            o.AddConnection(nextConnection.GetOtherNode(nextRoom), nextRoom);
        }
        
        
        return o;
    }
}