using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[TriggersEvent( "ground.enter", "Player has entered the ground" )]
	[TriggersEvent( "ground.exit", "Player has left the ground" )]
	public class PushGround : Tub.BaseEntity, INetworkObserved
	{
	   public Vector3 Velocity;
	   public Tub.PushGroundMode Mode;
	   public bool ForceUnground;
	   public bool ActLikeJumpPressed;
	   public bool DisableDrag;
	   public bool ApplyOnGroundEnter;
	   public bool ApplyOnGroundStay;
	   public bool ApplyOnGroundExit;
	}
}
