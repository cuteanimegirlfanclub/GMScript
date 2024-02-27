#ifndef TOON_LIGHT_INCLUDED
#define TOON_LIGHT_INCLUDED

#define MAX_DIRECTIONAL_LIGHT_COUNT 4

CBUFFER_START(_ToonLight)
	float3 _MainLightColor;
	float3 _MainLightPosition;
	uint _AdditionalLightsCount;
	float3 _AdditionalLightsPositions;
	float3 _AdditionalLightsColors;
CBUFFER_END

struct Light{
	float3 color;
	float3 direction;
};

Light GetMainLight () {
	Light light;
    light.direction = _MainLightPosition.xyz;
    //light.distanceAttenuation = unity_LightData.z; // unity_LightData.z is 1 when not culled by the culling mask, otherwise 0.
    //light.shadowAttenuation = 1.0;
    light.color = _MainLightColor.rgb;
	return light;
}


uint GetAdditionalLightCount(){
	return _AdditionalLightsCount;
}

Light GetAdditionalLight(int index){
	Light light;
	//uint addLightCount = GetAdditionalLightCount();
	//light.color = _AdditionalLightsColors[index].rgb;
	//light.direction = _AdditionalLightsPositions[index].xyz;
	return light;
}


#endif