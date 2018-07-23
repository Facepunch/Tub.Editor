using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[RequireComponent( typeof(Networker) )]
	public class TubLevel : Facepunch.Networked, INetworkObserved
	{
	   [Header("Information")]
	   public string UniqueIdentifier;
	   public string Title;
	   public int Version;
	   public Texture2D Icon;
	   public string Description;
	   public string Help;
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
