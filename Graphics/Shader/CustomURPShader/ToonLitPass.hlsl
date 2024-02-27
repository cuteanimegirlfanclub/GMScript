#ifndef TOON_LIT_PASS_INCLUDED
#define TOON_LIT_PASS_INCLUDED

#include "ToonCommon.hlsl"
#include "ToonSurface.hlsl"
#include "ToonLight.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

//#include "ToonLighting.hlsl"

TEXTURE2D(_BaseMap);  SAMPLER(sampler_BaseMap);
TEXTURE2D(_LightRMap); SAMPLER(sampler_LightRMap);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
	UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
	UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
	UNITY_DEFINE_INSTANCED_PROP(float4, _SpecularColor)
	UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
	UNITY_DEFINE_INSTANCED_PROP(float, _Antialiasing)	
	UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
	UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
	UNITY_DEFINE_INSTANCED_PROP(float, _Glossiness)
	UNITY_DEFINE_INSTANCED_PROP(float, _Fresnel)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct Attributes {
	float3 positionOS : POSITION;
	float2 baseUV: TEXCOORD0;
	float2 rmapUV: TEXCOORD1;
	float3 normalOS: NORMAL;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings {
	float4 positionCS : SV_POSITION;
	float3 positionWS : VAR_POSITION;
	float2 baseUV: VAR_BASE_UV; //random unused identifier
	float2 rmapUV: VAR_UV;
	float3 normalWS: VAR_NORMAL;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings ToonLitPassVertex(Attributes input) 
	{
		Varyings output;
		UNITY_SETUP_INSTANCE_ID(input);
		UNITY_TRANSFER_INSTANCE_ID(input, output);

		output.positionWS = TransformObjectToWorld(input.positionOS);
		output.positionCS = TransformWorldToHClip(output.positionWS);

		output.normalWS = TransformObjectToWorldNormal(input.normalOS);

		float4 baseST = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseMap_ST);
		output.baseUV = input.baseUV * baseST.xy + baseST.zw; //sclae xy, offset zw
		return output;
	}

half4 ToonLitPassFragment(Varyings input) : SV_TARGET
	{
		UNITY_SETUP_INSTANCE_ID(input);
		float4 baseMAP = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.baseUV);
		float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
		float4 base = baseColor * baseMAP;

		#if defined(_CLIPPING)
			clip(base.a - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff));
		#endif
		
		//====surface initialization====
		ToonSurface surface;
		surface.normal = normalize(input.normalWS);
		surface.viewDir =  normalize(_WorldSpaceCameraPos - input.positionWS);

		surface.albedo = base.rgb;
		surface.alpha = base.a;
		surface.metallic = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Metallic);
		surface.smoothness = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Smoothness);

		//====lighting function====
		Light mainLight = GetMainLight();

		float NdotL = dot(surface.normal, mainLight.direction);
		float3 halfVec = normalize(mainLight.direction + surface.viewDir);
		float NdotH = saturate(dot(surface.normal, halfVec));

		float rimDot = saturate(1 - dot(surface.viewDir, surface.normal));
		float rim = rimDot * NdotL; //not to apply rim to the shaded area
		float fresnelSize = 1 - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Fresnel);
		
		//====Toon function====
		//celshading
		float delta = fwidth(NdotL) * UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Antialiasing);
		float diffuseIntensity = smoothstep(0, delta, NdotL);
		float3 diffuseColor = diffuseIntensity * mainLight.color;

		//float3 diffuseToon = SAMPLE_TEXTURE2D(_LightRMap, sampler_LightRMap, float2(diffuse * 0.5 + 0.5, 0.5));

		//specular
		float specular = pow(NdotH * diffuseIntensity, UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Glossiness));
		float specularIntensity = smoothstep(0, 0.01 * UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Antialiasing), specular);
		float3 specularColor = specularIntensity * UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _SpecularColor);
		
		//fresnel rim
		float rimIntensity = smoothstep(fresnelSize, fresnelSize * 1.1, rim);

		//rim outline


		//final output color
		float3 color = surface.albedo * (diffuseColor + rimIntensity + specularColor + unity_AmbientSky);
		return float4(color, surface.alpha);
	}

#endif