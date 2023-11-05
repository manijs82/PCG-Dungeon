using System;
using UnityEngine;

public abstract class Mask
{
    protected Dungeon dungeon;
    
    public Mask(Dungeon dungeon)
    {
        this.dungeon = dungeon;
    }

    public bool[,] GetMask()
    {
        bool[,] mask = new bool[dungeon.dungeonParameters.width, dungeon.dungeonParameters.height];
        
        Iterator((x, y) => mask[x, y] = GetMaskValueAt(x, y));

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
}