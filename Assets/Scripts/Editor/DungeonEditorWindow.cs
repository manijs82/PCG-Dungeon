using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DungeonEditorWindow : EditorWindow
    {
        private Dungeon dungeon;
        private bool hasDungeon;
        private bool isEditingRooms;
        private Vector3[] roomPositions;

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
        }

        private void OnGUI()
        {
            if (!Application.isPlaying || dungeon == null)
            {
                hasDungeon = false;
                isEditingRooms = false;
                return;
            }

            if (!hasDungeon) return;
            if (GUILayout.Button("ReloadVisuals"))
            {
                dungeon.MakeGrid();
            }

            if (GUILayout.Button("Edit Rooms"))
            {
                roomPositions = new Vector3[dungeon.rooms.Count];
                for (int i = 0; i < roomPositions.Length; i++) 
                    roomPositions[i] = dungeon.rooms[i].startPoint;
                isEditingRooms = !isEditingRooms;
            }
        }

        private void OnSceneView(SceneView scene)
        {
            if (isEditingRooms)
            {
                for (int i = 0; i < roomPositions.Length; i++)
                {
                    EditorGUI.BeginChangeCheck();
                    roomPositions[i] = Handles.PositionHandle(roomPositions[i], Quaternion.identity);
                    if(EditorGUI.EndChangeCheck())
                        dungeon.rooms[i].ChangePosition(roomPositions[i]);
                }
            }
        }
    }
}