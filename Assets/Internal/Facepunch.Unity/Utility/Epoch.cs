using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch.Math
{
    public static class Epoch
    {
        private static readonly DateTime epoch = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

        /// <summary>
        /// Returns the current Unix Epoch
        /// </summary>
        public static int Current
        {
            get
            {
                return (int)(DateTime.UtcNow.Subtract( epoch ).TotalSeconds);
            }
        }

        /// <summary>
        /// Convert an epoch to a datetime
        /// </summary>
        public static DateTime ToDateTime( decimal unixTime )
        {
            return epoch.AddSeconds( (long)unixTime );
        }

        /// <summary>
        /// Convert an datetime to an epoch
        /// </summary>
        public static int FromDateTime( DateTime time )
        {
            return (int)(time.Subtract( epoch ).TotalSeconds);
        }

    }
}
