using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Editor
{
    public class DecorationNode : Node
    {
        public DecorationObject settings;
        public string GUID;
        public bool entryPoint;

        public DecorationNode()
        {
            settings = new DecorationObject();
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
        }
    }
}