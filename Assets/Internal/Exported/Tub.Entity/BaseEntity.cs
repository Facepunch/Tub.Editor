using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseEntity : Networked
	{
	   public EntityClass Classification;
	   public bool CanTakeDamage;
	   public float Health;
	}
}
