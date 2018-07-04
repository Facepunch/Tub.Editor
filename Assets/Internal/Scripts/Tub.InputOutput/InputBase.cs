using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Facepunch;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    public class InputBase : Networked, IPunObservable
    {

    }

}