using UnityEditor;
using UnityEngine;

public class GizmoVisual : DungeonVisualizer
{
    private Dungeon dungeon;
    
    protected override void Visualize(Dungeon dungeon)
    {
        this.dungeon = dungeon;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;
        if (dungeon == null) return;
        
        DrawOutline();
        DrawDungeon();
    }

    private void DrawOutline()
    {
        Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawLine(Vector3.zero, new Vector3(dungeon.dungeonParameters.width, 0));
        Gizmos.DrawLine(Vector3.zero, new Vector3(0, dungeon.dungeonParameters.height));
        Gizmos.DrawLine(new Vector3(dungeon.dungeonParameters.width, 0),
            new Vector3(dungeon.dungeonParameters.width, dungeon.dungeonParameters.height));
        Gizmos.DrawLine(new Vector3(0, dungeon.dungeonParameters.height),
            new Vector3(dungeon.dungeonParameters.width, dungeon.dungeonParameters.height));
        Gizmos.DrawSphere(new Vector3(dungeon.dungeonParameters.height / 2f, dungeon.dungeonParameters.height / 2f), .5f);
    }


    private void DrawDungeon()
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
        
        Handles.matrix = Matrix4x4.identity;
    }
#endif
}