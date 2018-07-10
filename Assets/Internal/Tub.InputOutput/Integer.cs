using Facepunch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    public class Integer : Boolean
    {
        public int IntegerValue { get; private set; }

        public int BooleanTestHigherThan = 0;
        public int BooleanTestLowerThan = 1000;
    }

}