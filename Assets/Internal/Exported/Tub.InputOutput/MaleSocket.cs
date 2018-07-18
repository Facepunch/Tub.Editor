using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Tub.InputOutput
{
	[RequireComponent( typeof(Networker) )]
	public class MaleSocket : Facepunch.Networked
	{
	   public string Identifier;
	   public bool ShowDebugText;
	}
}
