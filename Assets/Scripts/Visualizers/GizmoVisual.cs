using System.Collections.Generic;
using Freya;
using UnityEditor;
using UnityEngine;

public class GizmoVisual : DungeonVisualizer
{
    [SerializeField] private bool showRoomGraphPath;
    [SerializeField] private Color graphColor = Color.black;
    [SerializeField] private bool showRoomIndex;
    [SerializeField] private Color textColor = Color.red;
    [SerializeField] private bool showRoomArrayPath;
    [SerializeField] private Color arrayColor = Color.green;
    [SerializeField] private bool showDoors;
    [SerializeField] private Color doorColor = Color.red;
    [SerializeField] private bool showDungeonOutline;
    [SerializeField] private bool showRoomOutline;
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private bool showPDV;
    
    private Dungeon dungeon;
    private List<DecorationVolume> volumes = new();
    
    protected override void Visualize(Dungeon dungeon)
    {
        volumes = new List<DecorationVolume>();
        foreach (var room in dungeon.rooms) 
            volumes.Add(RoomDecorator.DecorateRoom(room, EnvironmentType.Forest));
        
        this.dungeon = dungeon;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;
        if (dungeon == null) return;

        var matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Handles.matrix = matrix;
        
        DrawDungeon();

        Handles.matrix = Matrix4x4.identity;
    }


    private void DrawDungeon()
    {
        DrawPDV();
        DrawRoomsGraph();
        
        if(showDungeonOutline)
            DrawOutline(dungeon.grid.GetBound());

        int count = 1;
        foreach (var room in dungeon.rooms)
        {
            DrawRoomIndexText(room, count);
            
            if(showRoomOutline)
                DrawOutline(room.bound);
            
            DrawRoomDoors(room);

            count++;
        }

        DrawRoomArray();
    }

    private void DrawPDV()
    {
        if (showPDV)
            foreach (var volume in volumes)
            {
                volume.DrawGizmos();
            }
    }

    private void DrawRoomsGraph()
    {
        if (!showRoomGraphPath) return;
        
        Handles.color = graphColor;
        foreach (var connection in dungeon.roomGraph.connections)
        {
            Handles.DrawAAPolyLine(connection.start.value.Center, connection.end.value.Center);
            Handles.DrawSolidDisc(connection.start.value.Center, Vector3.back, 2f);
            Handles.DrawSolidDisc(connection.end.value.Center, Vector3.back, 2f);
        }
    }

    private void DrawRoomArray()
    {
        if (!showRoomArrayPath) return;
        
        Handles.color = arrayColor;
        for (int i = 1; i < dungeon.rooms.Count; i++)
        {
            Handles.DrawAAPolyLine(dungeon.rooms[i - 1].Center, dungeon.rooms[i].Center);
            Handles.DrawSolidDisc(dungeon.rooms[i - 1].Center, Vector3.back, .7f);
            Handles.DrawSolidDisc(dungeon.rooms[i].Center, Vector3.back, .7f);
        }
    }

    private void DrawRoomIndexText(Room room, int count)
    {
        if (!showRoomIndex) return;
        
        var labelSkin = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20, 
            fontStyle = FontStyle.Bold,
            richText = true
        };
        Handles.Label(room.Center, $"<color=#{textColor.ToHexString()}> {count} </color>", labelSkin);
    }

    private void DrawRoomDoors(Room room)
    {
        if (!showDoors) return;
        
        Handles.color = doorColor;
        foreach (var door in room.doors)
            Handles.DrawSolidDisc(room.startPoint + door, Vector3.back, .2f);
    }

    private void DrawOutline(Bound bound)
    {
        Handles.color = outlineColor;
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.y), new Vector3(bound.XPW, bound.y));
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.y), new Vector3(bound.x, bound.YPH));
        Handles.DrawAAPolyLine(new Vector3(bound.XPW, bound.y), new Vector3(bound.XPW, bound.YPH));
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.YPH), new Vector3(bound.XPW, bound.YPH));
        Handles.DrawSolidDisc(bound.Center, Vector3.back, .5f);
    }
#endif
}