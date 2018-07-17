using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class Hurt : MonoBehaviour
	{
	   [InspectorFlags]
	   public Tub.EntityClass DamageTarget;
	   public Tub.DamageType DamageType;
	   public float DamageAmount;
	   public Tub.DeathInfo DeathInfo;
	}
}
