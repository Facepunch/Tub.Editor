using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public static class DebugDraw
    {
        public static void Point( Vector3 position, Color color, float scale = 1.0f, float duration = 0, bool depthTest = true )
        {
            color = (color == default( Color )) ? Color.white : color;

            Debug.DrawRay( position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale, color, duration, depthTest );
            Debug.DrawRay( position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale, color, duration, depthTest );
            Debug.DrawRay( position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale, color, duration, depthTest );
        }

        public static void Point( Vector3 position, float scale = 1.0f, float duration = 0, bool depthTest = true )
        {
            Point( position, Color.white, scale, duration, depthTest );
        }

        public static void Bounds( Bounds bounds, Color color, float duration = 0, bool depthTest = true )
        {
            Vector3 center = bounds.center;

            float x = bounds.extents.x;
            float y = bounds.extents.y;
            float z = bounds.extents.z;

            Vector3 ruf = center + new Vector3( x, y, z );
            Vector3 rub = center + new Vector3( x, y, -z );
            Vector3 luf = center + new Vector3( -x, y, z );
            Vector3 lub = center + new Vector3( -x, y, -z );

            Vector3 rdf = center + new Vector3( x, -y, z );
            Vector3 rdb = center + new Vector3( x, -y, -z );
            Vector3 lfd = center + new Vector3( -x, -y, z );
            Vector3 lbd = center + new Vector3( -x, -y, -z );

            Debug.DrawLine( ruf, luf, color, duration, depthTest );
            Debug.DrawLine( ruf, rub, color, duration, depthTest );
            Debug.DrawLine( luf, lub, color, duration, depthTest );
            Debug.DrawLine( rub, lub, color, duration, depthTest );

            Debug.DrawLine( ruf, rdf, color, duration, depthTest );
            Debug.DrawLine( rub, rdb, color, duration, depthTest );
            Debug.DrawLine( luf, lfd, color, duration, depthTest );
            Debug.DrawLine( lub, lbd, color, duration, depthTest );

            Debug.DrawLine( rdf, lfd, color, duration, depthTest );
            Debug.DrawLine( rdf, rdb, color, duration, depthTest );
            Debug.DrawLine( lfd, lbd, color, duration, depthTest );
            Debug.DrawLine( lbd, rdb, color, duration, depthTest );
        }

        public static void Normal( Vector3 point, Vector3 normal, float size, Color color, float duration = 0, bool depthtest = true )
        {
            Line( point, point + normal.normalized * size, color, duration, depthtest );
            Circle( point, normal.normalized, color, size * 0.5f, duration, depthtest );
        }

        public static void Bounds( Bounds bounds, float duration = 0, bool depthTest = true )
        {
            Bounds( bounds, Color.white, duration, depthTest );
        }

        public static void LocalCube( Transform transform, Vector3 size, Color color, Vector3 center = default( Vector3 ), float duration = 0, bool depthTest = true )
        {
            Vector3 lbb = transform.TransformPoint( center + ((-size) * 0.5f) );
            Vector3 rbb = transform.TransformPoint( center + (new Vector3( size.x, -size.y, -size.z ) * 0.5f) );

            Vector3 lbf = transform.TransformPoint( center + (new Vector3( size.x, -size.y, size.z ) * 0.5f) );
            Vector3 rbf = transform.TransformPoint( center + (new Vector3( -size.x, -size.y, size.z ) * 0.5f) );

            Vector3 lub = transform.TransformPoint( center + (new Vector3( -size.x, size.y, -size.z ) * 0.5f) );
            Vector3 rub = transform.TransformPoint( center + (new Vector3( size.x, size.y, -size.z ) * 0.5f) );

            Vector3 luf = transform.TransformPoint( center + ((size) * 0.5f) );
            Vector3 ruf = transform.TransformPoint( center + (new Vector3( -size.x, size.y, size.z ) * 0.5f) );

            Debug.DrawLine( lbb, rbb, color, duration, depthTest );
            Debug.DrawLine( rbb, lbf, color, duration, depthTest );
            Debug.DrawLine( lbf, rbf, color, duration, depthTest );
            Debug.DrawLine( rbf, lbb, color, duration, depthTest );

            Debug.DrawLine( lub, rub, color, duration, depthTest );
            Debug.DrawLine( rub, luf, color, duration, depthTest );
            Debug.DrawLine( luf, ruf, color, duration, depthTest );
            Debug.DrawLine( ruf, lub, color, duration, depthTest );

            Debug.DrawLine( lbb, lub, color, duration, depthTest );
            Debug.DrawLine( rbb, rub, color, duration, depthTest );
            Debug.DrawLine( lbf, luf, color, duration, depthTest );
            Debug.DrawLine( rbf, ruf, color, duration, depthTest );
        }

        public static void LocalCube( Transform transform, Vector3 size, Vector3 center = default( Vector3 ), float duration = 0, bool depthTest = true )
        {
            LocalCube( transform, size, Color.white, center, duration, depthTest );
        }

        public static void LocalCube( Matrix4x4 space, Vector3 size, Color color, Vector3 center = default( Vector3 ), float duration = 0, bool depthTest = true )
        {
            color = (color == default( Color )) ? Color.white : color;

            Vector3 lbb = space.MultiplyPoint3x4( center + ((-size) * 0.5f) );
            Vector3 rbb = space.MultiplyPoint3x4( center + (new Vector3( size.x, -size.y, -size.z ) * 0.5f) );

            Vector3 lbf = space.MultiplyPoint3x4( center + (new Vector3( size.x, -size.y, size.z ) * 0.5f) );
            Vector3 rbf = space.MultiplyPoint3x4( center + (new Vector3( -size.x, -size.y, size.z ) * 0.5f) );

            Vector3 lub = space.MultiplyPoint3x4( center + (new Vector3( -size.x, size.y, -size.z ) * 0.5f) );
            Vector3 rub = space.MultiplyPoint3x4( center + (new Vector3( size.x, size.y, -size.z ) * 0.5f) );

            Vector3 luf = space.MultiplyPoint3x4( center + ((size) * 0.5f) );
            Vector3 ruf = space.MultiplyPoint3x4( center + (new Vector3( -size.x, size.y, size.z ) * 0.5f) );

            Debug.DrawLine( lbb, rbb, color, duration, depthTest );
            Debug.DrawLine( rbb, lbf, color, duration, depthTest );
            Debug.DrawLine( lbf, rbf, color, duration, depthTest );
            Debug.DrawLine( rbf, lbb, color, duration, depthTest );

            Debug.DrawLine( lub, rub, color, duration, depthTest );
            Debug.DrawLine( rub, luf, color, duration, depthTest );
            Debug.DrawLine( luf, ruf, color, duration, depthTest );
            Debug.DrawLine( ruf, lub, color, duration, depthTest );

            Debug.DrawLine( lbb, lub, color, duration, depthTest );
            Debug.DrawLine( rbb, rub, color, duration, depthTest );
            Debug.DrawLine( lbf, luf, color, duration, depthTest );
            Debug.DrawLine( rbf, ruf, color, duration, depthTest );
        }

        public static void LocalCube( Matrix4x4 space, Vector3 size, Vector3 center = default( Vector3 ), float duration = 0, bool depthTest = true )
        {
            LocalCube( space, size, Color.white, center, duration, depthTest );
        }

        public static void Circle( Vector3 position, Vector3 up, Color color, float radius = 1.0f, float duration = 0, bool depthTest = true )
        {
            Vector3 _up = up.normalized * radius;
            Vector3 _forward = Vector3.Slerp( _up, -_up, 0.5f );
            Vector3 _right = Vector3.Cross( _up, _forward ).normalized * radius;

            Matrix4x4 matrix = new Matrix4x4();

            matrix[0] = _right.x;
            matrix[1] = _right.y;
            matrix[2] = _right.z;

            matrix[4] = _up.x;
            matrix[5] = _up.y;
            matrix[6] = _up.z;

            matrix[8] = _forward.x;
            matrix[9] = _forward.y;
            matrix[10] = _forward.z;

            Vector3 _lastPoint = position + matrix.MultiplyPoint3x4( new Vector3( Mathf.Cos( 0 ), 0, Mathf.Sin( 0 ) ) );
            Vector3 _nextPoint = Vector3.zero;

            color = (color == default( Color )) ? Color.white : color;

            for ( var i = 0; i < 91; i++ )
            {
                _nextPoint.x = Mathf.Cos( (i * 4) * Mathf.Deg2Rad );
                _nextPoint.z = Mathf.Sin( (i * 4) * Mathf.Deg2Rad );
                _nextPoint.y = 0;

                _nextPoint = position + matrix.MultiplyPoint3x4( _nextPoint );

                Debug.DrawLine( _lastPoint, _nextPoint, color, duration, depthTest );
                _lastPoint = _nextPoint;
            }
        }

        public static void Circle( Vector3 position, Color color, float radius = 1.0f, float duration = 0, bool depthTest = true )
        {
            Circle( position, Vector3.up, color, radius, duration, depthTest );
        }

        public static void Circle( Vector3 position, Vector3 up, float radius = 1.0f, float duration = 0, bool depthTest = true )
        {
            Circle( position, up, Color.white, radius, duration, depthTest );
        }

        public static void Circle( Vector3 position, float radius = 1.0f, float duration = 0, bool depthTest = true )
        {
            Circle( position, Vector3.up, Color.white, radius, duration, depthTest );
        }

        public static void Sphere( Vector3 position, Color color, float radius = 1.0f, float duration = 0, bool depthTest = true )
        {
            float angle = 10.0f;

            Vector3 x = new Vector3( position.x, position.y + radius * Mathf.Sin( 0 ), position.z + radius * Mathf.Cos( 0 ) );
            Vector3 y = new Vector3( position.x + radius * Mathf.Cos( 0 ), position.y, position.z + radius * Mathf.Sin( 0 ) );
            Vector3 z = new Vector3( position.x + radius * Mathf.Cos( 0 ), position.y + radius * Mathf.Sin( 0 ), position.z );

            Vector3 new_x;
            Vector3 new_y;
            Vector3 new_z;

            for ( int i = 1; i < 37; i++ )
            {

                new_x = new Vector3( position.x, position.y + radius * Mathf.Sin( angle * i * Mathf.Deg2Rad ), position.z + radius * Mathf.Cos( angle * i * Mathf.Deg2Rad ) );
                new_y = new Vector3( position.x + radius * Mathf.Cos( angle * i * Mathf.Deg2Rad ), position.y, position.z + radius * Mathf.Sin( angle * i * Mathf.Deg2Rad ) );
                new_z = new Vector3( position.x + radius * Mathf.Cos( angle * i * Mathf.Deg2Rad ), position.y + radius * Mathf.Sin( angle * i * Mathf.Deg2Rad ), position.z );

                Debug.DrawLine( x, new_x, color, duration, depthTest );
                Debug.DrawLine( y, new_y, color, duration, depthTest );
                Debug.DrawLine( z, new_z, color, duration, depthTest );

                x = new_x;
                y = new_y;
                z = new_z;
            }
        }

        public static void Sphere( Vector3 position, float radius = 1.0f, float duration = 0, bool depthTest = true )
        {
            Sphere( position, Color.white, radius, duration, depthTest );
        }
        public static void Cylinder( Vector3 start, Vector3 end, Color color, float radius = 1, float duration = 0, bool depthTest = true )
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp( up, -up, 0.5f );
            Vector3 right = Vector3.Cross( up, forward ).normalized * radius;

            //Radial circles
            Circle( start, up, color, radius, duration, depthTest );
            Circle( end, -up, color, radius, duration, depthTest );
            Circle( (start + end) * 0.5f, up, color, radius, duration, depthTest );

            //Side lines
            Debug.DrawLine( start + right, end + right, color, duration, depthTest );
            Debug.DrawLine( start - right, end - right, color, duration, depthTest );

            Debug.DrawLine( start + forward, end + forward, color, duration, depthTest );
            Debug.DrawLine( start - forward, end - forward, color, duration, depthTest );

            //Start endcap
            Debug.DrawLine( start - right, start + right, color, duration, depthTest );
            Debug.DrawLine( start - forward, start + forward, color, duration, depthTest );

            //End endcap
            Debug.DrawLine( end - right, end + right, color, duration, depthTest );
            Debug.DrawLine( end - forward, end + forward, color, duration, depthTest );
        }

        public static void Cylinder( Vector3 start, Vector3 end, float radius = 1, float duration = 0, bool depthTest = true )
        {
            Cylinder( start, end, Color.white, radius, duration, depthTest );
        }

        public static void Cone( Vector3 position, Vector3 direction, Color color, float angle = 45, float duration = 0, bool depthTest = true )
        {
            float length = direction.magnitude;

            Vector3 _forward = direction;
            Vector3 _up = Vector3.Slerp( _forward, -_forward, 0.5f );
            Vector3 _right = Vector3.Cross( _forward, _up ).normalized * length;

            direction = direction.normalized;

            Vector3 slerpedVector = Vector3.Slerp( _forward, _up, angle / 90.0f );

            float dist;
            var farPlane = new Plane( -direction, position + _forward );
            var distRay = new Ray( position, slerpedVector );

            farPlane.Raycast( distRay, out dist );

            Debug.DrawRay( position, slerpedVector.normalized * dist, color );
            Debug.DrawRay( position, Vector3.Slerp( _forward, -_up, angle / 90.0f ).normalized * dist, color, duration, depthTest );
            Debug.DrawRay( position, Vector3.Slerp( _forward, _right, angle / 90.0f ).normalized * dist, color, duration, depthTest );
            Debug.DrawRay( position, Vector3.Slerp( _forward, -_right, angle / 90.0f ).normalized * dist, color, duration, depthTest );

            Circle( position + _forward, direction, color, (_forward - (slerpedVector.normalized * dist)).magnitude, duration, depthTest );
            Circle( position + (_forward * 0.5f), direction, color, ((_forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude, duration, depthTest );
        }

        public static void Cone( Vector3 position, Vector3 direction, float angle = 45, float duration = 0, bool depthTest = true )
        {
            Cone( position, direction, Color.white, angle, duration, depthTest );
        }

        public static void Cone( Vector3 position, Color color, float angle = 45, float duration = 0, bool depthTest = true )
        {
            Cone( position, Vector3.up, color, angle, duration, depthTest );
        }

        public static void Cone( Vector3 position, float angle = 45, float duration = 0, bool depthTest = true )
        {
            Cone( position, Vector3.up, Color.white, angle, duration, depthTest );
        }

        public static void Line( Vector3 position, Vector3 endposition, Color color, float duration = 0, bool depthTest = true )
        {
            Debug.DrawLine( position, endposition, color, duration, depthTest );
        }

        public static void Arrow( Vector3 position, Vector3 direction, Color color, float duration = 0, bool depthTest = true )
        {
            Debug.DrawRay( position, direction, color, duration, depthTest );
            Cone( position + direction, -direction * 0.333f, color, 15, duration, depthTest );
        }

        public static void Arrow( Vector3 position, Vector3 direction, float duration = 0, bool depthTest = true )
        {
            Arrow( position, direction, Color.white, duration, depthTest );
        }

        public static void Capsule( Vector3 start, Vector3 end, Color color, float radius = 1, float duration = 0, bool depthTest = true )
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp( up, -up, 0.5f );
            Vector3 right = Vector3.Cross( up, forward ).normalized * radius;

            float height = (start - end).magnitude;
            float sideLength = Mathf.Max( 0, (height * 0.5f) - radius );
            Vector3 middle = (end + start) * 0.5f;

            start = middle + ((start - middle).normalized * sideLength);
            end = middle + ((end - middle).normalized * sideLength);

            //Radial circles
            Circle( start, up, color, radius, duration, depthTest );
            Circle( end, -up, color, radius, duration, depthTest );

            //Side lines
            Debug.DrawLine( start + right, end + right, color, duration, depthTest );
            Debug.DrawLine( start - right, end - right, color, duration, depthTest );

            Debug.DrawLine( start + forward, end + forward, color, duration, depthTest );
            Debug.DrawLine( start - forward, end - forward, color, duration, depthTest );

            for ( int i = 1; i < 26; i++ )
            {

                //Start endcap
                Debug.DrawLine( Vector3.Slerp( right, -up, i / 25.0f ) + start, Vector3.Slerp( right, -up, (i - 1) / 25.0f ) + start, color, duration, depthTest );
                Debug.DrawLine( Vector3.Slerp( -right, -up, i / 25.0f ) + start, Vector3.Slerp( -right, -up, (i - 1) / 25.0f ) + start, color, duration, depthTest );
                Debug.DrawLine( Vector3.Slerp( forward, -up, i / 25.0f ) + start, Vector3.Slerp( forward, -up, (i - 1) / 25.0f ) + start, color, duration, depthTest );
                Debug.DrawLine( Vector3.Slerp( -forward, -up, i / 25.0f ) + start, Vector3.Slerp( -forward, -up, (i - 1) / 25.0f ) + start, color, duration, depthTest );

                //End endcap
                Debug.DrawLine( Vector3.Slerp( right, up, i / 25.0f ) + end, Vector3.Slerp( right, up, (i - 1) / 25.0f ) + end, color, duration, depthTest );
                Debug.DrawLine( Vector3.Slerp( -right, up, i / 25.0f ) + end, Vector3.Slerp( -right, up, (i - 1) / 25.0f ) + end, color, duration, depthTest );
                Debug.DrawLine( Vector3.Slerp( forward, up, i / 25.0f ) + end, Vector3.Slerp( forward, up, (i - 1) / 25.0f ) + end, color, duration, depthTest );
                Debug.DrawLine( Vector3.Slerp( -forward, up, i / 25.0f ) + end, Vector3.Slerp( -forward, up, (i - 1) / 25.0f ) + end, color, duration, depthTest );
            }
        }

        public static void Capsule( Vector3 start, Vector3 end, float radius = 1, float duration = 0, bool depthTest = true )
        {
            Capsule( start, end, Color.white, radius, duration, depthTest );
        }
    }
}