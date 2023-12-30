using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static Vector3 OffsetDown(this Vector3 vector, float offset)
        {
            return new Vector3(vector.x, vector.y - offset, vector.z);
        }
    }
}