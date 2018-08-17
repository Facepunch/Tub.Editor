using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Return the vector with the y component set to 0
        /// </summary>
        public static Vector3 XZ( this Vector3 o, float y = 0.0f )
        {
            return new Vector3( o.x, y, o.z );
        }

        /// <summary>
        /// Given a vector, nullify it along an axis
        /// </summary>
        /// <returns></returns>
        public static Vector3 RemoveAxis( this Vector3 o, Vector3 axis )
        {
            var n = axis.normalized;
            return o - n * Vector3.Dot( o, n );
        }


        /// <summary>
        /// Given a vector, trace down to the ground
        /// </summary>
        /// <returns></returns>
        public static Vector3 TraceDown( this Vector3 o, int mask = 1 << 0, float maxDistance = 512.0f )
        {
            RaycastHit hitInfo;
            if ( Physics.Raycast( new Ray( o + Vector3.up * 0.002f, Vector3.down), out hitInfo, maxDistance, mask ) )
            {
                return hitInfo.point;
            }

            return o;
        }
        
    }
}