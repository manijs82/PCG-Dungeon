using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomPropertyDrawer(typeof(TileSet))]
    public class TileSetDrawer : PropertyDrawer
    {
        private static string[] Names =
        {
            "emptyTileSet",
            "wallTileSet",
            "groundTileSet",
            "doorTileSet"
        };
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Foldout foldout = new Foldout
            {
                text = property.displayName
            };

            SerializedProperty[] lists = new SerializedProperty[4];
            for (int i = 0; i < Names.Length; i++)
                lists[i] = property.FindPropertyRelative(Names[i]).FindPropertyRelative("list");

            for (var i = 0; i < lists.Length; i++)
            {
                PropertyField propertyField = new PropertyField(lists[i])
                {
                    label = Names[i]
                };
                
                foldout.Add(propertyField);
            }

            return foldout;
        }

        /*public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty[] lists = new SerializedProperty[4];
            for (int i = 0; i < Names.Length; i++)
                lists[i] = property.FindPropertyRelative(Names[i]).FindPropertyRelative("list");
            
            EditorGUILayout.fol
        }*/
    }
}