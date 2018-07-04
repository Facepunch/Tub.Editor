using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Facepunch.Editor;
using System;
using UnityEngine.Events;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( BooleanTest ) ), CanEditMultipleObjects]
    public class BooleanTestEditor : IntegerEditor
    {
        private ReorderableList Targets;

        protected override void OnEnable()
        {
            base.OnEnable();

            Targets = new ReorderableList( "Booleans To Test", serializedObject, serializedObject.FindProperty( "Targets" ) );
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();
                CurrentTab = GUILayout.Toolbar( CurrentTab, new string[] { "Boolean Test", "Integer", "Boolean Callbacks", "Boolean Objects" }, "LargeButton", GUI.ToolbarButtonSize.FitToContents );
                EditorGUILayout.Space();

                if ( CurrentTab == 0 ) EditorForBooleanTest();
                if ( CurrentTab == 1 ) EditorForInteger();
                if ( CurrentTab == 2 ) EditorForBooleanCallbacks();
                if ( CurrentTab == 3 ) EditorForBooleanObjects();
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        void EditorForBooleanTest()
        {
            Targets.DoLayoutList();
        }

        [DrawGizmo( GizmoType.Selected | GizmoType.NonSelected )]
        static void DrawSkeletonGizmo( BooleanTest obj, GizmoType gizmoType )
        {
            if ( Vector3.Distance( Utility.GetObjectCenter( obj.gameObject ), Camera.current.transform.position ) > 5.0f )
                return;

            if ( obj.Targets == null )
                obj.Targets = new Boolean[0];

            foreach ( var o in obj.Targets )
            {
                Utility.DrawArrowedLabel( o.gameObject, obj.gameObject, Color.cyan, $"BooleanTest\n({obj.BooleanValue})", (o.GetInstanceID() % 1000) / 1000.0f );
            }
        }
    }
}


