#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Facepunch
{
    public class EventLegacyAnimationPlay : EventBase
    {
        public override string Identifier => "animation.play";
        public override string DisplayName => "Legacy Animation Play";

        public override void Run( EventDefinition e, GameObject go )
        {
            if ( e.Animation == null ) return;

            e.Animation.Play( PlayMode.StopAll );
        }

#if UNITY_EDITOR
        public override void Inspector()
        {
            ObjectField<Animation>( nameof( EventDefinition.Object ), "Animation" );
          //  PropertyField( nameof( EventDefinition.StringValue ), "Trigger Name" ); // TODO - Animation Clip?
        }
#endif
    }
}