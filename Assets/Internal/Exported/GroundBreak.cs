using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class GroundBreak : Facepunch.Networked, INetworkObserved
	{
	   public bool ActivateOnLeave;
	   public float BreakingObjectDuration;
	   public float ReturnDelay;
	   public GameObject RegularObject;
	   public GameObject BreakingObject;
	   public GameObject BrokenObject;
	}
}
