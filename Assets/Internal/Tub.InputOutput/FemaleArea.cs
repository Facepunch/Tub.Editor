using Facepunch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    [AddComponentMenu( "Tub.IO/Female Area Socket" )]
    public class FemaleArea : Integer
    {
        [Header( "Female Socket" )]
        public string[] AcceptIdentifiers;
        public bool ShowDebugText;

        public Boolean Lock;
        public bool LockDirection;
    }



}