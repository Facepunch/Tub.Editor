using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( Exchangeable ) ), CanEditMultipleObjects]
    public class ExchangeableEditor : BooleanEditor
    {
        Exchangeable Exchangeable { get { return target as Exchangeable; } }

        public override void OnInspectorGUI()
        {
         //   GUILayout.Label( "Think of a Female Socket as a <b>Lock</b> or a <b>Electrical Socket</b>. It can accept only one male at a time and it will mount them.", TubEditorStyles.HelpBox );

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();
                CurrentTab = GUILayout.Toolbar( CurrentTab, new string[] { "Exchangeable", "Boolean Callbacks", "Boolean Objects" }, "LargeButton", GUI.ToolbarButtonSize.FitToContents );
                EditorGUILayout.Space();
                if ( CurrentTab == 0 ) EditorForExchangeable();
                if ( CurrentTab == 1 ) EditorForBooleanCallbacks();
                if ( CurrentTab == 2 ) EditorForBooleanObjects();
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        public void EditorForExchangeable()
        {
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "InfluenceName" ), new GUIContent( "Influence" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "Time" ), new GUIContent( "Time To Take" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "Destination" ), new GUIContent( "Exchange Object" ) );
        }
    }
}