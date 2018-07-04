using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch.Extend
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Given a number, will format as a memory value, ie 10gb, 4mb
        /// </summary>
        public static string FormatBytes<T>( this T input, bool shortFormat = false ) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            ulong i = (ulong) Convert.ChangeType( input, typeof( ulong ) );

            double readable = (double)i;
            string suffix;
            if ( i >= 0x1000000000000000 ) // Exabyte
            {
                suffix = "eb";
                readable = (double)( i >> 50 );
            }
            else if ( i >= 0x4000000000000 ) // Petabyte
            {
                suffix = "pb";
                readable = (double)( i >> 40 );
            }
            else if ( i >= 0x10000000000 ) // Terabyte
            {
                suffix = "tb";
                readable = (double)( i >> 30 );
            }
            else if ( i >= 0x40000000 ) // Gigabyte
            {
                suffix = "gb";
                readable = (double)( i >> 20 );
            }
            else if ( i >= 0x100000 ) // Megabyte
            {
                suffix = "mb";
                readable = (double)( i >> 10 );
            }
            else if ( i >= 0x400 ) // Kilobyte
            {
                suffix = "kb";
                readable = (double)i;
            }
            else
            {
                return i.ToString( "0b" ); // Byte
            }
            readable /= 1024;

            return readable.ToString( shortFormat ? "0" : "0.00" ) + suffix;
        }

        /// <summary>
        /// Clamp a number between two values
        /// </summary>
        public static T Clamp<T>( this T input, T min, T max ) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if ( input.CompareTo( min ) < 0 ) return min;
            if ( input.CompareTo( max ) > 0 ) return max;

            return input;
        }

        public static string FormatSeconds( this ulong i ) { return FormatSeconds( (long)i ); }
        public static string FormatSeconds( this long s )
        {
            var m = System.Math.Floor( (float)s / 60.0f );
            var h = System.Math.Floor( (float)m / 60.0f );
            var d = System.Math.Floor( (float)h / 24.0f );
            var w = System.Math.Floor( (float)d / 7.0f );

            if ( s < 60 ) return string.Format( "{0}s", s ); // 1s
            if ( m < 60 ) return string.Format( "{1}m{0}s", s % 60, m, h, d, w ); // 5m3s
            if ( h < 48 ) return string.Format( "{2}h{1}m{0}s", s % 60, m % 60, h, d, w ); // 6h40m34h
            if ( d < 7 ) return string.Format( "{3}d{2}h{1}m{0}s", s % 60, m % 60, h % 24, d % 7, w ); // 5d15h15m10s

            return string.Format( "{4}w{3}d{2}h{1}m{0}s", s % 60, m % 60, h % 24, d % 7, w );
        }

        public static string FormatSecondsLong( this ulong i ) { return FormatSecondsLong( (long)i ); }
        public static string FormatSecondsLong( this long s )
        {
            var m = System.Math.Floor( (float)s / 60.0f );
            var h = System.Math.Floor( (float)m / 60.0f );
            var d = System.Math.Floor( (float)h / 24.0f );
            var w = System.Math.Floor( (float)d / 7.0f );

            if ( s < 60 ) return string.Format( "{0} seconds", s ); // 1s
            if ( m < 60 ) return string.Format( "{1} minutes, {0} seconds", s % 60, m, h, d, w ); // 5m3s
            if ( h < 48 ) return string.Format( "{2} hours and {1} minutes", s % 60, m % 60, h, d, w ); // 6h40m34h
            if ( d < 7 ) return string.Format( "{3} days, {2} hours and {1} minutes", s % 60, m % 60, h % 24, d % 7, w ); // 5d15h15m10s

            return string.Format( "{3} days, {2} hours and {1} minutes", s % 60, m % 60, h % 24, d, w );
        }

        public static string FormatNumberShort( this ulong i ) { return FormatNumberShort( (long)i ); }
        public static string FormatNumberShort( this long num )
        {
            if ( num >= 100000 )
            {
                return FormatNumberShort( num / 1000 ) + "K";
            }

            if ( num >= 10000 )
            {
                return ( num / 1000D ).ToString( "0.#" ) + "K";
            }

            return num.ToString( "#,0" );
        }
    }
}
