using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ProbableElement<>))]
    public class ProbableElementDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty element = property.FindPropertyRelative("element");
            SerializedProperty weight = property.FindPropertyRelative("weight");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(element.type.Replace("PPtr<$", "").Replace(">", ""), GUILayout.Width(50));
            EditorGUILayout.PropertyField(element, GUIContent.none, true, GUILayout.MaxWidth(200));
            EditorGUILayout.LabelField("Weight", GUILayout.Width(40));
            EditorGUILayout.PropertyField(weight, GUIContent.none, true, GUILayout.MaxWidth(75));
            EditorGUILayout.EndHorizontal();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 1;
        }
    }
}