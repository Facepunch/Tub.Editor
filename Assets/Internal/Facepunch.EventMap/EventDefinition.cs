using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;
using UnityEngine.Serialization;

namespace Facepunch
{
    [System.Serializable]
    public struct EventDefinition
    {
        public string Name;
        public string TypeName;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public Vector3 Amount;
        public string StringValue;
        public GameObject Transform;
        public PositionType PositionType;
        public GameObject Prefab;

        public UnityEngine.Object Object;
        public UnityEngine.Object[] Objects;

        public bool[] Boolean;

        public AudioSource AudioSource => Object as AudioSource;
        public ParticleSystem ParticleSystem => Object as ParticleSystem;
        public Animator Animator => Object as Animator;
        public Animation Animation => Object as Animation;

        public Vector3 GetPosition( Transform t )
        {
            switch ( PositionType )
            {
                case PositionType.Local:
                    return t.TransformPoint( Position );

                case PositionType.World:
                    return Position;

                case PositionType.LocalWithWorldRotation:
                    return t.position + Position;
            }

            throw new System.Exception( "Unhandled PositionType" );
        }

        public bool GetBool( int i, bool defValue = false )
        {
            if ( Boolean == null ) return defValue;
            if ( Boolean.Length <= i ) return defValue;

            return Boolean[i];
        }

        public Quaternion GetRotation( Transform t )
        {
            return Quaternion.Euler( Rotation );
        }

        public bool Run( GameObject go )
        {
            var e = EventFactory.Get( TypeName );
            if ( e != null )
            {
                if ( UnityEngine.Application.isPlaying )
                {
                    e.Run( this, go );
                }
                else
                {
                    #if UNITY_EDITOR
                    e.RunEditor( this, go );
                    #endif
                }
                return true;
            }

            throw new System.Exception( $"Unhandled Type \"{TypeName}\"" );
        }

    }

    public enum PositionType
    {
        Local,
        World,
        LocalWithWorldRotation
    }
}