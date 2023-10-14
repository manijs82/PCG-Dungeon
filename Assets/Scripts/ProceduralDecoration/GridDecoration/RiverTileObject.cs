using UnityEngine;
using UnityEngine.Tilemaps;

public class RiverTileObject : TileGridObject
{
    public override int AnimateSpeed => 8;

    public RiverTileObject(int x, int y, Grid<GridObject> grid, CellType type) : base(x, y, grid, type)
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