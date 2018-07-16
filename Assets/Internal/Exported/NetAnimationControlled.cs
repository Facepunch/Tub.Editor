using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub
{
	public class NetAnimationControlled : NetAnimation, INetworkObserved, INetworkStatic
	{
	   public float Target;
	   public float Current;

	   //
	   // Methods here stubbed so you can hook them up with the Unity event stuff
	   //

	   public void SetValue( float delta )
	   {
	   }

	   public void Toggle()
	   {
	   }

	   public void SetOn()
	   {
	   }

	   public void SetOff()
	   {
	   }

	   public override void SetClip( string name )
	   {
	   }

	}
}
