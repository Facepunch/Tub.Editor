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

        public static void DeleteAllChildren( this GameObject self )
        {
            for( int i = self.transform.childCount-1; i >= 0; i-- )
            {
                GameObject.Destroy( self.transform.GetChild( i ).gameObject );
            }
        }

        public static U GetOrAddComponent<U>( this GameObject self ) where U : Component
        {
            var c = self.GetComponent<U>();
            if ( c != null ) return c;

            return self.AddComponent<U>();
        }
    }
 }