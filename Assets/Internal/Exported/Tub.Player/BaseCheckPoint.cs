using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseCheckPoint : Networked
	{
	   public bool Unlocked;
	   public bool IsEntryPoint;
	   public bool FreeSpawn;
	   public Transform[] SpawnPoints;
	}
}
