using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

[System.Serializable]
public class TileSet
{
    public ProbabilityList<TileBase> emptyTileSet;
    public ProbabilityList<TileBase> wallTileSet;
    public ProbabilityList<TileBase> groundTileSet;
    public ProbabilityList<TileBase> doorTileSet;

    public virtual TileChangeData GetTileData(CellType cellType, Vector3Int position)
    {
        TileChangeData data = new TileChangeData
        {
            position = position,
            tile = GetTile(cellType),
            color = Color.white,
            transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)
        };

        return data;
    }
    
    public virtual TileBase GetTile(CellType cellType)
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

    protected virtual TileBase GetRandomTile(ProbabilityList<TileBase> set)
    {
        return set.GetWeightedRandomElement();
    }
}