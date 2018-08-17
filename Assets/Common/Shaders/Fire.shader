Shader "Facepunch/Fire"
{
	Properties
	{
		_Noise ("Noise", 2D) = "white" {}
		_Gradient("Gradient", 2D) = "white" {}

		_Speed("Speed", float) = 1.0
		_WaveFrequency("Wave Frequency", float ) = 1.0
		_WaveSize("Wave Size", float ) = 1.0

		_CutOff("Cut Off", float) = 0.9

		[HDR]
		_BaseColor("Base Color", Color) = (1,1,1,1)
		[HDR]
		_TipColor("Tip Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		ZWrite Off
		Blend One One
		Offset -1, -1
		Cull Off

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
				float4 color: COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_noise : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float4 color: TEXCOORD2;
			};

			sampler2D _Noise;
			float4 _Noise_ST;

			sampler2D _Gradient;
			float4 _Gradient_ST;

			float4 _ColorObsured;

			float4 _BaseColor;
			float4 _TipColor;

			float _Speed;
			float _WaveFrequency;
			float _WaveSize;

			float _CutOff;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv_noise = TRANSFORM_TEX(v.uv, _Noise);
				o.color = v.color;
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 gradient = tex2D( _Gradient, i.uv );

				fixed4 col = tex2D(_Noise, i.uv_noise + float2( _WaveSize * sin( (i.color.r * 10) + _Time.z + i.uv.y * _WaveFrequency * 8 ) * 0.01, (_Time.x + i.color.g * 10) * -1 * _Speed));

				if (col.r > gradient.r * _CutOff * i.color.a)
					return 0;

				float4 output = _BaseColor;

				output += _TipColor * (i.uv.y);

				return output * i.color.a;
			}
			ENDCG
		}
	}
}
