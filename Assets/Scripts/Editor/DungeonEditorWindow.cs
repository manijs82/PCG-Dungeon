using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DungeonEditorWindow : EditorWindow
    {
        private Dungeon dungeon;
        private bool hasDungeon;
        private bool isEditingRooms;
        private int selectedRoomIndex;
        private int roomIndex;
        private Vector3 currentRoomPos;
        
        [MenuItem("PCG_Dungeon/Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DungeonEditorWindow>();
            window.titleContent = new GUIContent("DungeonEditor");
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneView;
            Generator.OnDungeonGenerated += d =>
            {
                dungeon = d;
                hasDungeon = true;
            };
            selectedRoomIndex = -1;
            currentRoomPos = Vector3.zero;
        }

        private void OnGUI()
        {
            if (!Application.isPlaying || dungeon == null)
            {
                hasDungeon = false;
                isEditingRooms = false;
                return;
            }
            if(!hasDungeon) return;
            if (GUILayout.Button("ReloadVisuals"))
            {
                dungeon.MakeGridOutOfRooms();
            }
            if (GUILayout.Button("Edit Rooms")) 
                isEditingRooms = !isEditingRooms;

            if (isEditingRooms)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    roomIndex = EditorGUILayout.IntSlider(roomIndex, 0, dungeon.rooms.Count - 1);
                    currentRoomPos = dungeon.rooms[roomIndex].startPoint;
                    if (GUILayout.Button("Select Room"))
                    {
                        selectedRoomIndex = roomIndex;
                    }
                }
            }
        }

        private void OnSceneView(SceneView scene)
        {
            if (isEditingRooms && selectedRoomIndex == roomIndex)
            {
                currentRoomPos = Handles.PositionHandle(currentRoomPos, Quaternion.identity);
                dungeon.rooms[selectedRoomIndex].ChangePosition(currentRoomPos);
            }
            else
            {
                Handles.PositionHandle(currentRoomPos, Quaternion.identity);
            }
        }
    }
}