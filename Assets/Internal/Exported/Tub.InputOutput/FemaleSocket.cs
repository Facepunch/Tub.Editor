using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[RequireComponent( typeof(Networker) )]
	public class FemaleSocket : Tub.InputOutput.Boolean, INetworkObserved
	{
	   public bool CanMountAnything;
	   public System.String[] MountableIdentifiers;
	   public System.String[] AcceptableIdentifiers;
	   public Transform MountPoint;
	   public System.String[] InfluenceTags;
	   public float InfluenceSpeedMultiplier;
	   public Tub.InputOutput.Boolean Lock;
	   public bool LockDirection;
	   public Tub.InputOutput.MaleSocket CurrentMale;
	}
}
