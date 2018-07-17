using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseCheckPoint : BaseEntity, INetworkObserved, INetworkStatic
	{
	   public Transform[] SpawnPoints;
	}
}
