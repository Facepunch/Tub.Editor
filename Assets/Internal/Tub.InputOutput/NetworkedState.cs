using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    public class NetworkedState : OutputTarget, INetworkObserved
    {
        public bool State;

        public UnityEngine.Events.UnityEvent OnTrue;
        public UnityEngine.Events.UnityEvent OnFalse;
    }
}