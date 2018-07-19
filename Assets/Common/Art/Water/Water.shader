Shader "Tub/Water" 
{
	Properties
	{
		_Color("Color 1", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_Color3("Color 3", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Roughness( "Roughness", float ) = 0.1
		_WaveSpeed("Wave Speed", float) = 0.5
		_Metallic("Metallic", float) = 0.5
		_Smooth("Smooth", float) = 0.5
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }


		//GrabPass
	//	{
	//		"_GrabTexture"
	//		Tags{ "LightMode" = "Always" }
//		}


	//	Pass
	//	{
			CGPROGRAM
			#pragma surface surf Standard
			#pragma target 4.0

			float _Roughness;
			float _WaveSpeed;
			float _Metallic;
			float _Smooth;
			sampler2D _MainTex;
			sampler2D _BumpMap;
			fixed4 _Color;
			fixed4 _Color2;
			fixed4 _Color3;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;


			struct Input
			{
				float2 uv_MainTex;
				float4 color : COLOR;
				float3 worldPos;
				float4 screenPos;
			};


			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				o.Albedo = _Color;
				o.Metallic = _Metallic;
				o.Smoothness = _Smooth;

				float4 bump = tex2D(_BumpMap, float2(IN.worldPos.x + _Time.x * _WaveSpeed, IN.worldPos.z) / 20);
				bump += tex2D(_BumpMap, float2(IN.worldPos.z - _Time.x * _WaveSpeed, IN.worldPos.x - _SinTime.x * _WaveSpeed) / 100);

				bump /= 2;

				o.Normal = UnpackNormal(lerp(0.5, bump, _Roughness));

				if (bump.z < _Color2.b)
				{
					o.Emission = 1 * _Color2.a * 100;
				}

				

				//IN.screenPos.xy += offset * IN.screenPos.z;
			//	#if UNITY_UV_STARTS_AT_TOP
			//	fixed2 sm2Adjust = IN.screenPos.xy / IN.screenPos.w;
			//	sm2Adjust.y = 1 - sm2Adjust.y;
			//	#endif

				//fixed2 distort = o.Normal * 600 * _GrabTexture_TexelSize.xy;
				//IN.screenPos.xy += distort * IN.screenPos.z;

				//o.Normal = float3( 0, 1,  0);

				//float3 bg = tex2Dproj(_GrabTexture, IN.screenPos);

				//o.Albedo = lerp( bg, o.Albedo, _Color.a);


			}
			ENDCG
	//	}
	}
		//Fallback "VertexLit"
}