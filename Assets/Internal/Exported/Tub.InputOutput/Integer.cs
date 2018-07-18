using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[RequireComponent( typeof(Networker) )]
	public class Integer : Tub.InputOutput.Boolean, INetworkObserved
	{
	   public int BooleanTestHigherThan;
	   public int BooleanTestLowerThan;

	   //
	   // Methods here stubbed so you can hook them up with the Unity event stuff
	   //

	   public void SetIntegerValue( int i )
	   {
	   }

	}
}
