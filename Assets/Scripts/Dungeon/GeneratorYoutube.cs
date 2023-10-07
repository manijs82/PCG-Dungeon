using UnityEditor;
using UnityEngine;

public class GeneratorYoutube : MonoBehaviour
{
    
    [SerializeField] private DungeonParameters dungeonParameters;
    [SerializeField] private bool mutate;
    
    private Dungeon[] dungeons;
    
    private void Start()
    {
        dungeons = new Dungeon[1];

        for (int i = 0; i < 1; i++)
        {
            dungeons[i] = new Dungeon(dungeonParameters);
        }
    }

    #if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        if (dungeons == null)
            return;
        
        var matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Handles.matrix = matrix;
        
        foreach (var dungeon in dungeons)
        {
            foreach (var room in dungeon.rooms)
            {
                DrawOutline(room.bound);
            }
        }

        if (mutate)
        {
            foreach (var dungeon in dungeons)
            {
                dungeon.Mutate();
            }

            mutate = false;
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
        Handles.DrawSolidDisc(bound.Center, Vector3.back, .5f);
    }
    
    #endif
}