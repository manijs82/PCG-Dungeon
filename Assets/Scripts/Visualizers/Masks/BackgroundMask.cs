using System;
using UnityEngine;

public class BackgroundMask : Mask
{
    public BackgroundMask(Dungeon dungeon, bool inverted = false) : base(dungeon, inverted)
    {
    }

    public override Texture2D GetMaskTexture(float resolutionScaling)
    {
        var texture = new Texture2D((int)(dungeon.grid.Width * resolutionScaling), (int)(dungeon.grid.Height * resolutionScaling));
        texture.filterMode = FilterMode.Point;
            
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                var maskValue = GetMaskValueAt(
                    (int) (x * (1f / resolutionScaling)),
                    (int) (y * (1f / resolutionScaling)));
                Color color = maskValue ? Color.white : Color.black;
                texture.SetPixel(x, y, color);
            }
        }
        
        texture.Apply();

        return texture;
    }

    public override bool GetMaskValueAt(int x, int y)
    {
        var tile = (TileGridObject)dungeon.grid.GetValue(x, y);
        
        return tile.Type == CellType.Empty;
    }
}