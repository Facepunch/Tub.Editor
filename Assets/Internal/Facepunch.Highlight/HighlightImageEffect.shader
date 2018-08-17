Shader "Facepunch/Highlight/HighlightImageEffect"
{
	Properties
	{
		_Size("Outline Size", Range( 0, 8 )) = 1.0

		[HDR] _ColorMain("Color Main", Color) = (1, 1, 1, 1)
		[HDR] _ColorOccluded("Color Occluded", Color) = (0, 0, 0, 0.5)
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 _ColorMain;
			float4 _ColorOccluded;
			sampler2D _HighlightTexture;

			float _Size;


			float4 SampleDirection(float2 uv, float2 direction)
			{
				return abs(tex2D(_HighlightTexture, uv + direction) - tex2D(_HighlightTexture, uv - direction));
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float w = _Size / _ScreenParams.x;
				float h = _Size / _ScreenParams.y;

				fixed4 c = tex2D( _HighlightTexture, i.uv );

				//
				// This is the object - don't draw over it
				//
				if ( c.r != 0 || c.g != 0 )
					return 0;

				//
				// Tweak depending on how smooth you want your lines to be
				//
				const int SampleCount = 16;

				float stageSize = 3.1415926 / SampleCount;
				float4 accum = 0;

				for (int testNum = 0; testNum < SampleCount; testNum++)
				{
					float degree = stageSize * testNum;
					accum += SampleDirection( i.uv, float2(sin(degree) * w, cos(degree) * h) );
				}

				//
				// Fuck all here
				//
				if (accum.r + accum.g == 0 )
					return 0;

				//
				// More occlusion than not
				//
				if ( accum.g > accum.r)
					return _ColorOccluded;

				return _ColorMain;
			}
			ENDCG
		}
	}
}
