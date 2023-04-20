using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static event Action<Dungeon> OnDungeonGenerated; 

    [SerializeField] private GameObject block;
    [SerializeField] private DungeonParameters dungeonParameters;
    [SerializeField] private int index = 1;
    [SerializeField] private bool stepDebug;

    private Dungeon candidateDungeon;
    private List<Dungeon> dungeonSteps;

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        dungeonSteps = new List<Dungeon>();
        Evolution<Dungeon> e = new Evolution<Dungeon>(dungeonParameters);

        Dungeon d = (Dungeon)e.samples[0];
        dungeonSteps.Add(new Dungeon(d));
        
        d.roomGraph = Triangulator.Triangulate(d.rooms);
        dungeonSteps.Add(new Dungeon(d));
        
        d.roomGraph = MST.GetMST(d.roomGraph);
        d.RemoveUnusedRooms();
        d.MakeGridOutOfRooms();
        candidateDungeon = d;
        dungeonSteps.Add(new Dungeon(d));

        if (!stepDebug)
        {
            index = dungeonSteps.Count - 1;
            VisualizeDebugDungeon();
        }

        print(candidateDungeon.fitnessValue);
        OnDungeonGenerated?.Invoke(candidateDungeon);
    }

    [ContextMenu("VDD")]
    private void VisualizeDebugDungeon()
    {
        if(index == dungeonSteps.Count) return;
        if(transform.childCount == 1)
        {
            index++;
            Destroy(transform.GetChild(0).gameObject);
        }
        var parent = new GameObject("Dun").transform;
        parent.SetParent(transform);
        
        var dungeon = dungeonSteps[index];
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;
        if(dungeonSteps == null) return;
        var dungeon = dungeonSteps[index];
        if (dungeon == null) return;
        
        Gizmos.DrawLine(Vector3.zero, new Vector3(dungeon.dungeonParameters.width, 0));
        Gizmos.DrawLine(Vector3.zero, new Vector3(0, dungeon.dungeonParameters.height));
        Gizmos.DrawLine(new Vector3(dungeon.dungeonParameters.width, 0), new Vector3(dungeon.dungeonParameters.width, dungeon.dungeonParameters.height));
        Gizmos.DrawLine(new Vector3(0, dungeon.dungeonParameters.height), new Vector3(dungeon.dungeonParameters.width, dungeon.dungeonParameters.height));
        Gizmos.DrawSphere(new Vector3(dungeon.dungeonParameters.height/2f, dungeon.dungeonParameters.height/2f), .5f);
        
        DrawDungeon(dungeon);
    }


    private void DrawDungeon(Dungeon dungeon)
    {
        Handles.color = Color.black;
        if(dungeon.roomGraph == null) return;
        foreach (var connection in dungeon.roomGraph.connections)
            Handles.DrawAAPolyLine(connection.start.value.Center, connection.end.value.Center);

        Handles.color = Color.red;

        int count = 1;
        foreach (var room in dungeon.rooms)
        {
            var labelSkin = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20, 
                fontStyle = FontStyle.Bold,
                richText = true
            };
            Handles.Label(room.Center, $"<color=red> {count} </color>", labelSkin);
            foreach (var door in room.doors)
                Handles.DrawSolidDisc(room.startPoint + door, Vector3.back, .2f);
            count++;
        }

        Handles.color = Color.green;
        for (int i = 1; i < dungeon.rooms.Count; i++)
            Handles.DrawAAPolyLine(dungeon.rooms[i-1].Center, dungeon.rooms[i].Center);
    }
#endif
}