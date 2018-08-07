Shader "Tub/Water"
{
	Properties
	{
		[HDR]
		_Color("Color", Color) = (1, 1, 1, 1)
		_DepthFactor("Depth Factor", float) = 1.0
		_DepthRampTex("Depth Ramp", 2D) = "white" {}
		_MainTex("Main Texture", 2D) = "white" {}
		
		_EdgeScale("Edge Scale", float) = 1
		_EdgeSpeed("Edge Speed", float) = 1
			
		_DistortStrength("Distort Strength", float) = 1.0
		_DistortDepth("Distort Depth", float) = 1.0

		_SurfaceStrength("Surface Strength", float) = 1.0
		_SurfaceDistort("Surface Distort", float) = 1.0

		_WaveSpeed("Wave Speed", float) = 1.0


		[HDR]
		_Highlight("Highlights", Color) = (1, 1, 1, 1)
			

		_BumpTex("Bump Texture", 2D) = "bump" {}
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}

		GrabPass
		{
			"_BackgroundTexture"
		}

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			sampler2D _BumpTex;
			float4 _BumpTex_ST;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _WaveSpeed;

			float _DistortStrength;
			float _DistortDepth;
			float _SurfaceStrength;
			float _SurfaceDistort;
			float _EdgeScale;
			float _EdgeSpeed;

			float4 _Highlight;

			sampler2D _BackgroundTexture;
			
			sampler2D _CameraDepthTexture;
			sampler2D _DepthRampTex;
			
			float4 _Color;
			float  _DepthFactor;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;

			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 grabPos: TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 worldPos : TEXCOORD3;
				UNITY_FOG_COORDS(4)
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.pos = UnityObjectToClipPos(input.vertex);
				output.uv = input.uv;
				output.grabPos = ComputeGrabScreenPos(output.pos);
				output.screenPos = ComputeScreenPos(output.pos);
				output.worldPos = mul(unity_ObjectToWorld, input.vertex);

				UNITY_TRANSFER_FOG(output, output.pos);
				 
				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				half2 wave1 = float2(input.worldPos.x + _Time.x * _WaveSpeed, input.worldPos.z + _Time.x * _WaveSpeed) * _BumpTex_ST.x;
				half2 wave2 = float2(input.worldPos.x - _Time.x * _WaveSpeed, input.worldPos.z - _Time.x * _WaveSpeed) * _BumpTex_ST.y;

				half3 normal = UnpackNormal( (tex2D(_BumpTex, wave1) + tex2D(_BumpTex, wave2)) / 2);

				float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
				float depth = LinearEyeDepth(depthSample).r;
				
				float4 newGrabPos = input.grabPos;

				float refractLine = saturate(_DistortDepth * (depth - input.screenPos.w));
				newGrabPos.y += normal.x * refractLine * _DistortStrength;
				newGrabPos.x += normal.y * refractLine * _DistortStrength;

				float4 depthSample2 = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, newGrabPos);
				if (LinearEyeDepth(depthSample2).r >= input.screenPos.w)
				{
					depth = LinearEyeDepth(depthSample2).r;
				}
				else
				{
					newGrabPos = input.grabPos;
				}

				float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));

				if (foamLine < 0)
					foamLine = 0;

				float4 foamRamp = tex2D(_DepthRampTex, float2(_Time.x * _EdgeSpeed + sin(input.worldPos.x - input.worldPos.z) * _EdgeScale * 0.1, foamLine));

				// sample main texture
				float4 albedo = tex2Dproj(_BackgroundTexture, newGrabPos);
				albedo.rgb = lerp(albedo.rgb, _Color.rgb, _Color.a);
				albedo.rgb = lerp(albedo, foamRamp.rgba, foamRamp.a);
				albedo.a = 1;

				albedo -= tex2D(_MainTex, wave1 * _MainTex_ST.xy + normal.xy * _SurfaceDistort * 0.1 ) * _SurfaceStrength;
				albedo += tex2D(_MainTex, wave2 * _MainTex_ST.xy + normal.xy * _SurfaceDistort * 0.1 ) * _SurfaceStrength;

				if (normal.x > _Highlight.a)
					albedo.rgb += _Highlight.rgb;

				UNITY_APPLY_FOG( input.fogCoord, albedo );

				return albedo;

			}
			ENDCG
		}
	}
}