using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapVisual : MonoBehaviour
{
    [SerializeField] private TileBase outLine;
    [SerializeField] private TileBase ground;
    [SerializeField] private TileBase filler;

    private Tilemap tilemap;
    
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        Generator.OnDungeonGenerated += SetTiles;
    }

    private void SetTiles(Dungeon d)
    {
        FillBlank();

        foreach (var room in d.rooms)
        {
            room.SetCells();
            for (int x = 0; x < room.cells.GetLength(0); x++)
            {
                for (int y = 0; y < room.cells.GetLength(1); y++)
                {
                    tilemap.SetTile(new Vector3Int((int) room.startPoint.x + x, (int) room.startPoint.y + y) + new Vector3Int(55, 0)
                        , GetTile(room.cells[x, y]));
                }
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
        }

        return filler;
    }
}
