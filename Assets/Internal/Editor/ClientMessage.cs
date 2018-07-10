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
        public static void Send( string clientMessage )
        {
            try
            {
                var sock = new TcpClient( "localhost", 8052 );

                using ( var stream = sock.GetStream() )
                {
                    if ( stream.CanWrite )
                    {
                        byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes( clientMessage );
                        stream.Write( clientMessageAsByteArray, 0, clientMessageAsByteArray.Length );
                    }
                }
            }
            catch ( System.Exception e )
            {
                Debug.LogError( $"Error talking to game: {e.Message}" );
            }
        }
    }

}