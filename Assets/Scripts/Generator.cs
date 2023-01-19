using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static event Action<Dungeon> OnDungeonGenerated; 

    [SerializeField] private GameObject block;

    private Dungeon candidateDungeon;

    private void Start()
    {
        Evolution<Dungeon> e = new Evolution<Dungeon>();

        Dungeon d = (Dungeon)e.samples[0];
        d.roomGraph = Triangulator.Triangulate(d.rooms);
        d.roomGraph = MST.GetMST(d.roomGraph);
        foreach (var room in d.rooms)
        {
            room.SetCells();
            GenerateRoom(room);
        }

        candidateDungeon = d;
        OnDungeonGenerated?.Invoke(candidateDungeon);
    }

    private void GenerateRoom(Room room)
    {
        for (int y = 0; y < room.cells.GetLength(1); y++)
        {
            for (int x = 0; x < room.cells.GetLength(0); x++)
            {
                var go = Instantiate(block, room.startPoint + new Vector2(x, y),
                    Quaternion.identity, transform);
                SpriteRenderer sprite = go.GetComponentInChildren<SpriteRenderer>();
                sprite.color = GetColor(room.cells[x, y]);
            }
        }
    }

    private Color GetColor(CellType roomCell)
    {
        switch (roomCell)
        {
            case CellType.Ground:
                return Color.white;
            case CellType.Wall:
                return Color.blue;
        }

        return Color.white;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector3.zero, new Vector3(50, 0));
        Gizmos.DrawLine(Vector3.zero, new Vector3(0, 50));
        Gizmos.DrawLine(new Vector3(50, 0), new Vector3(50, 50));
        Gizmos.DrawLine(new Vector3(0, 50), new Vector3(50, 50));
        Gizmos.DrawSphere(new Vector3(25, 25), .5f);

        if (candidateDungeon != null)
            DrawDungeon(candidateDungeon);
    }


    private void DrawDungeon(Dungeon dungeon)
    {
        Handles.color = Color.black;
        foreach (var connection in dungeon.roomGraph.connections)
            Handles.DrawAAPolyLine(connection.start.value.Center, connection.end.value.Center);
    }
#endif
}