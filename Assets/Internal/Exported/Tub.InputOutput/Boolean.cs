using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[RequireComponent( typeof(Networker) )]
	public class Boolean : Tub.InputOutput.InputBase, INetworkObserved
	{
	   public Tub.InputOutput.InputEvent OnBooleanTrue;
	   public Tub.InputOutput.InputEvent OnBooleanFalse;
	   public Tub.InputOutput.InputEvent OnBooleanChanged;
	   public GameObject[] ActiveOnTrue;
	   public GameObject[] ActiveOnFalse;
	   public Behaviour[] EnableOnTrue;
	   public Behaviour[] EnableOnFalse;
	   public TimeSince TimeSinceBecameTrue;
	   public TimeSince TimeSinceBecameFalse;

	   //
	   // Methods here stubbed so you can hook them up with the Unity event stuff
	   //

	   public void SetBooleanValue( bool b )
	   {
	   }

	}
}
