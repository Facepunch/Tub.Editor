#ifndef TEXEL_DENSITY_INCLUDED
#define TEXEL_DENSITY_INCLUDED

sampler2D global_TexelDensityGrad;
float global_TexelsPerMeter;

half4 ComputeTexelDensityColor( float3 worldPos, float2 uv, float2 res )
{
	float2 ddx_uv = ddx( uv.xy );
	float2 ddy_uv = ddy( uv.xy );

	float3 ddx_wpos = ddx( worldPos.xyz );
	float3 ddy_wpos = ddy( worldPos.xyz );

	float2 scale = res * ( 0.5 / global_TexelsPerMeter.xx );
	float dx = length( ddx_uv ) / length( ddx_wpos ) * scale.x;
	float dy = length( ddy_uv ) / length( ddy_wpos ) * scale.y;
	float d = max( dx, dy );

	float3 density = tex2D( global_TexelDensityGrad, float2( d, 0.5 ) ).rgb;

	// 2x2 checker block => 16x16 texels
	bool2 test = frac( uv.xy * res / 16.0 ) < 0.5;
	float checker = test.x ? test.y : !test.y;

	float3 color = density * lerp( 0.5, 1, checker );

	return half4( color, 1 );
}

#endif // TEXEL_DENSITY_INCLUDED
