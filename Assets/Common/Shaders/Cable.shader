Shader "Facepunch/Cable" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_Speed("Speed", float) = 1.0
		_Offset("Offset", float) = 1.0
	}

	SubShader 
	{

		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		half _Speed;
		half _Offset;
		fixed4 _Color;


		void vert(inout appdata_full v) 
		{
			float ang = sin( (_Time.w + _Offset) * (_Speed + _Offset * 0.01f)) * 3.14 * 0.5;

			ang += 3.14 * 3;

			v.vertex.x += sin(ang) * v.color.r * 2.0;
			v.vertex.y += cos(ang) * v.color.g * 2.0 * 0.5;
			v.vertex.z += sin(ang) * v.color.b * 2.0;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			clip(c.a - 0.2);

			// Metallic and smoothness come from slider variables
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
