using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Facepunch.Editor;
using System;
using UnityEngine.Events;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( Boolean ) ), CanEditMultipleObjects]
    public class BooleanEditor : Editor
    {
        Boolean Boolean { get { return target as Boolean; } }

        private ReorderableList ActiveOnTrue;
        private ReorderableList ActiveOnFalse;

        protected int CurrentTab;

        protected virtual void OnEnable()
        {
            ActiveOnTrue = new ReorderableList( "Activate Objects When True", serializedObject, serializedObject.FindProperty( "ActiveOnTrue" ) );
            ActiveOnFalse = new ReorderableList( "Activate Objects When False", serializedObject, serializedObject.FindProperty( "ActiveOnFalse" ) );
        }

        public override void OnInspectorGUI()
        {
            EditorForBooleanCallbacks();
            EditorForBooleanObjects();
            serializedObject.ApplyModifiedProperties();
        }

        public void EditorForBooleanCallbacks()
        {
            EditorGUILayout.HelpBox( "Callbacks will be run on all clients - not just the master client. They may also all get run once when joining the server (we'll do some magic to stop effects and sounds).", MessageType.Info );
            EditorGUILayout.Space();

            {
                CallbackEditor( "OnBooleanTrue" );
                CallbackEditor( "OnBooleanFalse" );
                CallbackEditor( "OnBooleanChanged" );
            }
        }

        public void EditorForBooleanObjects()
        {
            {
                ActiveOnTrue.DoLayoutList();
                ActiveOnFalse.DoLayoutList();
            }
        }

        public void CallbackEditor( string name )
        {
            EditorGUILayout.PropertyField( serializedObject.FindProperty( name ), new GUIContent( name ), true );
        }

        public void TagsEditor( ref string[] current, string label )
        {
            if ( current == null ) current = new string[0];
            var joined = string.Join( ", ", current );

            EditorGUI.BeginChangeCheck();
            joined = EditorGUILayout.DelayedTextField( label, joined );
            if ( EditorGUI.EndChangeCheck() )
            {
                current = joined.Split( new char[] { ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries );
            }
        }

        [DrawGizmo( GizmoType.Selected | GizmoType.NonSelected )]
        static void DrawBooleanGizmos( Boolean obj, GizmoType gizmoType )
        {
            if ( !SceneView.currentDrawingSceneView )
                return;

            if ( Vector3.Distance( obj.transform.position, Camera.current.transform.position ) > 5.0f )
                return;

            if ( obj.ActiveOnTrue == null ) obj.ActiveOnTrue = new GameObject[0];
            if ( obj.ActiveOnFalse == null ) obj.ActiveOnFalse = new GameObject[0];

            foreach ( var o in obj.ActiveOnTrue )
            {
                if ( o == null ) continue;
                Utility.DrawArrowedLabel( obj.gameObject, o, Color.green, "ActiveOnTrue", 0 );
            }

            foreach ( var o in obj.ActiveOnFalse )
            {
                if ( o == null ) continue;
                Utility.DrawArrowedLabel( obj.gameObject, o, Color.red, "ActiveOnFalse", 1 );
            }

            Utility.DrawEventConnections( obj.gameObject, obj.OnBooleanChanged, Color.grey, "OnBooleanChanged", 0.2f );
            Utility.DrawEventConnections( obj.gameObject, obj.OnBooleanFalse, Color.yellow, "OnBooleanFalse", 0.5f );
            Utility.DrawEventConnections( obj.gameObject, obj.OnBooleanTrue, Color.cyan, "OnBooleanTrue", 0.8f );

            SceneView.currentDrawingSceneView.Repaint();
        }
    }
}


