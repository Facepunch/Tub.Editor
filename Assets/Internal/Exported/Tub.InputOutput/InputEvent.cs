using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[System.Serializable]
	public class InputEvent : UnityEngine.Events.UnityEvent<Tub.InputOutput.InputData>
	{
	}
}
