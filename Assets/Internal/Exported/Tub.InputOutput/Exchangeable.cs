using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[RequireComponent( typeof(Networker) )]
	public class Exchangeable : Tub.InputOutput.Boolean, INetworkObserved
	{
	   public string InfluenceName;
	   public float Time;
	   public Facepunch.WeightedGameObjectList Destination;
	}
}
