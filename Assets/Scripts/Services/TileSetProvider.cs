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
        return Color.Lerp(darkGrassColor, brightGrassColor, ServiceLocator.PerlinNoiseProvider.GetNoise(x, y));
    }

    protected override void OnDestroy()
    {
        ServiceLocator.TileSetProvider = null;
        base.OnDestroy();
    }
}