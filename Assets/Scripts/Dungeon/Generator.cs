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
        candidateDungeon.roomGraph = candidateDungeon.roomGraph.GetPrimsMinimumSpanningTree(true, 0.2f, dungeonParameters.width / 6);

        var node1 = candidateDungeon.GetClosestRoomToPos(Vector2.zero);
        var node2 = candidateDungeon.GetClosestRoomToPos(new Vector2(dungeonParameters.width, dungeonParameters.height));
        var path =  candidateDungeon.roomGraph.DijkstraShortestPath(node1, node2);
        
        Graph<Room> copy = new Graph<Room>(candidateDungeon.roomGraph);
        foreach (var node in path)
        {
            node.Value.environmentType = EnvironmentType.Room;
            candidateDungeon.roomGraph.RemoveNode(node);
        }

        foreach (var island in candidateDungeon.roomGraph.GetIslands(false))
        {
            EnvironmentType islandType = dungeonRnd.Next(0, 10) > 5 ? EnvironmentType.Set : EnvironmentType.SetTwo;
            foreach (var node in island.Nodes)
            {
                node.Value.environmentType = islandType;
            }
        }

        candidateDungeon.startRoom = node1.Value;
        candidateDungeon.roomGraph = copy;
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
}