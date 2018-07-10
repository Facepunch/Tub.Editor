using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	[System.Serializable]
	public struct DeathInfo
	{
	   public Tub.DeathType DeathType;
	   public string ReasonOthers;
	   public string ReasonSelf;
	   public bool Gib;
	}
}
