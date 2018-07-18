using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[RequireComponent( typeof(Networker) )]
	public class BooleanTest : Tub.InputOutput.Integer, INetworkObserved
	{
	   public Tub.InputOutput.Boolean[] Targets;
	}
}
