using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[RequireComponent( typeof(Networker) )]
	public class VehicleSeat : Tub.BaseSeat, INetworkObserved, INetworkTakeover
	{
	}
}
