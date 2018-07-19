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
	   public bool CollectAllCollectables;
	   public bool EndGameIfNoOneAlive;
	   public bool SuccessIfAnyoneFinished;
	   public bool SuccessIfEveryoneFinished;
	}
}
