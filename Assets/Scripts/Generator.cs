using System;
using UnityEditor;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static event Action<Dungeon> OnDungeonGenerated; 

    [SerializeField] private GameObject block;
    [Space] [SerializeField] private Vector2Int roomCountRange = new(13, 16);
    [SerializeField] private Vector2Int roomWidthRange = new(10, 20);
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;

    private Dungeon candidateDungeon;

    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        Evolution<Dungeon> e = new Evolution<Dungeon>(roomCountRange, roomWidthRange, width, height);

        Dungeon d = (Dungeon)e.samples[0];
        d.roomGraph = Triangulator.Triangulate(d.rooms);
        d.roomGraph = MST.GetMST(d.roomGraph);
        d.RemoveUnusedRooms();
        candidateDungeon = d;
        GenerateDungeon(d);

        print(candidateDungeon.fitnessValue);
        OnDungeonGenerated?.Invoke(candidateDungeon);
    }

    private void GenerateDungeon(Dungeon dungeon)
    {
        dungeon.SetGrid();
        for (int y = 0; y < dungeon.grid.Height; y++)
        {
            for (int x = 0; x < dungeon.grid.Width; x++)
            {
                var go = Instantiate(block, new Vector2(x, y),
                    Quaternion.identity, transform);
                SpriteRenderer sprite = go.GetComponentInChildren<SpriteRenderer>();
                sprite.color = GetColor(((TileGridObject)dungeon.grid.GetValue(x, y)).Type);
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

        return Color.gray;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (candidateDungeon == null) return;
        
        Gizmos.DrawLine(Vector3.zero, new Vector3(candidateDungeon.width, 0));
        Gizmos.DrawLine(Vector3.zero, new Vector3(0, candidateDungeon.height));
        Gizmos.DrawLine(new Vector3(candidateDungeon.width, 0), new Vector3(candidateDungeon.width, candidateDungeon.height));
        Gizmos.DrawLine(new Vector3(0, candidateDungeon.height), new Vector3(candidateDungeon.width, candidateDungeon.height));
        Gizmos.DrawSphere(new Vector3(candidateDungeon.height/2f, candidateDungeon.height/2f), .5f);
        
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