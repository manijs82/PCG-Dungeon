using UnityEngine;

public class PerlinMask : Mask
{
    private float threshold;
    
    public PerlinMask(Dungeon dungeon, float threshold, bool inverted = false) : base(dungeon, inverted)
    {
        this.threshold = threshold;
        ServiceLocator.PerlinNoiseProvider.SetPerlinOffset(Generator.tileRnd.Next(0, 2000));
    }

    public override bool GetMaskValueAt(int x, int y)
    {
        return ServiceLocator.PerlinNoiseProvider.GetNoise(x, y) > threshold;
    }

    public override Texture2D GetMaskTexture(float resolutionScaling)
    {
        return null;
    }
}