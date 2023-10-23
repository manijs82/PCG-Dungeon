using System.Collections.Generic;
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
        
        public static T GetRandomElement<T>(this List<T> list)
        {
            int index = Generator.dungeonRnd.Next(0, list.Count);

            return list[index];
        }
        
        public static T GetRandomElement<T>(this T[,] array)
        {
            int indexX = Generator.dungeonRnd.Next(0, array.GetLength(0));
            int indexY = Generator.dungeonRnd.Next(0, array.GetLength(1));

            return array[indexX, indexY];
        }

        public static Vector2 GetPositionTowards(this GridObject from, GridObject to, float t)
        {
            return Vector2.Lerp(from.Point, to.Point, t);
        }
    }
}