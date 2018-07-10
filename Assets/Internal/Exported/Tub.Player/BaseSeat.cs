using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseSeat : Networked, INetworkObserved, INetworkTakeover
	{
	   public Transform SeatPoint;
	   public Tub.BaseControllable Controllable;
	}
}
