using UnityEngine;
using Facepunch;
using System.Linq;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    [AddComponentMenu( "Tub.IO/Exchangable" )]
    public class Exchangeable : Boolean
    {
        public string InfluenceName;
        public float Time = 2.0f;
        public WeightedGameObjectList Destination;
    }
}