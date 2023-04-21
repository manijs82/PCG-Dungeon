using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapVisual : DungeonVisualizer
{
    [SerializeField] private TileBase outLine;
    [SerializeField] private TileBase ground;
    [SerializeField] private TileBase filler;
    [SerializeField] private TileBase door;
    
    private Tilemap tilemap;
    
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.orientationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
    }

    protected override void Visualize(Dungeon dungeon)
    {
        for (int y = 0; y < dungeon.grid.Height; y++)
        {
            for (int x = 0; x < dungeon.grid.Width; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y)
                    , GetTile(((TileGridObject)dungeon.grid.GetValue(x, y)).Type));
            }
        }
    }

    private TileBase GetTile(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Empty:
                return filler;
            case CellType.Wall:
            case CellType.HallwayWall:
                return outLine;
            case CellType.Ground:
            case CellType.HallwayGround:    
                return ground;
            case CellType.Door:
                return door;
        }

        return filler;
    }
}
