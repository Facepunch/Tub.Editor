using Facepunch;
using UnityEngine;

public class BaseSeat : Networked, IPunObservable, INetworkTakeover
{
    public Transform SeatPoint;
    public BaseControllable Controllable;
}