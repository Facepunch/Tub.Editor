using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[TriggersEvent( "checkpoint.unlock", "Checkpoint has been unlocked" )]
	public class CheckPoint : BaseCheckPoint, INetworkObserved, INetworkStatic
	{
	   public bool Unlocked;
	}
}
