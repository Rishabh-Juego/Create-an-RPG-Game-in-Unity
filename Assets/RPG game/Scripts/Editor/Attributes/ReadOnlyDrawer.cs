#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace TGL.RPG.Utilities.Attributes
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false; // Disable editing
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}
#endif