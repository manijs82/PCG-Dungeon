using UnityEngine;

public class PerlinMask : Mask
{
    private float scale;
    private float threshold;
    private Vector2 randomOffset;
    
    public PerlinMask(Dungeon dungeon, float threshold, float scale = 1, bool inverted = false) : base(dungeon, inverted)
    {
        this.threshold = threshold;
        this.scale = scale;
        randomOffset = Vector2.one * Generator.dungeonRnd.Next(-dungeon.dungeonParameters.width, dungeon.dungeonParameters.width);
    }

    public override bool GetMaskValueAt(int x, int y)
    {
        return Mathf.PerlinNoise((randomOffset.x + x) / scale, (randomOffset.y + y) / scale) > threshold;
    }

    private float GetNoiseValueAt(float x, float y)
    {
        return Mathf.PerlinNoise(x + Mathf.PerlinNoise(x, y), y + Mathf.PerlinNoise(x, y));
    }

    public override Texture2D GetMaskTexture(float resolutionScaling)
    {
        return null;
    }
}