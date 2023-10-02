using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Editor
{
    public class DungeonEditorWindow : EditorWindow
    {
        private Dungeon dungeon;
        private bool hasDungeon;
        private bool isEditingRooms;
        private Vector3[] roomPositions;
        [SerializeField] private string screenshotPath;
        [SerializeField] private Tilemap tilemapToCapture;

        private SerializedObject so;
        private SerializedProperty screenshotPathProperty;
        private SerializedProperty tilemapToCaptureProperty;

        [MenuItem("PCG_Dungeon/Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DungeonEditorWindow>();
            window.titleContent = new GUIContent("DungeonEditor");
            window.Show();
        }

        private void OnEnable()
        {
            screenshotPath = EditorPrefs.GetString("ScreenshotPath", "C:/");
            
            so = new SerializedObject(this);
            screenshotPathProperty = so.FindProperty("screenshotPath");
            tilemapToCaptureProperty = so.FindProperty("tilemapToCapture");
            
            SceneView.duringSceneGui += OnSceneView;
            Generator.OnDungeonGenerated += d =>
            {
                dungeon = d;
                hasDungeon = true;
            };
        }

        private void OnGUI()
        {
            so.Update();
            
            EditorGUILayout.PropertyField(screenshotPathProperty);
            EditorGUILayout.PropertyField(tilemapToCaptureProperty);

            so.ApplyModifiedProperties();
            
            EditorGUILayout.Space();
            
            if (!Application.isPlaying || dungeon == null)
            {
                hasDungeon = false;
                isEditingRooms = false;
                return;
            }
            if (!hasDungeon) return;
            
            if (GUILayout.Button("TakeScreenshot"))
            {
                if(tilemapToCapture != null)
                    ScreenshotUtils.TakeScreenShot(dungeon, tilemapToCapture, screenshotPath);
            }

            if (GUILayout.Button("ReloadVisuals"))
            {
                dungeon.MakeGrid();
            }

            if (GUILayout.Button("Edit Rooms"))
            {
                roomPositions = new Vector3[dungeon.roomGraph.NodeCount];
                for (int i = 0; i < roomPositions.Length; i++) 
                    roomPositions[i] = dungeon.roomGraph.Nodes[i].Value.startPoint;
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

        private void OnDisable()
        {
            EditorPrefs.SetString("ScreenshotPath", screenshotPath);
        }
    }
}