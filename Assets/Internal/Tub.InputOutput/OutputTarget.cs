using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    public class OutputTarget : Networked
    {
        public virtual void OnOutputState( bool state )
        {
            Debug.LogWarning( "Unhandled OnOutputState" );
        }
    }
}