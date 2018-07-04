#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Facepunch
{
    public class EventAnimatorTrigger : EventBase
    {
        public override string Identifier => "animator.trigger";
        public override string DisplayName => "Animator Trigger";

        public override void Run( EventDefinition e, GameObject go )
        {
            if ( e.Animator == null ) return;

            e.Animator.SetTrigger( e.StringValue );
        }

#if UNITY_EDITOR
        public override void Inspector()
        {
            ObjectField<Animator>( nameof( EventDefinition.Object ), "Animator" );
            PropertyField( nameof( EventDefinition.StringValue ), "Trigger Name" );
        }
#endif
    }
}