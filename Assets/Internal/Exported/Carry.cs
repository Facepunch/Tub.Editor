using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class Carry : BaseCarry, INetworkObserved, INetworkTakeover
	{
	   public string ObjectName;
	   public Transform LeftHand;
	   public Transform RightHand;
	}
}
