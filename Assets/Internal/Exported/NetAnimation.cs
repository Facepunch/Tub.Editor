using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;
using System.Linq;

namespace Tub
{

    public class NetAnimation : Networked, INetworkStatic
    {
        public AnimationClip AnimationClip;

        [Range( 0, 5 )]
        public float Speed = 1.0f;

        [Range( 0, 1 )]
        public float Delta;

        protected EventMap animationEvent;
        protected Animation Animation;
        protected float lastTime = 0.0f;

        private void Awake()
        {
            GetComponents();
        }

        public void GetComponents()
        {
            Animation = GetComponent<Animation>();
            animationEvent = GetComponent<EventMap>();
        }

        public virtual void Update()
        {
            if ( AnimationClip == null ) return;

            UpdateToTime( (Delta * AnimationClip.length) + (Net.Time * Speed), true );
        }

        public void UpdateToTime( float time, bool triggerEvents = false )
        {
            if ( AnimationClip == null ) return;

            AnimationClip.SampleAnimation( gameObject, time );

            if ( triggerEvents )
            {
                TriggerEvents( Mathf.Min( time, lastTime ), Mathf.Max( time, lastTime ) );
            }

            lastTime = time;
        }

        public virtual void TriggerEvents( float minTime, float maxTime )
        {
            if ( animationEvent == null ) return;

            var events = AnimationClip.events; // does this create garbage?
            var delta = maxTime - minTime;

            minTime = Wrap( minTime, AnimationClip );
            maxTime = Wrap( maxTime, AnimationClip );

            for ( int i = 0; i < events.Length; i++ )
            {
                var t = events[i].time;

                // In Between
                if ( t > minTime && t < maxTime )
                {
                    animationEvent.TriggerEvent( events[i].stringParameter );
                    continue;
                }

                if ( t < maxTime && t > maxTime - delta )
                {
                    animationEvent.TriggerEvent( events[i].stringParameter );
                    continue;
                }

                if ( t > minTime && t < minTime + delta )
                {
                    animationEvent.TriggerEvent( events[i].stringParameter );
                    continue;
                }
            }
        }

        static float Wrap( float f, AnimationClip clip )
        {
            switch ( clip.wrapMode )
            {
                case WrapMode.Default:
                case WrapMode.Clamp:
                    {
                        return Mathf.Clamp( f, 0, clip.length );
                    }

                case WrapMode.Loop:
                    {
                        return f % clip.length;
                    }

                case WrapMode.PingPong:
                    {
                        f = f % clip.length * 2;
                        if ( f > clip.length ) f = clip.length - (f - clip.length);
                        return f;
                    }
            }

            return f;
        }

        public virtual void SetClip( string Name )
        {
            AnimationClip = Animation.GetClip( Name );
        }
    }

}

#if UNITY_EDITOR

[UnityEditor.CustomEditor( typeof( Tub.NetAnimation ) )]
public class NetAnimationEditor : UnityEditor.Editor
{
    [UnityEditor.DrawGizmo( UnityEditor.GizmoType.Selected | UnityEditor.GizmoType.NonSelected | UnityEditor.GizmoType.InSelectionHierarchy )]
    static void DrawNetAnimation( Tub.NetAnimation obj, UnityEditor.GizmoType gizmoType )
    {
        if ( !UnityEditor.SceneView.currentDrawingSceneView )
            return;

        if ( UnityEditor.AnimationMode.InAnimationMode() )
            return;

        obj.GetComponents();

        if ( UnityEditor.EditorApplication.isPaused )
        {
            obj.UpdateToTime( 0 );
            return;
        }

        if ( obj.GetComponentsInChildren<Transform>().Contains( UnityEditor.Selection.activeTransform ) )
        {
            obj.Update();
            Facepunch.Editor.WireframePreview.Draw( obj.transform, obj.transform.position, obj.transform.rotation, Color.cyan );

            obj.UpdateToTime( 0 );
        }
        else
        {
            obj.Update();
        }

        UnityEditor.SceneView.currentDrawingSceneView.Repaint();
    }
}

#endif
