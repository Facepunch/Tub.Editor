using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;
using System.Linq;

namespace Tub
{
    public class NetPosition : MonoBehaviour
    {
        public Vector3 Movement = new Vector3( 0, 1, 0 );
        public Vector3 Offset = new Vector3( 0, 0, 0 );
        public float Speed = 1.0f;

        public Easings.Functions Ease;

        public bool UseEaseOut;
        public Easings.Functions EaseOut;

        protected float lastTime = 0.0f;

        Vector3 initialPos;

        public void Awake()
        {
            initialPos = transform.localPosition;
        }

        public virtual void Update()
        {
            UpdateToTime( Net.Time, true );
        }

        public void UpdateToTime( float time, bool triggerEvents = false )
        {
            var e = Ease;
            var f = (Net.Time * Speed) % 2.0f;

            if ( f > 1 )
            {
                f = 1 - (f-1);

                if ( UseEaseOut )
                    e = EaseOut;
            }         

            transform.localPosition = initialPos + Offset + Movement * Easings.Interpolate( f, e );
        }
    }
}

#if UNITY_EDITOR

[UnityEditor.CustomEditor( typeof( Tub.NetPosition ) )]
public class NetPositionEditor : UnityEditor.Editor
{
    [UnityEditor.DrawGizmo( UnityEditor.GizmoType.Selected | UnityEditor.GizmoType.NonSelected | UnityEditor.GizmoType.InSelectionHierarchy )]
    static void DrawNetPosition( Tub.NetPosition obj, UnityEditor.GizmoType gizmoType )
    {
        if ( !UnityEditor.SceneView.currentDrawingSceneView )
            return;

        if ( UnityEditor.EditorApplication.isPlaying )
            return;

        if ( !obj.enabled )
            return;

        var lp = obj.transform.localPosition;
        var lr = obj.transform.localRotation;

        obj.Awake();

        obj.Update();
        Facepunch.Editor.WireframePreview.Draw( obj.transform, obj.transform.position, obj.transform.rotation, Color.cyan );

        obj.transform.localPosition = lp;
        obj.transform.localRotation = lr;

        UnityEditor.SceneView.currentDrawingSceneView.Repaint();
    }
}

#endif
