using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tub
{

    [ExecuteInEditMode]
    public class Cable : MonoBehaviour
    {
        [Range( 0, 1 )]
        public float Slack = 0.1f;
        [Range( 0, 1 )]
        public float Swing = 1.0f;

        [Range( 0, 16 )]
        public float TextureScale = 1.0f;

        public float RuntimeTextureScale = 1.0f;

        public float Offset;

        public bool IsDirty { get; set; }

        public Transform Target;

        public LineRenderer LineRenderer;

        Vector3 oldPosA;
        Vector3 oldPosB;

        void LateUpdate()
        {
            if ( Target == null )
                return;

            if ( oldPosA != transform.position )
            {
                oldPosA = transform.position;
                IsDirty = true;
            }

            if ( oldPosB != Target.position )
            {
                oldPosB = Target.position;
                IsDirty = true;
            }

            if ( IsDirty )
            {
                UpdateLine();
                IsDirty = false;
            }
        }

        private void OnValidate()
        {
            if ( Application.isPlaying )
                return;

            IsDirty = true;
        }

        void UpdateLine()
        {
            if ( LineRenderer.positionCount < 3 )
                return;

            LineRenderer.generateLightingData = true;
            LineRenderer.textureMode = LineTextureMode.Stretch;

            LineRenderer.SetPosition( 0, transform.position );
            LineRenderer.SetPosition( LineRenderer.positionCount - 1, Target.position );

            var delta = transform.position - Target.position;

            var direction = delta;
            direction.y = 0;
            direction = direction.normalized;

            if ( Offset == 0 )
                Offset = Random.Range( 0.0f, 10.0f );

            LineRenderer.colorGradient = new Gradient
            {
                colorKeys = new GradientColorKey[]
                {
                new GradientColorKey( Color.black, 0.0f ),
                new GradientColorKey( new Color( Slack * Swing * direction.x, Slack * Swing, Slack * Swing * direction.z) , 0.5f ),
                new GradientColorKey( Color.black, 1.0f ),
                },
                mode = GradientMode.Blend
            };

            var length = 0.0f;
            var a = LineRenderer.GetPosition( 0 );
            var b = LineRenderer.GetPosition( LineRenderer.positionCount - 1 );

            for ( int i = 1; i < LineRenderer.positionCount - 1; i++ )
            {
                var f = i / (float)LineRenderer.positionCount;
                var p = Vector3.Lerp( a, b, f );

                p += Vector3.down * Mathf.Sin( f * Mathf.PI ) * Slack * 10.0f;

                LineRenderer.SetPosition( i, p );

                length += Vector3.Distance( LineRenderer.GetPosition( i - 1 ), p );
            }

            if ( TextureScale < 0.001f )
                TextureScale = 0.001f;

            if ( !Application.isPlaying )
            {
                RuntimeTextureScale = length / TextureScale;
            }

            var block = new MaterialPropertyBlock();
            block.SetFloat( "_Speed", 0.1f + Mathf.InverseLerp( 20, 0, delta.magnitude ) * 2.0f );
            block.SetFloat( "_Offset", Offset );
            block.SetVector( "_MainTex_ST", new Vector4( RuntimeTextureScale, 1.0f, 0.0f, 0.0f ) );
            LineRenderer.SetPropertyBlock( block );
        }

    }
}