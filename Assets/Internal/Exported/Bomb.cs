using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[TriggersEvent( "bomb.triggered", "Bomb is live - will explode in [FuseTime]" )]
	[TriggersEvent( "bomb.explode", "Bomb explodes" )]
	public class Bomb : Tub.Carry, INetworkObserved, INetworkTakeover
	{
	   public float FuseTime;
	   public float ExplosionDamage;
	   public float ExplosionRadius;
	}
}
