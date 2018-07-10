#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Facepunch
{
    public class EventPlayParticles : EventBase
    {
        public override string Identifier => "particle.play";
        public override string DisplayName => "Particle Play";

        public override void Run( EventDefinition e, GameObject go )
        {
            if ( e.ParticleSystem == null ) return;

            e.ParticleSystem.Play( true );
        }

#if UNITY_EDITOR
        public override void Inspector()
        {
            ObjectField<ParticleSystem>( nameof( EventDefinition.Object ), "Particle System" );
        }
#endif
    }
}