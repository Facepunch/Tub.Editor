using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Facepunch.Networker ) )]
    public class BooleanTest : Integer
    {
        public Boolean[] Targets;
    }
}