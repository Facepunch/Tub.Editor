using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[TriggersEvent( "button.press", "When the button is pressed" )]
	[TriggersEvent( "button.release", "When the button is released" )]
	[TriggersEvent( "button.toggle.on", "Button has been switched to on" )]
	[TriggersEvent( "button.toggle.off", "Button has been switched to off" )]
	public class Button : Tub.InputOutput.Boolean, INetworkObserved
	{
	   public Tub.InputOutput.ButtonPressMode Type;
	   public float RepeatTime;
	   public float DelayBeforeReset;
	   public Tub.InputOutput.Boolean EnabledIf;
	}
}
