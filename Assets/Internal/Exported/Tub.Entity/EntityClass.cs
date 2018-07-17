using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[System.Flags]
	public enum EntityClass
	{
	   Player = 1,
	   HatesPlayer = 2,
	   Monster = 4,
	   HatesMonster = 8,
	   Npc = 16,
	   HatesNpc = 32,
	}
}
