using System;
using Facepunch;
using Tub.InputOutput;
using UnityEngine;

public class BaseCarry : BaseEntity
{
    [Header( "Holding" )]
    public AnimatorOverrideController HoldType;
    public Transform LocalHold;
}
