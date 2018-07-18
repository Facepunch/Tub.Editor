using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[RequireComponent( typeof(Networker) )]
	public class TubLevel : Facepunch.Networked, INetworkObserved
	{
	   public MissionDef Mission;
	   public Tub.CollectibleInformation[] Collectibles;
	   public bool ShowMinimap;
	   public int MinimapSize;
	   public Texture MinimapTexture;
	   public bool CollectAllCollectables;
	   public bool EndGameIfNoOneAlive;
	   public bool SuccessIfAnyoneFinished;
	   public bool SuccessIfEveryoneFinished;
	}
}
