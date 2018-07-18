using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseCheckPoint : Tub.BaseEntity, INetworkObserved, INetworkStatic
	{
	   public Transform[] SpawnPoints;
	}
}
