Shader "Facepunch/Highlight/HighlightMesh"
{
	Properties
	{
		_DepthBias("Depth Bias", Range( 0, 1 )) = 0.1
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD0;
			};

			sampler2D _CameraDepthTexture;
			float _DepthBias;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ (_CameraDepthTexture, i.screenPos );
				float depth = LinearEyeDepth(depthSample);
				float diff = i.screenPos.w - depth;

				if ( diff > _DepthBias )
					return fixed4(0, 1, 0, 0);

				return fixed4(1, 0, 0, 0);
			}

			ENDCG
		}
	}
}
