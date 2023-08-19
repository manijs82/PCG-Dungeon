using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using PortDirection = UnityEditor.Experimental.GraphView.Direction;

namespace Editor
{
    public class DecorationGraphView : GraphView
    {
        public event Action<DecorationNode> OnNodeRemoved;
        public event Action<DecorationNode> OnNodeCreated;
        public event Action<DecorationNode> OnEntryNodeCreated;
        public event Action<DecorationNode> OnEntryNodeLoaded;
        public static readonly Vector2 DefaultNodeSize = new Vector2(200, 150);

        public DecorationNode entryPointNode;

        public DecorationGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            
            AddElement(CreateEntryPointNode());

            graphViewChanged = OnGraphChange;
        }

        private GraphElement CreateEntryPointNode()
        {
            entryPointNode = new DecorationNode()
            {
                title = "START",
                GUID = Guid.NewGuid().ToString()
            };
            SetEntryPoint(entryPointNode);
            
            entryPointNode.capabilities -= Capabilities.Deletable;
            
            entryPointNode.RefreshExpandedState();
            entryPointNode.SetPosition(new Rect(new Vector2(100, 200), DefaultNodeSize));

            var button = new Button(() => CreateNode(entryPointNode, "CHILD"))
            {
                text = "Add Child"
            };
            entryPointNode.titleButtonContainer.Add(button);

            return entryPointNode;
        }
        
        private GraphViewChange OnGraphChange(GraphViewChange change)
        {
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    HandleElementRemove(element);
                }
            }

            return change;
        }

        private void HandleElementRemove(GraphElement element)
        {
            if (element is UnityEditor.Experimental.GraphView.Edge edge)
            {
                var change = new GraphViewChange
                {
                    elementsToRemove = new List<GraphElement> { edge.input.node }
                };
                OnGraphChange(change);
                RemoveElement(edge.input.node);

                var outNode = edge.output.node;
                RemoveElement(edge.output);
                
                if(outNode != null)
                {
                    outNode.RefreshExpandedState();
                    outNode.RefreshPorts();
                }
            }
            if (element is Node node)
            {
                var list = node.outputContainer.Children().ToList();
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var socket = list[i];
                    if (socket is not Port port) continue;
                    
                    var connectedEdge = edges
                        .FirstOrDefault(x => x.output == port);
                    if (connectedEdge == null) continue;
                    
                    connectedEdge.input.Disconnect(connectedEdge);
                    OnGraphChange(new GraphViewChange()
                    {
                        elementsToRemove = new List<GraphElement> { connectedEdge }
                    });
                    RemoveElement(connectedEdge);
                }
                
                OnNodeRemoved?.Invoke(node as DecorationNode);
            }
        }

        private DecorationNode CreateNode(DecorationNode parentNode, string nodeName)
        {
            var decorationNode = new DecorationNode(parentNode)
            {
                title = nodeName,
                GUID = Guid.NewGuid().ToString()
            };

            var edge = ConnectNodes(parentNode, decorationNode);

            PositionChildNodes(parentNode);
            
            var button = new Button(() => CreateNode(decorationNode, "CHILD"))
            {
                text = "Add Child"
            };
            decorationNode.titleButtonContainer.Add(button);
            AddElement(decorationNode);
            AddElement(edge);

            OnNodeCreated?.Invoke(decorationNode);
            return decorationNode;
        }

        public void LoadNode(NodeData nodeData)
        {
            var decorationNode = new DecorationNode()
            {
                title = "CHILD",
                GUID = nodeData.guid
            };
            if (nodeData.isEntry) SetEntryPoint(decorationNode, false);
            
            decorationNode.SetPosition(nodeData.position);
            
            if(decorationNode.entryPoint)
            {
                decorationNode.title = "START";
                decorationNode.capabilities -= Capabilities.Deletable;
            }
            
            var button = new Button(() => CreateNode(decorationNode, "CHILD"))
            {
                text = "Add Child"
            };
            decorationNode.titleButtonContainer.Add(button);
            
            AddElement(decorationNode);
        }

        private void SetEntryPoint(DecorationNode decorationNode, bool created = true)
        {
            if(entryPointNode != null)
                RemoveElement(entryPointNode);
            entryPointNode = decorationNode;
            decorationNode.SetEntry();
            if(created) OnEntryNodeCreated?.Invoke(decorationNode);
            else OnEntryNodeLoaded?.Invoke(decorationNode);
        }

        public void LoadEdges(NodeLinkData linkData)
        {
            var fromNode = GetNode(linkData.startNodeGuid);
            var toNode = GetNode(linkData.endNodeGuid);
            var edge = ConnectNodes(fromNode, toNode);
            toNode.parent = fromNode;
            
            AddElement(edge);
        }

        private DecorationNode GetNode(string guid)
        {
            foreach (var node in nodes)
            {
                if(node is not DecorationNode decorationNode) continue;
                if (decorationNode.GUID == guid) return decorationNode;
            }

            return null;
        }

        private Edge ConnectNodes(Node from, Node to)
        {
            var outPort = AddPort(from, PortDirection.Output);
            var inPort = AddPort(to, PortDirection.Input);
            return outPort.ConnectTo(inPort);
        }

        private void PositionChildNodes(DecorationNode parentNode)
        {
            var list = parentNode.outputContainer.Children().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is Port port)
                {
                    var node = port.connections.ToList()[0].input.node;
                    var yOffset = list.Count == 1 ? 0 : Mathf.Lerp(-list.Count/2f * 100, list.Count/2f * 100, i / (float)(list.Count - 1));
                    node.SetPosition(new Rect(parentNode.GetPosition().position + Vector2.right * 300 + Vector2.up * yOffset, DefaultNodeSize));
                    
                }
            }
        }

        private Port AddPort(Node node, PortDirection portDirection)
        {
            var port = node.InstantiatePort(Orientation.Horizontal, portDirection, Port.Capacity.Multi, typeof(float));
            port.portName = portDirection.ToString();
            
            if(portDirection == PortDirection.Input)
                node.inputContainer.Add(port);
            if(portDirection == PortDirection.Output)
                node.outputContainer.Add(port);
            
            port.capabilities -= Capabilities.Copiable;
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            return port;
        }

        public DecorationNode GetChildAt(DecorationNode node, int index)
        {
            var port = node.outputContainer.ElementAt(index) as Port;
            var child = edges.FirstOrDefault(e => e.output == port)?.input.node as DecorationNode;
            return child;
        }
    }
}