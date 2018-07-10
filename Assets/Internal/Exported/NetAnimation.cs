using UnityEngine;
using Facepunch;
using System.Collections.Generic;


public class NetAnimation : Networked, INetworkStatic
{
   public AnimationClip AnimationClip;
   public float Speed;
   public float Delta;
}
