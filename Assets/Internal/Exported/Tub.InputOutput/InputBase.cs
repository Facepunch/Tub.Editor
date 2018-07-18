using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[RequireComponent( typeof(Networker) )]
	public class InputBase : Facepunch.Networked, INetworkObserved
	{
	}
}
