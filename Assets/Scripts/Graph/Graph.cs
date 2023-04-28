using System.Collections.Generic;

namespace Graph
{
    public class Graph<T>
    {
        public List<Node<T>> vertices;
        public List<Connection<T>> connections;

        public Graph()
        {
            vertices = new List<Node<T>>();
            connections = new List<Connection<T>>();
        }

        public List<Connection<T>> GetConnectedConnections(Node<T> node)
        {
            var o = new List<Connection<T>>();
            foreach (var connection in connections)
            {
                if(connection.HasNode(node))
                    o.Add(connection);
            }

            return o;
        }

        public void AddVertices(Node<T> node)
        {
            if(!vertices.Contains(node)) vertices.Add(node);
        }

        public T GetVertex(int index)
        {
            return vertices[index].value;
        }

        public Connection<T> AddConnection(Node<T> start, Node<T> end)
        {
            if(ContainConnection(start, end)) return null;
            
            if(!vertices.Contains(start)) vertices.Add(start);
            if(!vertices.Contains(end)) vertices.Add(end);
            var connection = new Connection<T>(start, end);
            connections.Add(connection);

            return connection;
        }

        public List<Node<T>> RemoveUnconnectedVertices()
        {
            var unconnected = new List<Node<T>>();
            foreach (var node in vertices)
            {
                bool isConnected = false;
                foreach (var connection in connections)
                    if(connection.Contains(node))
                    {
                        isConnected = true;
                        break;
                    }

                if (isConnected) continue;
                unconnected.Add(node);
            }

            return unconnected;
        }

        private bool ContainConnection(Node<T> start, Node<T> end)
        {
            foreach (var connection in connections)
            {
                if ((connection.start == start && connection.end == end) ||
                    (connection.end == start && connection.start == end))
                {
                    return true;
                }
            }

            return false;
        }
    }
}