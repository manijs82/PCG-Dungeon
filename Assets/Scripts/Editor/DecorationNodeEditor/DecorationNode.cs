using System;
using Mani.Hierarchy;
using UnityEditor.Experimental.GraphView;

namespace Editor
{
    public class DecorationNode : Node
    {
        public static event Action<DecorationNode> OnNodeSelected;
        public static event Action<DecorationNode> OnNodeUnSelected;

        public DecorationNode parent;
        public HierarchyNode<DecorationVolume> volume;
        public string GUID;
        public bool entryPoint;

        public DecorationNode(DecorationNode parent = null, HierarchyNode<DecorationVolume> volume = null)
        {
            this.volume = volume ?? new HierarchyNode<DecorationVolume>(new DecorationVolume());
            this.parent = parent;
        }

        public void SetEntry()
        {
            entryPoint = true;
            volume.SetAsRoot();
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            OnNodeUnSelected?.Invoke(this);
        }
    }
}