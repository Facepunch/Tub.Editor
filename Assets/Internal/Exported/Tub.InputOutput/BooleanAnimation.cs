using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	public class BooleanAnimation : Tub.InputOutput.Boolean, INetworkObserved
	{
	   public Tub.InputOutput.Boolean Boolean;
	   public Animation Animation;
	   public AnimationClip OnClip;
	   public AnimationClip OffClip;
	   public bool AllowInterupt;
	   public bool CurrentValue;
	}
}
