using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[RequireComponent( typeof(Networker) )]
	public class TubLevel : Facepunch.Networked, INetworkObserved
	{
	   public MissionDef Mission;
	   [HideInInspector]
	   public Tub.CollectableInformation[] Collectibles;
	   [Header("Progress Measure")]
	   public bool CollectAllCollectables;
	   [Header("End Conditions")]
	   public bool EndGameIfNoOneAlive;
	   public bool SuccessIfAnyoneFinished;
	   public bool SuccessIfEveryoneFinished;
	}
}
