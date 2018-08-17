Shader "Hidden/TexelDensityRender"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("", Float) = 0.5
		_TexelDensityGrad( "Texel Density Grad", 2D ) = "white" {}
	}

	CGINCLUDE
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "UnityBuiltin3xTreeLibrary.cginc"
		#include "TerrainEngine.cginc"
		#include "UnityInstancing.cginc"
		#include "TexelDensity.cginc"

		#pragma target 3.0
		#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON

		struct VertexOutput
		{
			float4 pos : SV_POSITION;
			float2 tex : TEXCOORD0;
			float3 posLocal : TEXCOORD1;
			float3 normWorld : TEXCOORD2;
			float4 projPos : TEXCOORD3;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		float4 _MainTex_ST;
		float _Cutoff;

		VertexOutput vertSharedBegin( appdata_full v )
		{
			UNITY_SETUP_INSTANCE_ID( v );
			VertexOutput o;
			UNITY_INITIALIZE_OUTPUT( VertexOutput, o );
			UNITY_TRANSFER_INSTANCE_ID( v, o );
			return o;
		}

		VertexOutput vertSharedEnd( VertexOutput _o, appdata_full v )
		{
			VertexOutput o = _o;
			o.pos = UnityObjectToClipPos( v.vertex );
			o.tex = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			o.posLocal = v.vertex.xyz;
			o.normWorld = UnityObjectToWorldNormal( v.normal );
			o.projPos = ComputeScreenPos( o.pos );
			return o;
		}

		VertexOutput vertStandard( appdata_full v )
		{
			VertexOutput o = vertSharedBegin( v );
			return vertSharedEnd( o, v );
		}

		float3 WorldPos( VertexOutput i )
		{
			return mul( unity_ObjectToWorld, float4( i.posLocal.xyz, 1 ) ).xyz;
		}

		half4 fragStandard( VertexOutput i ) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID( i );

			float4 color = ComputeTexelDensityColor( WorldPos( i ), i.tex, _MainTex_TexelSize.zw );

			half alpha = 1;
		#if defined( _ALPHATEST_ON ) || defined( _ALPHABLEND_ON ) || defined( _ALPHAPREMULTIPLY_ON )
			alpha = tex2D( _MainTex, i.tex.xy ).a;
			#if defined( _ALPHATEST_ON )
				clip( alpha - _Cutoff );
				alpha = 1;
			#endif
		#endif
			return half4( color.rgb, alpha );
		}
	ENDCG

	Category
	{
		Blend Off
		Cull Off
		ZTest LEqual

		SubShader
		{
			Tags { "RenderType"="Opaque" }
			Pass
			{
				CGPROGRAM
					#pragma vertex vertStandard
					#pragma fragment fragStandard
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="TransparentCutout" }
			Pass
			{
				CGPROGRAM
					#pragma vertex vertStandard
					#pragma fragment fragStandard
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="TreeBark" }
			Pass
			{
				CGPROGRAM
					#pragma vertex vertBark
					#pragma fragment fragStandard

					VertexOutput vertBark( appdata_full v )
					{
						VertexOutput o = vertSharedBegin( v );
						TreeVertBark( v );
						return vertSharedEnd( o, v );
					}
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="TreeLeaf" }
			Pass
			{
				CGPROGRAM
					#pragma vertex vertLeaf
					#pragma fragment fragStandard

					VertexOutput vertLeaf( appdata_full v )
					{
						VertexOutput o = vertSharedBegin( v );
						TreeVertLeaf( v );
						return vertSharedEnd( o, v );
					}
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="TreeOpaque" "DisableBatching"="True" }
			Pass
			{
				CGPROGRAM
					#pragma vertex vertTree
					#pragma fragment fragStandard

					VertexOutput vertTree( appdata_full v )
					{
						VertexOutput o = vertSharedBegin( v );
						TerrainAnimateTree( v.vertex, v.color.w );
						return vertSharedEnd( o, v );
					}
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="TreeTransparentCutout" "DisableBatching"="True" }
			Pass
			{
				CGPROGRAM
					#pragma vertex vertTree
					#pragma fragment fragStandard

					VertexOutput vertTree( appdata_full v )
					{
						VertexOutput o = vertSharedBegin( v );
						TerrainAnimateTree( v.vertex, v.color.w );
						return vertSharedEnd( o, v );
					}
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="TreeBillboard" }
			Pass
			{
				Cull Off
				CGPROGRAM
					#pragma vertex vertTreeBillboard
					#pragma fragment fragStandard

					VertexOutput vertTreeBillboard( appdata_full v )
					{
						VertexOutput o = vertSharedBegin( v );
						TerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);
						return vertSharedEnd( o, v );
					}
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="GrassBillboard" }
			Pass
			{
				Cull Off
				CGPROGRAM
					#pragma vertex vertGrassBillboard
					#pragma fragment fragStandard

					VertexOutput vertGrassBillboard( appdata_full v )
					{
						VertexOutput o = vertSharedBegin( v );
						WavingGrassBillboardVert( v );
						return vertSharedEnd( o, v );
					}
				ENDCG
			}
		}

		SubShader
		{
			Tags { "RenderType"="Grass" }
			Pass
			{
				Cull Off
				CGPROGRAM
					#pragma vertex vertGrass
					#pragma fragment fragStandard

					VertexOutput vertGrass( appdata_full v )
					{
						VertexOutput o = vertSharedBegin( v );
						WavingGrassVert( v );
						return vertSharedEnd( o, v );
					}
				ENDCG
			}
		}
	}

	Fallback Off
}
