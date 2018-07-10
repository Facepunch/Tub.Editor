using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseEntity : Networked, INetworkObserved
	{
	   public EntityClass Classification;
	   public bool CanTakeDamage;
	   public float Health;
	}
}
