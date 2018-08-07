Shader "Unlit/Trigger"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ HIDE_TRIGGERS
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				half3 worldNormal : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

#ifdef HIDE_TRIGGERS
				clip(-1);
				return 0;
#else 
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				UNITY_APPLY_FOG(i.fogCoord, col);

				float light = 0.5 + dot(i.worldNormal, float3(0.3, 0.3, 0.3));
				col.rgb *= light;

				return col;
#endif
			}
			ENDCG
		}
	}
}
