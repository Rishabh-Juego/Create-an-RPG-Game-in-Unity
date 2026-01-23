#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TGL.RPG.Utilities.Attributes
{
    [CustomPropertyDrawer(typeof(ShowInInspectorAttribute))]
    public class ShowInInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the target object
            object target = property.serializedObject.targetObject;
        
            // Get the property info (assuming it's a property, not a field)
            PropertyInfo propInfo = target.GetType().GetProperty(property.name, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
            if (propInfo != null)
            {
                // Get the value
                object value = propInfo.GetValue(target);
            
                // Display it as read-only
                GUI.enabled = false;
                EditorGUI.TextField(position, label.text, value?.ToString() ?? "null");
                GUI.enabled = true;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Property not found");
            }
        }
    }
}

#endif

