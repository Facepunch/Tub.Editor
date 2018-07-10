using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( BooleanAnimation ) ), CanEditMultipleObjects]
    public class BooleanAnimationEditor : BooleanEditor
    {
        BooleanAnimation BooleanAnimation { get { return target as BooleanAnimation; } }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            using ( new EditorGUILayout.VerticalScope() )
            {
                EditorGUILayout.HelpBox( "Will play the animation based on true/false of Target Boolean. Will itself be true if playing an animation (so can be used to disable buttons etc while playing)", MessageType.Info );

                CurrentTab = GUILayout.Toolbar( CurrentTab, new string[] { "Animation", "Boolean Callbacks", "Boolean Objects" } );

                if ( CurrentTab == 0 ) EditorForBooleanAnimation();
                if ( CurrentTab == 1 ) EditorForBooleanCallbacks();
                if ( CurrentTab == 2 ) EditorForBooleanObjects();
            }
        }


        public void EditorForBooleanAnimation()
        {
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "Boolean" ), new GUIContent( "Target Boolean" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "Animation" ), new GUIContent( "Target Animation" ) );

            EditorGUILayout.PropertyField( serializedObject.FindProperty( "OnClip" ), new GUIContent( "OnClip" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "OffClip" ), new GUIContent( "OffClip" ) );

            EditorGUILayout.PropertyField( serializedObject.FindProperty( "AllowInterupt" ), new GUIContent( "AllowI nterupt" ) );
        }
    }


}
