using System;
using UnityEngine;

namespace MeshGen
{
    public struct Vertex
    {
        public Vector3 position;
        public int triangle;

        public Vertex(Vector3 position)
        {
            this.position = position;
            this.triangle = -1;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Vertex other)
                return false;

            return position == other.position;
        }
        
        public static bool operator ==(Vertex a, Vertex b) => a.Equals(b);
        
        public static bool operator !=(Vertex a, Vertex b) => !a.Equals(b);

        public bool Equals(Vector3 other)
        {
            return position.Equals(other);
        }

        public bool Equals(Vertex other)
        {
            return position.Equals(other.position);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}