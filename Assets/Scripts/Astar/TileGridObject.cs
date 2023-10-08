using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridObject : GridObject
{
    public CellType Type;

    public TileGridObject(int x, int y, CellType type) : base(x, y)
    {
        Type = type;
    }

    public override bool IsBlocked => Type is CellType.Wall or CellType.Ground;

    public virtual TileChangeData GetTileVisual()
    {
        TileChangeData data = new TileChangeData
        {
            position = new Vector3Int(x, y),
            tile = ServiceLocator.TileSetProvider.hallwaySet.GetTile(Type),
            color = ServiceLocator.TileSetProvider.GetGrassColorAt(x, y),
            transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)
        };

        return data;
    }
}