using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseCarry : Tub.BaseEntity, INetworkObserved
	{
	   [Header("Holding")]
	   public AnimatorOverrideController HoldType;
	   public Transform LocalHold;
	}
}
