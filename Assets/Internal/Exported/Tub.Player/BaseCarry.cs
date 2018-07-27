using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseCarry : Tub.BaseHurtable, INetworkObserved
	{
	   [Header("Holding")]
	   public Tub.HoldType HoldType;
	   public Transform LocalHold;
	}
}
