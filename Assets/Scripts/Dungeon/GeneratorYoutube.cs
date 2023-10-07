using Mani;
using Mani.Graph;
using UnityEditor;
using UnityEngine;

public class GeneratorYoutube : MonoBehaviour
{
    
    [SerializeField] private DungeonParameters dungeonParameters;
    [SerializeField] private bool newDungeon;
    [SerializeField] private bool mutate;
    [SerializeField] private bool triangulate;
    [SerializeField] private bool mst;
    [SerializeField] private bool doAll;
    
    private Dungeon dungeon;
    
    private void Start()
    {
        dungeon = new Dungeon(dungeonParameters);
    }

    #if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        if (dungeon == null)
            return;
        
        var matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Handles.matrix = matrix;
        
        
        foreach (var room in dungeon.rooms)
        {
            DrawOutline(room.bound);
        }

        if (dungeon.roomGraph != null)
        {
            Handles.color = Color.black;
            
            foreach (var connection in dungeon.roomGraph.Edges)
            {
                Handles.DrawAAPolyLine(connection.Start.Value.Center, connection.End.Value.Center);
            }
        }

        if (newDungeon)
        {
            dungeon = new Dungeon(dungeonParameters);
            newDungeon = false;
        }

        if (doAll)
        {
            for (int i = 0; i < 100; i++)
            {
                dungeon.Mutate();
                dungeon.roomGraph = new Graph<Room>();
                foreach (var room in dungeon.rooms) dungeon.roomGraph.AddNode(new Node<Room>(room));
                dungeon.roomGraph.TriangulateDelaunay(node => node.Value.Center.ToPoint());
                dungeon.roomGraph = dungeon.roomGraph.GetPrimsMinimumSpanningTree(true, 0.2f, dungeonParameters.width / 6);
                doAll = false;
            }
        }

        if (mutate)
        {
            dungeon.Mutate();

            mutate = false;
        }

        if (triangulate)
        {
            dungeon.roomGraph = new Graph<Room>();
            foreach (var room in dungeon.rooms) dungeon.roomGraph.AddNode(new Node<Room>(room));
            dungeon.roomGraph.TriangulateDelaunay(node => node.Value.Center.ToPoint());
            triangulate = false;
        }

        if (mst && dungeon.roomGraph != null)
        {
            dungeon.roomGraph = dungeon.roomGraph.GetPrimsMinimumSpanningTree(true, 0.2f, dungeonParameters.width / 6);
            mst = false;
        }
        
        Handles.matrix = Matrix4x4.identity;
    }
    
    private void DrawOutline(Bound bound)
    {
        Handles.color = Color.white;
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.y), new Vector3(bound.XPW, bound.y));
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.y), new Vector3(bound.x, bound.YPH));
        Handles.DrawAAPolyLine(new Vector3(bound.XPW, bound.y), new Vector3(bound.XPW, bound.YPH));
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.YPH), new Vector3(bound.XPW, bound.YPH));
        Handles.DrawSolidDisc(bound.Center, Vector3.back, 1f);
    }
    
    #endif
}