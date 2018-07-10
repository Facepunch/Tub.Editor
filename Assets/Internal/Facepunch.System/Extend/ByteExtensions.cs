using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch.Extend
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Gets a float from the 4 bytes of memory at the specified offset
        /// </summary>
        public static unsafe float ReadFloat( this byte[] buffer, int iOffset = 0 )
        {
            fixed ( byte* b = &buffer[iOffset] )
            {
                return *(float*)( b );
            }
        }

        /// <summary>
        /// Sets 4 bytes of memory representing the float at the specified offset
        /// </summary>
        public static unsafe void WriteFloat( this byte[] buffer, float f, int iOffset = 0 )
        {
            byte* data = (byte*)&f;

            buffer[iOffset + 0] = data[0];
            buffer[iOffset + 1] = data[1];
            buffer[iOffset + 2] = data[2];
            buffer[iOffset + 3] = data[3];
        }
    }
}
