using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Facepunch
{
    public static class BinaryReaderExtensions
    {
        public static Vector3 ReadVector3( this BinaryReader o )
        {
            return new Vector3( o.ReadSingle(), o.ReadSingle(), o.ReadSingle() );
        }
    }
}