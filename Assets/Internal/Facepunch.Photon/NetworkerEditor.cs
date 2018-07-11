#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;

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

            if ( Application.isPlaying )
            {
                EditorGUILayout.LabelField( $"View ID: {t.viewIdField}", EditorStyles.whiteMiniLabel );
                EditorGUILayout.LabelField( $"This Owner ID: {t.viewIdField}", EditorStyles.whiteMiniLabel );
                return;
            }

            ApplyNetworkerConstraints( t );

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

        static string NetworkertHash( Networker networker )
        {
            return $"{string.Join( ";", networker.ObservedComponents.Select( x => x?.ToString() ) )}-{networker.ownershipTransfer}-{networker.synchronization}-{networker.SyncTransforms}";
        }

        public static void ApplyNetworkerConstraints( Networker networker )
        {
            if ( networker.ObservedComponents == null )
            {
                networker.ObservedComponents = new List<Component>();
            }

            var hash = NetworkertHash( networker );

            var observables = networker.GetComponents<INetworkObservable>();

            networker.ObservedComponents.Clear();
            networker.ObservedComponents.AddRange( observables.Select( x => x as Component ) );

            if ( networker.SyncTransforms && networker.GetComponent<INetworkStatic>() != null )
            {
                networker.SyncTransforms = false;
            }

            if ( !networker.SyncTransforms )
            {
                networker.ObservedComponents.Remove( networker );
            }

            if ( networker.GetComponent<INetworkTakeover>() != null )
            {
                networker.ownershipTransfer = OwnershipOption.Takeover;
            }

            if ( networker.GetComponent<INetworkTakeover>() == null )
            {
                networker.ownershipTransfer = OwnershipOption.Fixed;
            }

            if ( networker.ObservedComponents.Count > 0 )
            {
                networker.synchronization = ViewSynchronization.UnreliableOnChange;
            }

            if ( networker.ObservedComponents.Count == 0 )
            {
                networker.synchronization = ViewSynchronization.Off;
            }

            if ( hash != NetworkertHash( networker ) )
            {
               // Debug.Log( $"Updating Networker {networker.gameObject.name} - {hash}" );
                UnityEditor.EditorUtility.SetDirty( networker.gameObject );
            }

        }

        [PostProcessScene]
        public static void ApplyNetworkerConstraintsAll()
        {
            Facepunch.Networker[] views = SceneManager.GetActiveScene().GetRootGameObjects()
                                    .SelectMany( x => x.GetComponentsInChildren<Facepunch.Networker>( true ) )
                                    .OrderBy( x => x.gameObject.GetInstanceID() )
                                    .ToArray();

            foreach ( var view in views )
            {
                ApplyNetworkerConstraints( view );
            }
        }
    }
}

#endif