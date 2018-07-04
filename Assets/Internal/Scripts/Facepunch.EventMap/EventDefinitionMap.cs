using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;

namespace Facepunch
{
    [System.Serializable]
    public class EventDefinitionMap
    {
        public EventDefinition[] Events;

        public string editorselected;

        internal void Trigger( string name, GameObject target, bool warnOnMissing )
        {
            if ( Events == null || Events.Length == 0 )
            {
                if ( warnOnMissing )
                    Debug.Log( $"Unhandled Event \"{name}\" on {target.name}" );

                return;
            }

            bool handled = false;

            for ( int i = 0; i < Events.Length; i++ )
            {
                if ( Events[i].Name == name )
                {
                    handled = Events[i].Run( target ) || handled;
                }
            }

            if ( !handled && warnOnMissing)
                Debug.Log( $"Unhandled Event \"{name}\" on {target.name}" );
        }

        public bool HasEvent( string name )
        {
            if ( Events == null || Events.Length == 0 ) return false;

            for ( int i = 0; i < Events.Length; i++ )
            {
                if ( Events[i].Name == name )
                    return true;
            }

            return false;
        }
    }
}