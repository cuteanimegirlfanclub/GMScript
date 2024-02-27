#ifndef TOON_SURFACE_INCLUDED
#define TOON_SURFACE_INCLUDED

struct ToonSurface
{
	float3 normal;
	float3 albedo;
	float alpha;

	float3 emission;
	float metallic;
	float smoothness;
	float occulusion;

	float3 viewDir;
};

#endif