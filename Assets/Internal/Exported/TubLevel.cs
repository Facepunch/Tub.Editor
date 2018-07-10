using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class TubLevel : Networked, INetworkObserved
	{
	   public MissionDef Mission;
	   public Tub.CollectibleInformation[] Collectibles;
	   public bool ShowMinimap;
	   public int MinimapSize;
	   public Texture MinimapTexture;
	   public bool CollectAllCollectables;
	}
}
