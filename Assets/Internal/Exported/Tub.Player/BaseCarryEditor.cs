#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Facepunch.Editor;

namespace Tub
{

    [CustomEditor( typeof( BaseCarry ) )]
    public class BaseCarryEditor : Editor
    {
        [DrawGizmo( GizmoType.Selected | GizmoType.NonSelected | GizmoType.InSelectionHierarchy )]
        static void DrawBaseCarry( BaseCarry obj, GizmoType gizmoType )
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

            var mat = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>( "Assets/Editor/Hand.mat" );

            if ( obj.LeftHandPosition )
                Utility.DrawQuad( mat, Color.white, obj.LeftHandPosition.position, obj.LeftHandPosition.rotation * Quaternion.Euler( 0, 180, 0 ) * Quaternion.Euler( -20, 0, 0 ) , new Vector3( 0.25f, 0.25f, -0.25f ) );

            if ( obj.RightHandPosition )
                Utility.DrawQuad( mat, Color.white, obj.RightHandPosition.position, obj.RightHandPosition.rotation * Quaternion.Euler( 20, 0, 0 ), Vector3.one * 0.25f );
        }
    }
}

#endif