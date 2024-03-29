﻿using System;
using System.Diagnostics;
using LSystem;
using Mani;
using Mani.Graph;
using UnityEngine;
using Random = System.Random;

public class Generator : MonoBehaviour
{
    public static Random tileRnd;
    public static Random dungeonRnd;
    public static Random terrainRnd;
    public static Random meshRnd;
    
    public static event Action<Dungeon> OnDungeonGenerated;

    [SerializeField] private bool randomSeed;
    [Range(100000, 1000000000)]
    [SerializeField] private int seed = 123454321;
    [SerializeField] private DungeonParameters dungeonParameters;

    private Dungeon candidateDungeon;

    private void Awake()
    {
        FractalTree fractalTree = new FractalTree();
        
        if (randomSeed)
            seed = UnityEngine.Random.Range(100000, 1000000000);
        
        dungeonRnd = new Random(seed - 50);
        tileRnd = new Random(seed + 50);
        terrainRnd = new Random(seed + 100);
        meshRnd = new Random(seed);
        UnityEngine.Random.InitState(seed);
    }

    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        var watch = new Stopwatch();
        watch.Start();
        
        FindBestDungeon();
        ConstructDungeonGraph();
        RoomDecorator.GenerateRoomTypes(candidateDungeon);
        
        candidateDungeon.MakeGrid();
        
        watch.Stop();
        print($"Generation time: '{watch.ElapsedMilliseconds}'ms");
        
        OnDungeonGenerated?.Invoke(candidateDungeon);
        
        candidateDungeon.fitnessValue = candidateDungeon.Evaluate();
        print($"Dungeon Fitness: '{candidateDungeon.fitnessValue}'");
        //print(GetAverageFitnessValueOfGenerator());
    }

    public void SetDungeon(Dungeon dungeon)
    {
        dungeonRnd = new Random(seed - 50);
        tileRnd = new Random(seed + 50);
        candidateDungeon = dungeon;
        candidateDungeon.MakeGrid();
        OnDungeonGenerated?.Invoke(candidateDungeon);
    }

    private void FindBestDungeon()
    {
        /* Evolution<Dungeon> e = new Evolution<Dungeon>(dungeonParameters);
        var d = (Dungeon)e.samples[0]; */
        
        candidateDungeon = new Dungeon(dungeonParameters);
        for (int i = 0; i < dungeonParameters.width / 2; i++)
        {
            candidateDungeon.Mutate();
        }
    }

    private void ConstructDungeonGraph()
    {
        candidateDungeon.roomGraph = new Graph<Room>();
        foreach (var room in candidateDungeon.rooms) candidateDungeon.roomGraph.AddNode(new Node<Room>(room));
        candidateDungeon.roomGraph.TriangulateDelaunay(node => node.Value.Center.ToPoint());
        candidateDungeon.roomGraph = candidateDungeon.roomGraph.GetPrimsMinimumSpanningTree(true, 0.2f, dungeonParameters.width / 6, dungeonRnd);
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