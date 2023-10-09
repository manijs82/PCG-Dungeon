using UnityEngine;
using UnityEngine.Tilemaps;

public class RiverTileObject : TileGridObject
{
    public RiverTileObject(int x, int y, CellType type) : base(x, y, type)
    {
    }

    public override TileChangeData GetTileVisual()
    {
        TileChangeData data = new TileChangeData
        {
            position = new Vector3Int(x, y),
            tile = ServiceLocator.TileSetProvider.riverSet.GetTile(Type),
            color = Color.white,
            transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)
        };

        return data;
    }
}