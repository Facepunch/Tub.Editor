using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class CitizenNpc : Tub.BaseNpc, INetworkObserved
	{
	   public float RunSpeed;
	   public float WalkSpeed;
	   public float RunDistance;
	   public float WalkDistance;
	}
}
