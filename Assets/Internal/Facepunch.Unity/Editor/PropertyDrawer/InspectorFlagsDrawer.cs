using UnityEditor;
using UnityEngine;
using System;

namespace Facepunch
{
    [CustomPropertyDrawer( typeof( InspectorFlagsAttribute ) )]
    public class InspectorFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            property.intValue = EditorGUI.MaskField( position, label, property.intValue, property.enumDisplayNames );
        }
    }
}