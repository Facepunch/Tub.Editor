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
    }
}