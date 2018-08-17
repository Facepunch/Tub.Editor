using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Facepunch.Editor
{
    public static class Utility
    {
        public static Vector3 GetObjectCenter( GameObject obj )
        {
            var rs = obj.GetComponentsInChildren<Renderer>().Where( x => !(x is ParticleSystemRenderer) ).ToArray();
            if ( rs.Length == 0 ) return obj.transform.position;

            var bounds = rs[0].bounds;

            foreach ( var r in rs )
            {
                bounds.Encapsulate( r.bounds );
            }

            return bounds.center;
        }

        public static void DrawArrowedLabel( GameObject from, GameObject to, Color color, string label, float timeOffset )
        {
            var a = Utility.GetObjectCenter( from );
            var b = Utility.GetObjectCenter( to );
            var delta = b - a;
            var tan = Vector3.Cross( Camera.current.transform.forward, delta ).normalized * 0.02f * (timeOffset - 0.5f);

            a += tan;
            b += tan;

            if ( delta.magnitude <= 0.001f )
                delta = Vector3.up * 0.1f;

            Handles.color = new Color( color.r, color.g, color.b, 0.3f );
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
            Handles.DrawDottedLine( a, a + delta, 2.0f );

            Handles.color = new Color( color.r, color.g, color.b, 0.9f );
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
            Handles.DrawDottedLine( a, a + delta, 2.0f );

            var ConePos = a + delta * ((Time.realtimeSinceStartup * 0.1f + timeOffset * 0.5f) % 1.0f);

            var eyeDelta = ConePos - Camera.current.transform.position;
            if ( eyeDelta.magnitude < 5 )
            {
                var isLooking = Vector3.Angle( eyeDelta, Camera.current.transform.rotation * Vector3.forward );

                if ( isLooking < 90 )
                {
                    Handles.color = new Color( color.r, color.g, color.b, 0.3f );
                    Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
                    Handles.ConeHandleCap( 0, ConePos, Quaternion.LookRotation( delta ), 0.02f, EventType.Repaint );

                    Handles.color = new Color( color.r, color.g, color.b, 0.9f );
                    Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
                    Handles.ConeHandleCap( 0, ConePos, Quaternion.LookRotation( delta ), 0.02f, EventType.Repaint );

                    ConePos = a + delta * ((Time.realtimeSinceStartup * 0.1f + timeOffset * 0.5f - 0.1f) % 1.0f);

                    // todo - cache these styles
                    var style = new GUIStyle( "Label" );
                    style.fontSize = 9;
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = Color.black;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.fixedWidth = 1;
                    style.fixedHeight = 1;
                    style.stretchWidth = false;
                    style.stretchHeight = false;
                    style.clipping = TextClipping.Overflow;
                    style.padding = new RectOffset( 0, -2, 0, -2  );

                    Handles.Label( ConePos, label, style );

                    style = new GUIStyle( "Label" );
                    style.fontSize = 9;
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = color;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.fixedWidth = 1;
                    style.fixedHeight = 1;
                    style.stretchWidth = false;
                    style.stretchHeight = false;
                    style.clipping = TextClipping.Overflow;
                    style.padding = new RectOffset( 0, 0, 0, 0 );


                    Handles.Label( ConePos, label, style );


                }
            }
        }

        public static void DrawEventConnections( GameObject obj, UnityEventBase e, Color color, string label, float timeOffset )
        {
            var center = Utility.GetObjectCenter( obj );

            for ( int i = 0; i < e.GetPersistentEventCount(); i++ )
            {
                var target = e.GetPersistentTarget( i );
                var method = e.GetPersistentMethodName( i );

                if ( target == null )
                {
                    Handles.color = Color.red;
                    Handles.DrawDottedLine( center, center + UnityEngine.Random.onUnitSphere * 0.5f, 5.0f );
                    continue;
                }
                
                GameObject go = target as GameObject;

                if ( target is Component ) go = (target as Component).gameObject;

                DrawArrowedLabel( obj, go, color, $"{label}\n{target.GetType().Name}.{method}", timeOffset );
            }
        }

        public static void DrawQuad( Material material, Color color, Vector3 position, Quaternion rotation, Vector3 scale )
        {
            material.SetPass( 0 );

            GL.PushMatrix();
            GL.MultMatrix( Matrix4x4.TRS( position, rotation, scale ) );
            GL.Begin( GL.QUADS );
            GL.Color( color );
            GL.TexCoord2( 0, 0 );
            GL.Vertex3( -0.5f, 0, -0.5f );

            GL.TexCoord2( 1, 0 );
            GL.Vertex3( 0.5f, 0, -0.5f );

            GL.TexCoord2( 1, 1 );
            GL.Vertex3( 0.5f, 0, 0.5f );

            GL.TexCoord2( 0, 1 );
            GL.Vertex3( -0.5f, 0, 0.5f );

            GL.End();
            GL.PopMatrix();
        }

    }
}
