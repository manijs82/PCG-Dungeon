using System;
using System.Collections.Generic;
using Mani;
using Mani.Graph;
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
        FindBestDungeon();
        ConstructDungeonGraph();
        
        candidateDungeon.OnMakeGrids += () => OnDungeonGenerated?.Invoke(candidateDungeon);
        candidateDungeon.RemoveUnusedRooms();
        candidateDungeon.MakeGridOutOfRooms();
        
        print(candidateDungeon.fitnessValue);

        //print(GetAverageFitnessValueOfGenerator());
    }

    private void FindBestDungeon()
    {
        Evolution<Dungeon> e = new Evolution<Dungeon>(dungeonParameters);
        var d = (Dungeon)e.samples[0];
        candidateDungeon = d;
    }

    private void ConstructDungeonGraph()
    {
        candidateDungeon.roomGraph = new Graph<Room>();
        foreach (var room in candidateDungeon.rooms) candidateDungeon.roomGraph.AddNode(new Node<Room>(room));
        candidateDungeon.roomGraph.TriangulateDelaunay(node => node.Value.Center.ToPoint());
        candidateDungeon.roomGraph = MST.GetMST(candidateDungeon.roomGraph);
    }

    private float GetAverageFitnessValueOfGenerator()
    {
        float sum = 0f;
        int iterationCount = 1000;
        for (int i = 0; i < iterationCount; i++)
        {
            Evolution<Dungeon> e = new Evolution<Dungeon>(dungeonParameters);
            sum += ((Dungeon)e.samples[0]).fitnessValue / ((Dungeon)e.samples[0]).optimalFitnessValue;
        }

        return sum / iterationCount;
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