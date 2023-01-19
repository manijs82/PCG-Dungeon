namespace Graph
{
    public class Connection<T>
    {
        public Node<T> start;
        public Node<T> end;
        public float cost;

        public Connection(Node<T> start, Node<T> end)
        {
            this.start = start;
            this.end = end;
        }

        public bool HasNode(Node<T> node)
        {
            return start == node || end == node;
        }

        public Node<T> GetOtherNode(Node<T> node)
        {
            if (start == node) return end;
            if (end == node) return start;
            return null;
        }

        public void SetCost(float cost)
        {
            this.cost = cost;
        }
    }
}