using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DecorationEditor : EditorWindow
    {
        private DecorationGraphView graphView;

        [MenuItem("PCG_Dungeon/DecorationEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DecorationEditor>();
            window.titleContent = new GUIContent("Decoration Editor");
            window.Show();
        }

        private void OnEnable()
        {
            InitializeGraphView();
        }

        private void OnGUI()
        {
            
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }

        private void InitializeGraphView()
        {
            graphView = new DecorationGraphView()
            {
                name = "Decoration Graph"
            };
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
    }
}