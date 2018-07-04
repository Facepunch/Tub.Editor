using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Facepunch.Extend
{
    public static class StringExtensions
    {
        /// <summary>
        /// Puts quote marks around a string. Internal quotes are backslashed.
        /// </summary>
        public static string QuoteSafe( this string str )
        {
            str = str.Replace( "\"", "\\\"" ).TrimEnd( '\\' );
            return "\"" + str + "\"";
        }


        /// <summary>
        /// in  : I am "splitting a" string "because it's fun"
        /// out : ["I", "am", "splitting a", "string", "because it's fun"]
        /// </summary>
        public static string[] SplitQuotesStrings( this string input )
        {
            // Hide backslashed quotes - so we can retain them
            input = input.Replace( "\\\"", "&qute;" );

            var splitter = new Regex( "\"([^\"]+)\"|'([^']+)'|\\S+" );

            MatchCollection collection = splitter.Matches( input );

            string[] strArray = new string[collection.Count];
            for ( int i = 0; i < collection.Count; i++ )
            {
                strArray[i] = collection[i].Groups[0].Value.Trim( new char[] { ' ', '"' } );
                strArray[i] = strArray[i].Replace( "&qute;", "\"" );
            }

            return strArray;
        }

        /// <summary>
        /// 128-bit data type that returns sane results for almost any input.
        /// All other numeric types can cast from this.
        /// </summary>
        public static decimal ToDecimal( this string str, decimal Default = 0 )
        {
            decimal res = Default;
            decimal.TryParse( str, out res );
            return res;
        }

        /// <summary>
        /// Convert to float, if not then return Default
        /// </summary>
        public static float ToFloat( this string str, float Default = 0 )
        {
            return (float)str.ToDecimal( (decimal) Default );
        }

        /// <summary>
        /// Convert to int, if not then return Default
        /// </summary>
        public static int ToInt( this string str, int Default = 0 )
        {
            const decimal min = (decimal)int.MinValue;
            const decimal max = (decimal)int.MaxValue;

            decimal num = str.ToDecimal( Default );

            return num <= min ? int.MinValue : num >= max ? int.MaxValue : (int)num;
        }

        /// <summary>
        /// Try to convert to bool. Inputs can be true, false, yes, no, 0, 1, null (caps insensitive)
        /// </summary>
        public static bool ToBool( this string str )
        {
            if ( str == null ) return false;
            if ( str == "1" ) return true;

            str = str.Trim();
            str = str.ToLower();

            if ( str == "true" ) return true;
            if ( str == "t" ) return true;
            if ( str == "yes" ) return true;
            if ( str == "y" ) return true;

            return false;
        }

        /// <summary>
        /// If the string is longer than this amount of characters then truncate it
        /// If appendage is defined, it will be appended to the end of truncated strings (ie, "..")
        /// </summary>
        public static string Truncate( this string str, int maxLength, string appendage = null )
        {
            if ( string.IsNullOrEmpty( str ) ) return str;
            if ( str.Length <= maxLength ) return str;

            if ( appendage != null )
                maxLength -= appendage.Length;

            str = str.Substring( 0, maxLength );

            if ( appendage == null )
                return str;

            return string.Concat( str, appendage );
        }

        private static char[] FilenameDelim = new[] { '/', '\\' };

        /// <summary>
        /// If the string is longer than this amount of characters then truncate it
        /// If appendage is defined, it will be appended to the end of truncated strings (ie, "..")
        /// </summary>
        public static string TruncateFilename( this string str, int maxLength, string appendage = null )
        {
            if ( string.IsNullOrEmpty( str ) ) return str;
            if ( str.Length <= maxLength ) return str;

            maxLength -= 3; //account for delimiter spacing

            string final = str;
            List<string> parts;

            int loops = 0;
            while ( loops++ < 100 )
            {
                parts = str.Split( FilenameDelim ).ToList();
                parts.RemoveRange( parts.Count - 1 - loops, loops );
                if ( parts.Count == 1 )
                {
                    return parts.Last();
                }

                parts.Insert( parts.Count - 1, "..." );
                final = string.Join( "/" , parts.ToArray() );
                if ( final.Length < maxLength )
                {
                    return final;
                }
            }

            return str.Split( FilenameDelim ).ToList().Last();
        }
        

        /// <summary>
        /// An extended Contains which takes a StringComparison
        /// </summary>
        public static bool Contains( this string source, string toCheck, StringComparison comp )
        {
            return source.IndexOf( toCheck, comp ) >= 0;
        }

        /// <summary>
        /// Given a large string, find all occurrances of a substring and return them with padding.
        /// This is useful in situations where you're searching for a word in a hug body of text, and
        /// want to show how it's used without displaying the whole text.
        /// </summary>
        public static string Snippet( this string source, string find, int padding )
        {
            if ( string.IsNullOrEmpty( find ) ) return string.Empty;

            StringBuilder sb = new StringBuilder();

            for ( int index = 0; index < source.Length; index += find.Length )
            {
                index = source.IndexOf( find, index, StringComparison.InvariantCultureIgnoreCase );
                if ( index == -1 )
                    break;

                var startPos = ( index - padding ).Clamp( 0, source.Length );
                var endPos = ( startPos + find.Length + padding * 2 ).Clamp( 0, source.Length );
                index = endPos;

                if ( sb.Length > 0 )
                    sb.Append( " ... " );

                sb.Append( source.Substring( startPos, endPos - startPos ) );
            }

            return sb.ToString();
        }

        private static readonly char[] _badCharacters =
        {
            // Ascii Table 0-31 - excluding tab, newline, return
            '\x00', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06',
            '\x07', '\x08', '\x09', '\x0B', '\x0C', '\x0D',
            '\x0E', '\x0F', '\x10', '\x12', '\x13', '\x14',
            '\x16', '\x17', '\x18', '\x19', '\x1A', '\x1B',
            '\x1C', '\x1D', '\x1E', '\x1F',
            
            '\xA0', // Non breaking space
            '\xAD', // Soft hyphen

            '\u2000', // En quad
            '\u2001', // Em quad
            '\u2002', // En space
            '\u2003', // Em space
            '\u2004', // Three per em space
            '\u2005', // Four per em space
            '\u2006', // Six per em space
            '\u2007', // Figure space
            '\u2008', // Punctuation space
            '\u2009', // Thin space
            '\u200A', // Hair space
            '\u200B', // Zero width space
            '\u200C', // Zero width non-joiner
            '\u200D', // Zero width joiner
            '\u200E', '\u200F',

            '\u2010', // Hyphen
            '\u2011', // Non breaking hyphen
            '\u2012', // Figure dash
            '\u2013', // En dash
            '\u2014', // Em dash
            '\u2015', // Horizontal bar
            '\u2016', // Double vertical line
            '\u2017', // Double low line
            '\u2018', // Left single quotation mark
            '\u2019', // Right single quotation mark
            '\u201A', // Single low-9 quotation mark
            '\u201B', // Single high reversed-9 quotation mark
            '\u201C', // Left double quotation mark
            '\u201D', // Right double quotation mark
            '\u201E', // Double low-9 quotation mark
            '\u201F', // Double high reversed-9 quotation mark

            '\u2028', // Line separator
            '\u2029', // Paragraph separator
            '\u202F', // Narrow no-break space
            
            '\u205F', // Medium mathematical space
            '\u2060', // Word joiner

            '\u2420', // Symbol for space
            '\u2422', // Blank symbol
            '\u2423', // Open box
            
            '\u3000', // Ideographic space

            '\uFEFF'  // Zero width no-break space
        };

        /// <summary>
        /// Removes bad, invisible characters that are commonly used to exploit.
        /// https://en.wikipedia.org/wiki/Zero-width_non-joiner
        /// </summary>
        public static string RemoveBadCharacters( this string str )
        {
            str = new string( str.Where( x => !_badCharacters.Contains( x ) ).ToArray() );

            return str;
        }


        /// <summary>
        /// Convert to a base64 encoded string
        /// </summary>
        public static string Base64Encode( this string plainText )
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes( plainText );
            return System.Convert.ToBase64String( plainTextBytes );
        }

        /// <summary>
        /// Convert from a base64 encoded string
        /// </summary>
        public static string Base64Decode( this string base64EncodedData )
        {
            var base64EncodedBytes = System.Convert.FromBase64String( base64EncodedData );
            return System.Text.Encoding.UTF8.GetString( base64EncodedBytes );
        }
    }
}
