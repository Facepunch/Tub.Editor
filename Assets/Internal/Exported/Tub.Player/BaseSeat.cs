using UnityEngine;
using Facepunch;
using System.Collections.Generic;


public class BaseSeat : Networked, INetworkObserved, INetworkTakeover
{
   public Transform SeatPoint;
   public BaseControllable Controllable;
}
