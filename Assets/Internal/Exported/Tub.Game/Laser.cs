using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;

namespace Tub
{
    [ExecuteInEditMode]
    public class Laser : MonoBehaviour
    {
        public LineRenderer LineRenderer;
        public LayerMask LayerMask;
        public GameObject EndPoint;

        [Header( "Damage" )]
        public DeathInfo DeathInfo = new DeathInfo
        {
            DeathType = DeathType.Burnt,
            ReasonSelf = "You were lasered",
            ReasonOthers = "was lasered"
        };
        public DamageType DamageType;
        public float DamageAmount = 1000.0f;

        [Header( "Line Shape" )]
        public float RandomWiggle = 0.0f;
        public Vector3 SinWave;
        public Vector3 CosWave;

        void Update()
        {
            var delta = transform.forward * 4096;

            RaycastHit hit;
            if ( Physics.Raycast( new Ray( transform.position, delta ), out hit, delta.magnitude, LayerMask, QueryTriggerInteraction.Ignore ) )
            {
                UpdateEndPoint( hit.point, hit.normal );

#if !TUB_EDITOR
                if ( UnityEngine.Application.isPlaying )
                {
                    var damage = hit.collider.GetComponentInParent<BaseHurtable>();

                    if ( damage != null && damage.IsMine )
                    {
                        var di = new DamageInfo
                        {
                            DamageType = DamageType,
                            Damage = DamageAmount * Time.timeScale,
                            SourcePosition = transform.position,
                            Direction = delta,
                            Gib = DeathInfo.Gib
                        };

                        damage.TakeDamage( di );
                    }
                }
#endif
            }
            else
            {
                UpdateEndPoint( transform.position + delta, delta.normalized * -1 );
            }
        }

        void UpdateEndPoint( Vector3 point, Vector3 normal )
        {
            if ( LineRenderer != null && LineRenderer.positionCount > 1 )
            {
                var lastPos = LineRenderer.positionCount - 1;

                LineRenderer.SetPosition( 0, transform.position );
                LineRenderer.SetPosition( lastPos, point );

                UpdateLineMiddle();
            }

            DebugDraw.Sphere( point, Color.red, 0.1f, 0, true );

            if ( EndPoint != null )
            {
                EndPoint.transform.SetPositionAndRotation( point, Quaternion.LookRotation( Vector3.Reflect( transform.forward, normal ), transform.up ) );
            }
        }


        void UpdateLineMiddle()
        {
            if ( LineRenderer.positionCount < 3 )
                return;

            var a = LineRenderer.GetPosition( 0 );
            var b = LineRenderer.GetPosition( LineRenderer.positionCount - 1 );

            for ( int i = 1; i < LineRenderer.positionCount - 1; i++ )
            {
                var f = i / (float)LineRenderer.positionCount;
                var p = Vector3.Lerp( a, b, f );

                if ( RandomWiggle != 0 )
                {
                    p += Random.insideUnitSphere * RandomWiggle;
                }

                p += transform.right * Mathf.Sin( (Time.time * SinWave.x) + i * SinWave.z ) * SinWave.y;
                p += transform.up * Mathf.Cos( (Time.time * CosWave.x) + i * CosWave.z ) * CosWave.y;

                LineRenderer.SetPosition( i, p );
            }
        }
    }
}