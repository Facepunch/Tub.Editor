using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Facepunch
{
    public static class BinaryWriterExtensions
    {
        public static void Write( this BinaryWriter o, Vector3 vec )
        {
            o.Write( vec.x );
            o.Write( vec.y );
            o.Write( vec.z );
        }
    }
}