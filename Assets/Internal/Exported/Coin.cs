using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[TriggersEvent( "collected", "The collectable has been collected" )]
	public class Coin : Tub.BaseEntity, INetworkObserved
	{
	}
}
