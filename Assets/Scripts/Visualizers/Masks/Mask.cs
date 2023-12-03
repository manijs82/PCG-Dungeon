using System;
using UnityEngine;

public abstract class Mask
{
    protected Dungeon dungeon;
    protected bool inverted;
    
    public Mask(Dungeon dungeon, bool inverted = false)
    {
        this.dungeon = dungeon;
        this.inverted = inverted;
    }

    public bool[,] GetMask()
    {
        bool[,] mask = new bool[dungeon.dungeonParameters.width, dungeon.dungeonParameters.height];
        
        Iterator((x, y) => mask[x, y] = inverted ? !GetMaskValueAt(x, y) : GetMaskValueAt(x, y));

        return mask;
    }

    public void Iterator(Action<int, int> action)
    {
        for (int y = 0; y < dungeon.dungeonParameters.height; y++)
        for (int x = 0; x < dungeon.dungeonParameters.width; x++)
            action(x, y);
    }

    public abstract bool GetMaskValueAt(int x, int y);

    public abstract Texture2D GetMaskTexture(float resolutionScaling);

    public static bool[,] GetCombinedMask(CombineMode combineMode, params Mask[] masks)
    {
        bool[,] combinedMask = new bool[masks[0].dungeon.dungeonParameters.width, masks[0].dungeon.dungeonParameters.height];
        
        masks[0].Iterator((x, y) =>
        {
            bool combinedMaskValue = masks[0].inverted ? !masks[0].GetMaskValueAt(x, y) : masks[0].GetMaskValueAt(x, y);

            for (var i = 1; i < masks.Length; i++)
            {
                var mask = masks[i];
                var maskValueAt = mask.inverted ? !mask.GetMaskValueAt(x, y) : mask.GetMaskValueAt(x, y);
                switch (combineMode)
                {
                    case CombineMode.Union:
                        combinedMaskValue |= maskValueAt;
                        break;
                    case CombineMode.Intersection:
                        combinedMaskValue &= maskValueAt;
                        break;
                }
            }

            combinedMask[x, y] = combinedMaskValue;
        });

        return combinedMask;
    }
}

public enum CombineMode
{
    Union, Intersection
}