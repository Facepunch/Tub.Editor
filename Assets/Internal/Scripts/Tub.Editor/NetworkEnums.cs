using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public enum ViewSynchronization
    {
        Off,
        ReliableDeltaCompressed,
        Unreliable,
        UnreliableOnChange
    }

    public enum OwnershipOption
    {
        Fixed,
        Takeover,
        Request
    }
}
