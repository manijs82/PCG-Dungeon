using System.Collections.Generic;

namespace Graph
{
    public class Node<T>
    {
        public T value;
        public List<Node<T>> neighbors;

        public Node(T value)
        {
            this.value = value;
            neighbors = new List<Node<T>>();
        }

        public void AddNeighbor(Node<T> node)
        {
            if(!neighbors.Contains(node)) neighbors.Add(node);
        }
    }
}