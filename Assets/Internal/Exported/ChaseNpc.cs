using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[TriggersEvent( "chase.start", "Spotted an enemy, going to chase" )]
	[TriggersEvent( "chase.celebrate", "Chase target died" )]
	[TriggersEvent( "chase.lost", "Chase target got away" )]
	[TriggersEvent( "chase.bored", "Got bored of chasing" )]
	public class ChaseNpc : Tub.BaseNpc, INetworkObserved
	{
	   [InspectorFlags]
	   public Tub.EntityClass ChaseIf;
	   public float ChaseSpeed;
	   public float WalkSpeed;
	   public float SpotDistance;
	   public float LoseDistance;
	   [Header("Home")]
	   public bool ReturnHome;
	   public Transform[] HomePositions;
	   [Header("Chasing")]
	   public float DelayBeforeChase;
	   [Header("Fail Conditions")]
	   public bool GiveUpIfCantReach;
	   public bool GetBoredOfChasing;
	   public Vector2 BoredOfChasingTime;
	}
}
