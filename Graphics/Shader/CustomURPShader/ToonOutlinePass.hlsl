#ifndef TOON_OUTLINE_PASS_INCLUDED
#define TOON_OUTLINE_PASS_INCLUDED

#include "ToonCommon.hlsl"

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
	UNITY_DEFINE_INSTANCED_PROP(float, _OutlineSize)
	UNITY_DEFINE_INSTANCED_PROP(float4, _OutlineColor)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct Attributes {
	float3 positionOS : POSITION;
	float3 normalOS: NORMAL;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings {
	float4 positionCS : SV_POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings ToonOutlinePassVertex(Attributes input) 
	{
		Varyings output;
		UNITY_SETUP_INSTANCE_ID(input);
		UNITY_TRANSFER_INSTANCE_ID(input, output);

		float3 normal = normalize(input.normalOS) * UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _OutlineSize);
		float3 position = input.positionOS + normal;

		float3 positionWS = TransformObjectToWorld(position);
		output.positionCS = TransformWorldToHClip(positionWS);

		return output;
	}

half4 ToonOutlinePassFragment(Varyings input) : SV_TARGET
	{
		UNITY_SETUP_INSTANCE_ID(input);
		float4 color = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _OutlineColor);
		return color;
	}

#endif