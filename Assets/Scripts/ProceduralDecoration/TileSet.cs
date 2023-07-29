using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileSet
{
    public TileBase[] emptySet;
    public TileBase[] wallSet;
    public TileBase[] groundSet;
    public TileBase[] doorSet;
    
    public TileBase GetTile(CellType cellType)
    {
        switch (cellType)
        {
            default:
            case CellType.Empty:
                return GetRandomTile(emptySet);
            case CellType.Wall:
                return GetRandomTile(wallSet);
            case CellType.Ground:
                return GetRandomTile(groundSet);
            case CellType.Door:
                return GetRandomTile(doorSet);
        }
    }

    private TileBase GetRandomTile(TileBase[] set)
    {
        return set[Generator.tileRnd.Next(0, set.Length)];
    }
}