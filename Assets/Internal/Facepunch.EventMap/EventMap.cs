using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Facepunch
{
    public class EventMap : MonoBehaviour
    {
        public EventDefinitionMap Map = new EventDefinitionMap();

        [Obsolete]
        public void TriggerAnimationEvent( string name ) => TriggerEvent( name );

        public void TriggerEvent( string name )
        {
            Map.Trigger( name, gameObject, true );
        }

        public bool HasEvent( string name ) => Map.HasEvent( name );
    }
}