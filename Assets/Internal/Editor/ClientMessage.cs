using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Tub
{
    public static class ClientMessage
    {
        /// <summary>
        /// Send a message to the game from the editor
        /// </summary>
        public static bool Send( string clientMessage )
        {
            var sock = new TcpClient();

            if ( !sock.ConnectAsync( "127.0.0.1", 8052 ).Wait( 50 ) )
                return false;

            using ( var stream = sock.GetStream() )
            {
                if ( stream.CanWrite )
                {
                    byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes( clientMessage );
                    stream.Write( clientMessageAsByteArray, 0, clientMessageAsByteArray.Length );
                }
            }

            sock.Close();
            return true;
        }
    }

}