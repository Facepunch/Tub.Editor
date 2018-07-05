using Facepunch;
using System.Collections;
using UnityEngine;

[TriggersEvent( "die", "Entity has died. A good place to spawn a ragdoll, or gibs" )]
[TriggersEvent( "die.gib", "Entity has died. The damage was enough to give it. If this isn't implemented we'll call die instead." )]
public class BaseEntity : Networked, ITakeDamage, IPunObservable
{
    [InspectorFlags]
    public EntityClass Classification;

    public bool CanTakeDamage = false;
    public float Health = 100;

} 
