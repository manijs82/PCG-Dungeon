using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static event Action<Dungeon> OnDungeonGenerated; 

    [SerializeField] private GameObject block;
    [SerializeField] private DungeonParameters dungeonParameters;

    private Dungeon candidateDungeon;

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        Dungeon d;
        do
        {
            Evolution<Dungeon> e = new Evolution<Dungeon>(dungeonParameters);
            d = (Dungeon)e.samples[0];
        } while (d.fitnessValue < 0);

        d.OnMakeGrids += () => OnDungeonGenerated?.Invoke(d);
        
        d.roomGraph = Triangulator.Triangulate(d.rooms);
        
        d.roomGraph = MST.GetMST(d.roomGraph);
        d.RemoveUnusedRooms();
        d.MakeGridOutOfRooms();
        candidateDungeon = d;

        print(candidateDungeon.fitnessValue);
    }

    [ContextMenu("VDD")]
    private void VisualizeDebugDungeon()
    {
        if(transform.childCount == 1)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        var parent = new GameObject("Dun").transform;
        parent.SetParent(transform);
        
        var dungeon = candidateDungeon;
        dungeon.MakeGridOutOfRooms();
        for (int y = 0; y < dungeon.grid.Height; y++)
        {
            for (int x = 0; x < dungeon.grid.Width; x++)
            {
                var go = Instantiate(block, new Vector2(x, y),
                    Quaternion.identity, parent);
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
            case CellType.HallwayGround:
                return Color.white;
            case CellType.Wall:
            case CellType.HallwayWall:
                return Color.blue;
            case CellType.Door:
                return Color.black;
        }

        return Color.gray;
    }


}