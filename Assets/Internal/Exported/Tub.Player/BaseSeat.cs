using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseSeat : Facepunch.Networked, INetworkObserved, INetworkTakeover
	{
	   public Transform SeatPoint;
	   public Tub.BaseControllable Controllable;
	}
}
