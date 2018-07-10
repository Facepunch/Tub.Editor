#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Facepunch
{
    public class EventPlaySound : EventBase
    {
        public override string Identifier => "sound.play";
        public override string DisplayName => "Play Sound";

        public override void Run( EventDefinition e, GameObject go )
        {
            if ( e.AudioSource == null ) return;

            if ( e.Objects != null && e.Objects.Length > 0 )
            {
                e.AudioSource.clip = e.Objects.Random() as AudioClip;
            }

            if ( e.Scale.x > 0 && e.Scale.y > 0 )
            {
                e.AudioSource.volume = UnityEngine.Random.Range( e.Scale.x, e.Scale.y );
            }

            if ( e.Amount.x > 0 && e.Amount.y > 0 )
            {
                e.AudioSource.pitch = UnityEngine.Random.Range( e.Amount.x, e.Amount.y );
            }

            e.AudioSource.Play();
        }

#if UNITY_EDITOR
        public override void Inspector()
        {
            ObjectField<AudioSource>( nameof( EventDefinition.Object ), "Audio Source" );
            PropertyField( nameof( EventDefinition.Objects ), "Audio Clip(s)" );

            RandomRangeVector3( nameof( EventDefinition.Scale ), "Volume", 0, 1 );
            RandomRangeVector3( nameof( EventDefinition.Amount ), "Pitch", 0, 2 );
        }
#endif
    }
}