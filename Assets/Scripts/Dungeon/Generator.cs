using System;
using Mani;
using Mani.Graph;
using UnityEngine;
using Random = System.Random;

public class Generator : MonoBehaviour
{
    public static Random tileRnd;
    public static Random dungeonRnd;
    
    public static event Action<Dungeon> OnDungeonGenerated;

    [SerializeField] private bool randomSeed;
    [Range(100000, 1000000000)]
    [SerializeField] private int seed = 123454321;
    [SerializeField] private GameObject block;
    [SerializeField] private DungeonParameters dungeonParameters;

    private Dungeon candidateDungeon;

    private void Start()
    {
        if (randomSeed)
            seed = UnityEngine.Random.Range(100000, 1000000000);
        
        dungeonRnd = new Random(seed - 50);
        tileRnd = new Random(seed + 50);
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        FindBestDungeon();
        ConstructDungeonGraph();
        
        candidateDungeon.RemoveUnusedRooms();
        candidateDungeon.MakeGridOutOfRooms();
        OnDungeonGenerated?.Invoke(candidateDungeon);
        
        print(candidateDungeon.fitnessValue);

        //print(GetAverageFitnessValueOfGenerator());
    }

    public void SetDungeon(Dungeon dungeon)
    {
        dungeonRnd = new Random(seed - 50);
        tileRnd = new Random(seed + 50);
        candidateDungeon = dungeon;
        candidateDungeon.MakeGridOutOfRooms();
        OnDungeonGenerated?.Invoke(candidateDungeon);
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
        candidateDungeon.roomGraph = candidateDungeon.roomGraph.GetPrimsMinimumSpanningTree(true, 0.4f, dungeonParameters.width / 6);

        Graph<Room> copy = new Graph<Room>(candidateDungeon.roomGraph);
        
        var node1 = candidateDungeon.roomGraph.Nodes[0];
        var node2 = candidateDungeon.roomGraph.Nodes[30];
        var path =  candidateDungeon.roomGraph.DijkstraShortestPath(node1, node2);
        foreach (var node in path)
        {
            node.Value.environmentType = EnvironmentType.Room;
            copy.RemoveNode(node);
        }

        foreach (var island in copy.GetIslands(false))
        {
            foreach (var node in island.Nodes)
            {
                node.Value.environmentType = dungeonRnd.Next(0, 10) > 5 ? EnvironmentType.Set : EnvironmentType.SetTwo;
            }
        }
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
                return Color.white;
            case CellType.Wall:
                return Color.blue;
            case CellType.Door:
                return Color.black;
        }

        return Color.gray;
    }


}