using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch.Crypt
{
    public class Md5
    {
        public static string Calculate( string input )
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            return Calculate( inputBytes );
        }

        public static string Calculate( byte[] input )
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] hash = md5.ComputeHash(input);

            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < hash.Length; i++ )
            {
                sb.Append( hash[i].ToString( "x2" ) );
            }

            return sb.ToString();
        }
    }
}
