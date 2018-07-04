#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Facepunch
{
    public class EventCreateObject : EventBase
    {
        public override string Identifier => "object.create";
        public override string DisplayName => "Create Object";

        public override void Run( EventDefinition e, GameObject go )
        {
            if ( e.Prefab == null )
                return;

            GameObject.Instantiate( e.Prefab, e.GetPosition( go.transform ), e.GetRotation( go.transform ) );
        }

#if UNITY_EDITOR

        public override void RunEditor( EventDefinition e, GameObject go )
        {
            DebugDraw.Sphere( e.GetPosition( go.transform ), 0.1f, 0.1f, true );
        }

        public override void Inspector()
        {
            PropertyField( "Prefab", "Prefab" );
            PropertyField(  "PositionType", "Position Type" );
            PropertyField(  "Position", "Position" );
            PropertyField(  "Rotation", "Rotation" );
        }
#endif
    }
}