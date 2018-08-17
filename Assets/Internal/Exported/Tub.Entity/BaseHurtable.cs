using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[TriggersEvent( "die", "Entity has died. A good place to spawn a ragdoll, or gibs" )]
	[TriggersEvent( "die.gib", "Entity has died. The damage was enough to give it. If this isn't implemented we'll call die instead." )]
	[TriggersEvent( "takedamage", "Entity has taken damage" )]
	public class BaseHurtable : Tub.BaseEntity, INetworkObserved
	{
	   [Header("Hurtable")]
	   public bool CanTakeDamage;
	   public float Health;
	}
}
