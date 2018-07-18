using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( Integer ) ), CanEditMultipleObjects]
    public class IntegerEditor : BooleanEditor
    {
        Integer Integer { get { return target as Integer; } }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            { 
                EditorGUILayout.BeginVertical( GUILayout.Width( 80 ) );
                {
                    CurrentTab = GUILayout.SelectionGrid( CurrentTab, new string[] { "Integer", "Boolean Callbacks", "Boolean Objects" }, 1 );
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    if ( CurrentTab == 0 ) EditorForInteger();
                    if ( CurrentTab == 1 ) EditorForBooleanCallbacks();
                    if ( CurrentTab == 2 ) EditorForBooleanObjects();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }


        public void EditorForInteger()
        {
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "BooleanTestHigherThan" ), new GUIContent( "BooleanTestHigherThan" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "BooleanTestLowerThan" ), new GUIContent( "BooleanTestLowerThan" ) );
        }
    }


}
