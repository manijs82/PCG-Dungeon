using Mani.Hierarchy;
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
        private VisualElement detailPanelElement;
        private VolumeDetailPanel detailPanel;

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
            InitializeToolbar();
            LoadHierarchyAsset();
            InitializeDetailPanel();
        }

        private void OnGUI()
        {
            
        }

        private void OnDisable()
        {
            SaveGraph();
            graphView.OnNodeCreated -= OnAddNode;
            graphView.OnEntryNodeCreated -= OnCreateRootNode;
            rootVisualElement.Remove(graphView);
            rootVisualElement.Remove(toolbar);
        }

        private void InitializeGraphView()
        {
            if(rootVisualElement.Contains(graphView))
            {
                rootVisualElement.Remove(graphView);
                graphView.OnNodeCreated -= OnAddNode;
                graphView.OnEntryNodeCreated -= OnCreateRootNode;
            }
            graphView = new DecorationGraphView()
            {
                name = "Decoration Graph"
            };
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
            graphView.SendToBack();
            graphView.OnNodeCreated += OnAddNode;
            graphView.OnEntryNodeCreated += OnCreateRootNode;
            graphView.OnNodeRemoved += OnRemoveNode;
        }

        private void InitializeToolbar()
        {
            toolbar = new Toolbar();

            objectField = new ObjectField("Decoration Volume Hierarchy");
            objectField.objectType = typeof(DecorationVolumeHierarchy);
            objectField.RegisterValueChangedCallback(evt => LoadGraph(evt.newValue as DecorationVolumeHierarchy));
            toolbar.Add(objectField);
            
            toolbar.Add(new Button(SaveGraph) {text = "Save"});

            rootVisualElement.Add(toolbar);
        }

        private void LoadHierarchyAsset()
        {
            var hierarchyAsset = AssetDatabase.LoadAssetAtPath<DecorationVolumeHierarchy>(EditorPrefs.GetString("ObjectPath"));
            objectField.SetValueWithoutNotify(hierarchyAsset);

            if (hierarchyAsset != null)
                LoadGraph(hierarchyAsset);
        }

        private void InitializeDetailPanel()
        {
            detailPanelElement = new VisualElement();
            rootVisualElement.Add(detailPanelElement);
            detailPanelElement.PlaceInFront(graphView);
            detailPanelElement.style.position = new StyleEnum<Position>(Position.Absolute);
            detailPanelElement.style.left = new StyleLength(2);
            detailPanelElement.style.top = new StyleLength(20);
            detailPanelElement.style.backgroundColor = new StyleColor(EditorGUIUtility.isProSkin
                ? new Color32(56, 56, 56, 255)
                : new Color32(194, 194, 194, 255));
            detailPanelElement.style.width = new StyleLength(300);
            detailPanelElement.style.height = new StyleLength(600);

            detailPanel = new VolumeDetailPanel(detailPanelElement, hierarchy);
        }

        private void OnAddNode(DecorationNode node)
        {
            if (hierarchy == null) return;
            if (node.parent == null) return;
            hierarchy.volumeHierarchy.AddChild(node.parent.volume, node.volume);
            EditorUtility.SetDirty(hierarchy);
        }

        private void OnRemoveNode(DecorationNode node)
        {
            if (hierarchy == null) return;
            if (node.volume.Parent == null) return;
            node.volume.Parent.RemoveNeighbor(node.volume);
            EditorUtility.SetDirty(hierarchy);
        }

        private void OnCreateRootNode(DecorationNode node)
        {
            if (hierarchy == null) return;
            hierarchy.volumeHierarchy.Root = node.volume;
            EditorUtility.SetDirty(hierarchy);
        }

        private void SaveGraph()
        {
            if(hierarchy == null) return;
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
            EditorUtility.SetDirty(hierarchy);
            AssetDatabase.SaveAssetIfDirty(hierarchy);
            LoadGraph(hierarchy);
        }

        private void LoadGraph(DecorationVolumeHierarchy hierarchy)
        {
            InitializeGraphView();
            this.hierarchy = hierarchy;
            EditorPrefs.SetString("ObjectPath", AssetDatabase.GetAssetPath(hierarchy));

            foreach (var nodeData in hierarchy.graphViewData.nodes) graphView.LoadNode(nodeData);
            foreach (var link in hierarchy.graphViewData.links) graphView.LoadEdges(link);
            
            SyncHierarchyData();
        }

        private void SyncHierarchyData()
        {
            SyncChildrenNodeValue(graphView.entryPointNode, hierarchy.volumeHierarchy.Root);
            graphView.entryPointNode.volume = hierarchy.volumeHierarchy.Root;
        }

        private void SyncChildrenNodeValue(DecorationNode parentNode, HierarchyNode<DecorationVolume> parentVolume)
        {
            for (int i = 0; i < parentVolume.ChildCount; i++)
            {
                var child = graphView.GetChildAt(parentNode, i);
                child.volume = parentVolume.Children[i];
                SyncChildrenNodeValue(child, child.volume);
            }
        }
    }
}