#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Facepunch
{
    [CustomEditor( typeof( Networker ) )]
    public class NetworkerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var t = target as Networker;
            if ( t == null )
                return;

            //    EditorGUILayout.BeginVertical( GUI.skin.box );

            if ( Application.isPlaying )
            {
                EditorGUILayout.LabelField( $"View ID: {t.viewIdField}", EditorStyles.whiteMiniLabel );
                EditorGUILayout.LabelField( $"This Owner ID: {t.viewIdField}", EditorStyles.whiteMiniLabel );
                return;
            }

            EditorGUIUtility.labelWidth = 100;

            EditorGUILayout.BeginHorizontal( GUILayout.MinHeight( 20 ) );
            {

                EditorGUILayout.BeginVertical( GUI.skin.box, GUILayout.ExpandHeight( true ) );
                {
                    EditorGUILayout.LabelField( "Options", EditorStyles.whiteMiniLabel );

                    if ( t.GetComponent<INetworkStatic>() == null )
                    {
                        EditorGUILayout.PropertyField( serializedObject.FindProperty( "SyncTransforms" ), new GUIContent( "Sync Position" ) );
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical( GUI.skin.box, GUILayout.ExpandHeight( true ) );
                EditorGUILayout.LabelField( "Info", EditorStyles.whiteMiniLabel );
                EditorGUILayout.LabelField( $"ViewId", $"{t.viewIdField}" );
                EditorGUILayout.LabelField( $"Ownership", $"{t.ownershipTransfer}" );
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical( GUI.skin.box, GUILayout.ExpandHeight( true ) );
                EditorGUILayout.LabelField( "Watching", EditorStyles.whiteMiniLabel );

                foreach ( var c in t.ObservedComponents )
                {
                    if ( GUILayout.Button( ComponmentName( c ), "Label" ) )
                    {
                        // Do shit.
                    }
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        string ComponmentName( Component c )
        {
            if ( c == null ) return "(null)";
            if ( c == target ) return "Networker (this)";
            return c.GetType().Name;
        }

    }
}

#endif