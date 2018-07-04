using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Facepunch.Extend
{
    public static class RayExtensions
    {
        /// <summary>
        /// Return the postion along this ray closest to this position
        /// </summary>
        public static Vector3 ClosestPoint( this Ray ray, Vector3 position )
        {
            Vector3 point2origin = ray.origin - position;
            return point2origin - Vector3.Dot( point2origin, ray.direction ) * ray.direction;
        }

        /// <summary>
        /// Return the distance along this ray closest to this position
        /// </summary>
        public static float ClosestDistance( this Ray ray, Vector3 position )
        {
            return ray.ClosestPoint( position ).magnitude;
        }
    }
}
