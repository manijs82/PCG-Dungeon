using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapVisual : MonoBehaviour
{
    [SerializeField] private TileBase outLine;
    [SerializeField] private TileBase ground;
    [SerializeField] private TileBase filler;
    [SerializeField] private TileBase door;
    
    private Tilemap tilemap;
    
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        Generator.OnDungeonGenerated += SetTiles;
    }

    private void SetTiles(Dungeon d)
    {
        //FillBlank();
        
        for (int y = 0; y < d.grid.Height; y++)
        {
            for (int x = 0; x < d.grid.Width; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y) + new Vector3Int(d.grid.Width + 5, 0)
                    , GetTile(((TileGridObject)d.grid.GetValue(x, y)).Type));
            }
        }
    }

    private void FillBlank()
    {
        tilemap.BoxFill(new Vector3Int(105, 50), filler, 55, 0, 105, 50);
    }

    private TileBase GetTile(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Empty:
                return filler;
            case CellType.Wall:
                return outLine;
            case CellType.Ground:
                return ground;
            case CellType.Door:
                return door;
        }

        return filler;
    }
}
