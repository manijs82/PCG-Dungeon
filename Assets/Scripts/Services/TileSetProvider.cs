using UnityEngine;

public class TileSetProvider : Service
{
    public Color darkGrassColor;
    public Color brightGrassColor;
    [Space]
    public TileSet dungeonSet;
    public TileSet environmentSet;
    public TileSet sideSet;
    public TileSet sideTwoSet;
    public TileSet hallwaySet;
    public TileSet riverSet;

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.TileSetProvider = this;
    }

    public Color GetGrassColorAt(int x, int y)
    {
        var noise = ServiceLocator.PerlinNoiseProvider.GetNoise(x, y);
        noise = Mathf.Clamp01(noise - .3f);
        return Color.Lerp(darkGrassColor, brightGrassColor, noise);
    }

    protected override void OnDestroy()
    {
        ServiceLocator.TileSetProvider = null;
        base.OnDestroy();
    }
}