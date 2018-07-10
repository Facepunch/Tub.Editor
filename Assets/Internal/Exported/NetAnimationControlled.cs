using UnityEngine;
using Facepunch;
using System.Collections.Generic;


public class NetAnimationControlled : NetAnimation, INetworkObserved, INetworkStatic
{
   public float Target;
   public float Current;
}
