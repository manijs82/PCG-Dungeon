using Mani;
using UnityEngine;

namespace Utils
{
    public static class DungeonUtils
    {
        public static Vector2 GetRandomVectorInside(this Bound bound)
        {
            int x = Generator.dungeonRnd.Next(bound.x, bound.XPW);
            int y = Generator.dungeonRnd.Next(bound.y, bound.YPH);

            return new Vector2(x, y);
        }
        
        public static Vector2Int GetRandomVectorIntInside(this Bound bound)
        {
            int x = Generator.dungeonRnd.Next(bound.x, bound.XPW);
            int y = Generator.dungeonRnd.Next(bound.y, bound.YPH);

            return new Vector2Int(x, y);
        }
        
        public static Point GetRandomPointInside(this Bound bound)
        {
            int x = Generator.dungeonRnd.Next(bound.x, bound.XPW);
            int y = Generator.dungeonRnd.Next(bound.y, bound.YPH);

            return new Point(x, y);
        }
    }
}