#ifndef TOON_LIGHTING_INCLUDED
#define TOON_LIGHTING_INCLUDED

float3 IncomingToonLight (ToonSurface surface, Light light) {
	float3 NdotL = saturate(dot(surface.normal, light.direction)) * light.color;//diffuse
	//float lightIntensity = NdotL > 0? 1 : 0;
	return NdotL;
}

float3 GetLighting (ToonSurface surface, Light light) {
	return IncomingToonLight(surface, light) * surface.albedo;
}

float3 GetLighting (ToonSurface surface) {
	return GetLighting(surface, GetMainLight()); // GetMainLight() return the light struct
}

float GetToonSpecular(){
	//OnGoing...
}

#endif