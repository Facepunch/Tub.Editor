using Facepunch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    public class Boolean : InputBase
    {
        public bool BooleanValue { get; private set; }

        public InputEvent OnBooleanTrue;
        public InputEvent OnBooleanFalse;
        public InputEvent OnBooleanChanged;

        public GameObject[] ActiveOnTrue;
        public GameObject[] ActiveOnFalse;

        public TimeSince TimeSinceBecameTrue;
        public TimeSince TimeSinceBecameFalse;
    }
}