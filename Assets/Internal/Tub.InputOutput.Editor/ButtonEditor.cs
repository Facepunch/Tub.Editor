using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( Button ) ), CanEditMultipleObjects]
    public class ButtonEditor : BooleanEditor
    {
        Button Button { get { return target as Button; } }

        public override void OnInspectorGUI()
        {
         //   GUILayout.Label( "Think of a Female Socket as a <b>Lock</b> or a <b>Electrical Socket</b>. It can accept only one male at a time and it will mount them.", TubEditorStyles.HelpBox );

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();
                CurrentTab = GUILayout.Toolbar( CurrentTab, new string[] { "Button", "Boolean Callbacks", "Boolean Objects" }, "LargeButton", GUI.ToolbarButtonSize.FitToContents );
                EditorGUILayout.Space();
                if ( CurrentTab == 0 ) EditorForButton();
                if ( CurrentTab == 1 ) EditorForBooleanCallbacks();
                if ( CurrentTab == 2 ) EditorForBooleanObjects();
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        public void EditorForButton()
        {
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "Type" ), new GUIContent( "Button press style" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "RepeatTime" ), new GUIContent( "Seconds before can press again" ) );

            if ( Button.Type == ButtonPressMode.OnWithTimedReset )
            {
                EditorGUILayout.PropertyField( serializedObject.FindProperty( "DelayBeforeReset" ), new GUIContent( "Reset to off after seconds" ) );
            }

            EditorGUILayout.PropertyField( serializedObject.FindProperty( "EnabledIf" ), new GUIContent( "Enabled If True" ) );
        }
    }

}
