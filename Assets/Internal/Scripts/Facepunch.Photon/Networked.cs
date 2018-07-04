using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public class Networked : MonoBehaviour
    {

    }

    /// <summary>
    /// Force the Networker containing this object to not network position
    /// For entities like MovePattern that move themselves
    /// </summary>
    public interface INetworkStatic
    {

    }
}