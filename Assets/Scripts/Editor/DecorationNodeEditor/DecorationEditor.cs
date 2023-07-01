using ManisDataStructures.Hierarchy;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DecorationEditor : EditorWindow
    {
        private DecorationVolumeHierarchy hierarchy;
        private DecorationGraphView graphView;
        private Toolbar toolbar;
        private ObjectField objectField;

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
            GenerateToolbar();
            var hierarchyAsset = AssetDatabase.LoadAssetAtPath<DecorationVolumeHierarchy>(EditorPrefs.GetString("ObjectPath"));
            objectField.SetValueWithoutNotify(hierarchyAsset);
            
            LoadGraph(hierarchyAsset);
        }

        private void OnGUI()
        {
            
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
            rootVisualElement.Remove(toolbar);
            SaveGraph();
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
        
        private void GenerateToolbar()
        {
            toolbar = new Toolbar();

            objectField = new ObjectField("Decoration Volume Hierarchy");
            objectField.objectType = typeof(DecorationVolumeHierarchy);
            objectField.RegisterValueChangedCallback(evt => LoadGraph(evt.newValue as DecorationVolumeHierarchy));
            toolbar.Add(objectField);
            
            toolbar.Add(new Button(SaveGraph) {text = "Save"});

            rootVisualElement.Add(toolbar);
        }

        private void SaveGraph()
        {
            var graphViewData = new GraphViewData();
            foreach (var node in graphView.nodes)
            {
                var decorationNode = node as DecorationNode;
                var nodeData = new NodeData(decorationNode.GUID, decorationNode.GetPosition(), decorationNode.entryPoint);
                
                graphViewData.nodes.Add(nodeData);
            }

            foreach (var edge in graphView.edges)
            {
                var fromNode = edge.output.node as DecorationNode;
                var toNode = edge.input.node as DecorationNode;
                
                graphViewData.links.Add(new NodeLinkData(fromNode.GUID, toNode.GUID));
            }

            hierarchy.graphViewData = graphViewData;
            hierarchy.volumeHierarchy = new Hierarchy<DecorationVolume>(
                new HierarchyNode<DecorationVolume>(new DecorationVolume()));
            EditorUtility.SetDirty(hierarchy);
            AssetDatabase.SaveAssetIfDirty(hierarchy);
        }

        private void LoadGraph(DecorationVolumeHierarchy hierarchy)
        {
            this.hierarchy = hierarchy;
            EditorPrefs.SetString("ObjectPath", AssetDatabase.GetAssetPath(hierarchy));

            foreach (var nodeData in hierarchy.graphViewData.nodes) graphView.LoadNode(nodeData);
            foreach (var link in hierarchy.graphViewData.links) graphView.LoadEdges(link);
        }
    }
}