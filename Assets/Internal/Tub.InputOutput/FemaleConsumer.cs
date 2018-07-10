using Facepunch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    [AddComponentMenu( "Tub.IO/Female Consumer" )]
    public class FemaleConsumer : Integer
    {
        [Header( "Female Socket" )]
        public string[] AcceptIdentifiers;

        public Boolean Lock;
        public bool LockDirection;
    }

}