using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public static class GameObjectExtensions
    {
        public static void SetIgnoreCollisions( this GameObject self, GameObject other, bool ignore )
        {
            var a = self.GetComponentsInChildren<Collider>( true );
            var b = other.GetComponentsInChildren<Collider>( true );

            foreach ( var aa in a )
            {
                foreach ( var bb in b )
                {
                    Physics.IgnoreCollision( aa, bb, ignore );
                }
            }
        }
    }
 }