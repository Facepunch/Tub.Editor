using UnityEngine;
using System;
using Facepunch;

public struct DamageInfo
{
    public Vector3 SourcePosition { get; set; }
    public float Damage { get; set; }
    public Vector3 HitPosition { get; set; }
    public Vector3 Direction { get; set; }
    public DamageType DamageType { get; set; }

    public BaseEntity SourceEntity;

    public bool Gib;
}

public enum DamageType
{
    Generic,
    Explosion,
    Burn,
    Melee
}
