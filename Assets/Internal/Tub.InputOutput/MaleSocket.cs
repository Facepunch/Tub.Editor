using UnityEngine;
using Facepunch;
using System.Linq;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    [AddComponentMenu( "Tub.IO/Male Socket" )]
    public class MaleSocket : Networked
    {
        public string Identifier;
        public bool ShowDebugText;
    }
} 
