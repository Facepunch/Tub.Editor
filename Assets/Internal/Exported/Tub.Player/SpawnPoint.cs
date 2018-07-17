using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class SpawnPoint : BaseCheckPoint, INetworkObserved, INetworkStatic
	{
	   public bool FreeSpawn;
	}
}
