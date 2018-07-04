#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Facepunch;


namespace Facepunch
{
    [CustomPropertyDrawer( typeof( EventDefinition ) )]
    public class EventDefinitionDrawer : PropertyDrawer
    {
        // This allows us to use GuiLayout
        public override bool CanCacheInspectorGUI( SerializedProperty property ) => false;
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => -4;

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUILayout.BeginVertical( GUILayout.ExpandWidth( true ), GUILayout.MaxWidth( 10000 ) );

            EventBase eventbase = null;

            {
                EditorGUI.BeginChangeCheck();
                var type = property.FindPropertyRelative( "TypeName" ).stringValue;
                var selected = EditorGUILayout.Popup( Array.IndexOf( EventFactory.Identifiers, type ), EventFactory.DisplayNames );

                if ( selected >= 0 )
                {
                    eventbase = EventFactory.Get( EventFactory.Identifiers[selected] );

                    if ( EditorGUI.EndChangeCheck() )
                    {    
                        if ( eventbase != null )
                        {
                            property.FindPropertyRelative( "TypeName" ).stringValue = eventbase.Identifier;
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            if ( eventbase != null )
            {
                EditorGUI.indentLevel++;
                eventbase.Property = property;
                eventbase.Inspector();
                eventbase.Property = null;
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();

            property.serializedObject.ApplyModifiedProperties();
        }

    }
}

#endif