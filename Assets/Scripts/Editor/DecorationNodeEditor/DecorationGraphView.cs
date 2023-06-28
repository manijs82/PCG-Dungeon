using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DecorationGraphView : GraphView
    {
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
        }

        private GraphElement CreateEntryPointNode()
        {
            var entryPoint = new DecorationNode()
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                settings = new DecorationObject(),
                entryPoint = true
            };
            
            entryPoint.capabilities &= ~Capabilities.Deletable;
            
            entryPoint.RefreshExpandedState();
            entryPoint.SetPosition(new Rect(new Vector2(100, 200), DefaultNodeSize));

            return entryPoint;
        }
    }
}