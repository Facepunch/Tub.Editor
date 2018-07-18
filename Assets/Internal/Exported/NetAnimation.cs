using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class NetAnimation : Facepunch.Networked, INetworkStatic
	{
	   public AnimationClip AnimationClip;
	   public float Speed;
	   public float Delta;

	   //
	   // Methods here stubbed so you can hook them up with the Unity event stuff
	   //

	   public virtual void SetClip( string name )
	   {
	   }

	}
}
