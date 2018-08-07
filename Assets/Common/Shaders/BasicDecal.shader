Shader "Facepunch/Basic Decal" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.1
		_Metallic ("Metallic", Range(0,1)) = 0.1
	}

	SubShader 
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" "ForceNoShadowCasting" = "True" }
		LOD 200
		Offset -10, -10

		CGPROGRAM
		#pragma surface surf Standard decal:blend
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			//o.Normal = 1;
			//o.Emission = 0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
