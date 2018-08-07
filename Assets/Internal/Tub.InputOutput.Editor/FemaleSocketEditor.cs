using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( FemaleSocket ) ), CanEditMultipleObjects]
    public class FemaleSocketEditor : BooleanEditor
    {
        FemaleSocket FemaleSocket { get { return target as FemaleSocket; } }

        public override void OnInspectorGUI()
        {
         //   GUILayout.Label( "Think of a Female Socket as a <b>Lock</b> or a <b>Electrical Socket</b>. It can accept only one male at a time and it will mount them.", TubEditorStyles.HelpBox );

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();
                CurrentTab = GUILayout.Toolbar( CurrentTab, new string[] { "Female Socket", "Boolean Callbacks", "Boolean Objects" }, "LargeButton", GUI.ToolbarButtonSize.FitToContents );
                EditorGUILayout.Space();
                if ( CurrentTab == 0 ) EditorForFemaleSocket();
                if ( CurrentTab == 1 ) EditorForBooleanCallbacks();
                if ( CurrentTab == 2 ) EditorForBooleanObjects();
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        public void EditorForFemaleSocket()
        {
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "CurrentMale" ) );

#if !TUB_EDITOR

            FemaleSocket.UpdateMountedPosition();
#endif

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField( serializedObject.FindProperty( "CanMountAnything" ) );

            if ( FemaleSocket.CanMountAnything ) EditorGUI.BeginDisabledGroup( true );
            TagsEditor( ref FemaleSocket.MountableIdentifiers, "Can Mount Any With Tag:" );
            if ( FemaleSocket.CanMountAnything ) EditorGUI.EndDisabledGroup();

            TagsEditor( ref FemaleSocket.AcceptableIdentifiers, "Is True When Mounted Has Tag:" );

            EditorGUILayout.PropertyField( serializedObject.FindProperty( "MountPoint" ), new GUIContent( "Mount Point:" ) );

            if ( FemaleSocket.MountPoint == null )
                FemaleSocket.MountPoint = FemaleSocket.transform;

            EditorGUILayout.Space();

            TagsEditor( ref FemaleSocket.InfluenceTags, "Influence Tags:" );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "InfluenceSpeedMultiplier" ), new GUIContent( "Influence Speed Multiplier:" ) );

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField( serializedObject.FindProperty( "Lock" ), new GUIContent( "Lock" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "LockDirection" ), new GUIContent( "Locked If True" ) );


            
            

        }
    }
}