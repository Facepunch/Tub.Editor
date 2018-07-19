using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class BaseEntity : Facepunch.Networked, INetworkObserved
	{
	   [InspectorFlags]
	   public Tub.EntityClass Classification;
	}
}
