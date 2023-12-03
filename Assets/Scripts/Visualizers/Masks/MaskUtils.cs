using System.Collections.Generic;
using UnityEngine;

public static class MaskUtils
{
    public static IEnumerable<Vector3> MaskPositions(this IEnumerable<Vector3> positions, bool[,] mask)
    {
        foreach (var position in positions)
        {
            int x = Mathf.FloorToInt(position.x);
            int y = Mathf.FloorToInt(position.y);

            if (mask[x, y])
                yield return position;
        }
    }

    public static void Invert(this bool[,] mask)
    {
        for (int i = 0; i < mask.GetLength(0); i++)
        for (int j = 0; j < mask.GetLength(1); j++)
            mask[i, j] = !mask[i, j];
    }
}