using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class VolumeDetailPanel
    {
        private VisualElement panel;
        private DecorationVolumeHierarchy volumeHierarchy;
        private DecorationNode currentNode;

        private EnumField environmentSettingsElement;

        public VolumeDetailPanel(VisualElement panel, DecorationVolumeHierarchy volumeHierarchy)
        {
            this.panel = panel;
            this.volumeHierarchy = volumeHierarchy;
            DecorationNode.OnNodeSelected += SetNode;
            //DecorationNode.OnNodeSelected += UnSetNode;

            environmentSettingsElement = new EnumField(EnvironmentType.Room);
        }

        private void SetNode(DecorationNode node)
        {
            if(node == null) return;
            UnSetNode();
            currentNode = node;
            panel.Add(environmentSettingsElement);

            environmentSettingsElement.RegisterValueChangedCallback(OnEnvironmentSettingsChanged);
            environmentSettingsElement.SetValueWithoutNotify(currentNode.volume.Value.environmentType);
        }

        private void UnSetNode()
        {
            currentNode = null;
            
            if(panel.Contains(environmentSettingsElement))
                panel.Remove(environmentSettingsElement);
            environmentSettingsElement.UnregisterValueChangedCallback(OnEnvironmentSettingsChanged);
        }

        private void OnEnvironmentSettingsChanged(ChangeEvent<Enum> evt)
        {
            currentNode.volume.Value.environmentType = (EnvironmentType)evt.newValue;
        }
    }
}