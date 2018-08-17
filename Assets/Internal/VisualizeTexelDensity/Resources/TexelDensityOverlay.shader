Shader "Hidden/TexelDensityOverlay"
{
	Properties
	{
		_MainTex ("", 2D) = "" {}
		_TexelDensityMap ( "", 2D ) = "" {}
	}
	SubShader
	{
		Pass
		{
 			ZTest Always Cull Off ZWrite Off Fog { Mode Off }
			Blend Off

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _TexelDensityMap;
				float _Opacity;

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 texcoord : TEXCOORD0;
				};

				v2f vert( appdata_t v )
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = v.texcoord.xy;
					return o;
				}

				fixed4 frag( v2f i ) : SV_Target
				{
					half4 density = tex2D( _TexelDensityMap, i.texcoord );
					half4 main = tex2D( _MainTex, i.texcoord );

					half3 col = lerp( main.rgb, density.rgb, saturate( density.a * _Opacity ) );

					return half4( col, 1 );
				}
			ENDCG

		}
	}
	Fallback Off
}
