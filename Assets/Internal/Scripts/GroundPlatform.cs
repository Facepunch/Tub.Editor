using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Facepunch;

public class GroundPlatform : Networked
{
    public Transform Root;
    public Transform targetRoot => Root == null ? transform : Root;
}



#if UNITY_EDITOR

[UnityEditor.CustomEditor( typeof( GroundPlatform ) )]
public class GroundPlatformEditor : UnityEditor.Editor
{
    [UnityEditor.DrawGizmo( UnityEditor.GizmoType.Selected | UnityEditor.GizmoType.NonSelected )]
    static void DrawSkeletonGizmo( GroundPlatform target, UnityEditor.GizmoType gizmoType )
    {
        if ( !UnityEditor.SceneView.currentDrawingSceneView ) return;

        if ( target.targetRoot.localScale != Vector3.one || target.targetRoot.lossyScale != Vector3.one )
        {
            UnityEditor.Handles.color = Color.red;

            for( int i=0; i<16; i++ )
            UnityEditor.Handles.DrawDottedLine( target.transform.position, target.transform.position + Random.onUnitSphere, 3.0f );
        }

        UnityEditor.SceneView.currentDrawingSceneView.Repaint();
    }

    public override void OnInspectorGUI()
    {
        var target = this.target as GroundPlatform;

        if ( target.targetRoot.localScale != Vector3.one || target.targetRoot.lossyScale != Vector3.one )
        {
            UnityEditor.EditorGUILayout.HelpBox( "Your ground's scale should be exactly [1,1,1] or this won't work.", UnityEditor.MessageType.Error );
        }

        base.OnInspectorGUI();

    }
}

#endif
