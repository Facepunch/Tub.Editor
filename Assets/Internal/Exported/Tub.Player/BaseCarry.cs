using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseCarry : Tub.BaseHurtable, INetworkObserved
	{
	   [Header("Holding")]
	   public CitizenHoldType HoldType;
	   public Transform LeftHandPosition;
	   public bool LeftHandIk;
	   public Transform RightHandPosition;
	   public bool RightHandIk;
	}
}
