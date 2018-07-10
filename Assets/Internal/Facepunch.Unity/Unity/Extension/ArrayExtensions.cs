using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public static class ArrayExtensions
    {
        public static T Random<T>( this T[] arry )
        {
            return arry[UnityEngine.Random.Range( 0, arry.Length )];
        }
    }
}