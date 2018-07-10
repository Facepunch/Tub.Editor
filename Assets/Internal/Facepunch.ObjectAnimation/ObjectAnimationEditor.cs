#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Facepunch
{
    [CustomEditor( typeof( ObjectAnimation ) )]
    public class ObjectAnimationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            bool justCreated = false;

            var t = target as ObjectAnimation;

            if ( t.AnimationClip == null )
            {
                t.AnimationClip = new AnimationClip();
                t.AnimationClip.legacy = true;
                t.AnimationClip.name = $"{t.gameObject.name} Clip";
                justCreated = true;
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();

            GUI.changed = false;

            t.AnimationClip.name = EditorGUILayout.TextField( new GUIContent( "Clip Name" ), t.AnimationClip.name );
            t.AnimationClip.wrapMode = (WrapMode) EditorGUILayout.EnumPopup( "Wrap Mode", t.AnimationClip.wrapMode );

            if ( GUI.changed )
            {
                EditorUtility.SetDirty( t.AnimationClip );
            }

            var othersReferencing = FindObjectsOfType( typeof( ObjectAnimation ) )
                            .Cast<ObjectAnimation>()
                            .Where( x => x.AnimationClip == t.AnimationClip )
                            .Where( x => x != t )
                            .ToArray();

            var anim = t.GetComponent<Animation>();

            if ( othersReferencing.Length > 0 )
            {
                EditorGUILayout.Space();

                if ( GUILayout.Button( "This animation is being referenced in other InlineAnimations.\nClick here to create a unique instance of it." ))
                {
                    Debug.Log( "Old Clip: " + t.AnimationClip );

                    t.AnimationClip = Instantiate<AnimationClip>( t.AnimationClip );
                    anim.clip = t.AnimationClip;
                    UnityEditor.AnimationUtility.SetAnimationClips( anim, new[] { t.AnimationClip } );
                    EditorUtility.SetDirty( t );

                    Debug.Log( "New Clip: " + t.AnimationClip );
                    justCreated = true;
                }
            }

            if ( anim == null )
            {
                anim = t.gameObject.AddComponent<Animation>();
                anim.playAutomatically = false;
                UnityEditor.AnimationUtility.SetAnimationClips( anim, new[] { t.AnimationClip } );

                anim.hideFlags = HideFlags.HideInInspector;

                EditorUtility.SetDirty( target );
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical( EditorStyles.helpBox, GUILayout.MaxWidth( 64 ), GUILayout.MaxHeight( 64 ) );

            var style = new GUIStyle( EditorStyles.helpBox )
            {
                stretchHeight = true,
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 0
            };

            GUILayout.Box( "Drag\nClip", style, GUILayout.ExpandWidth( true ), GUILayout.MinWidth( 10 ) );

            var boxrect = GUILayoutUtility.GetLastRect();
            if ( boxrect.Contains( Event.current.mousePosition ) &&  Event.current.type == EventType.MouseDrag )
            {
                // Clear out drag data
                DragAndDrop.PrepareStartDrag();

                // Set up what we want to drag
                DragAndDrop.objectReferences = new[] { t.AnimationClip };

                // Start the actual drag
                DragAndDrop.StartDrag( "Dragging title" );

                // Make sure no one uses the event after us
                Event.current.Use();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();


            if ( !justCreated )
            {
                // Are we a prefab?
                var prefab = PrefabUtility.GetPrefabParent( t.gameObject );
                if ( prefab != null )
                {
                    var path = AssetDatabase.GetAssetPath( t.AnimationClip );

                    if ( !path.EndsWith( ".prefab" ) )
                    {
                        AssetDatabase.AddObjectToAsset( t.AnimationClip, prefab );
                        AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( t.AnimationClip ) );
                    }

                    //
                    // TODO - if we remove this component, or this clip, we need to remove it from the prefab
                    //
                }
            }

        }
    }
}

#endif