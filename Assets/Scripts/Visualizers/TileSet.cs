using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Utils;

[System.Serializable]
public class TileSet
{
    public ProbabilityList<TileBase> emptyTileSet;
    public ProbabilityList<TileBase> wallTileSet;
    public ProbabilityList<TileBase> groundTileSet;
    public ProbabilityList<TileBase> doorTileSet;
    
    public TileBase GetTile(CellType cellType)
    {
        switch (cellType)
        {
            default:
            case CellType.Empty:
                return GetRandomTile(emptyTileSet);
            case CellType.Wall:
                return GetRandomTile(wallTileSet);
            case CellType.Ground:
                return GetRandomTile(groundTileSet);
            case CellType.Door:
                return GetRandomTile(doorTileSet);
        }
    }

    private TileBase GetRandomTile(ProbabilityList<TileBase> set)
    {
        return set.GetWeightedRandomElement();
    }
}