using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class SpawnPoint : Tub.BaseCheckPoint, INetworkObserved, INetworkStatic
	{
	   public bool FreeSpawn;
	}
}
