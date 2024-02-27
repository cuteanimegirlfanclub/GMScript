Shader "Custom/ToonLitShader"
{
    Properties
    {
        [MainTexture] _BaseMap("Texture", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1.0, 1.0, 0.0, 1.0)

        _Antialiasing("Band Smoothing", Range(0.0, 10.0)) = 5.0

        _LightingRamp("Lighting Ramp", 2D) = "white" {}

        //================SurfaceProperty=========================
        _Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _Glossiness("Glossiness/Shininess", Range(200.0, 500.0)) = 400
        _Fresnel("Fresnel/Rim Amount", Range(0, 1)) = 0.5

        _SpecularColor("Specular Color", Color) = (1.0, 1.0, 0.0, 1.0)

        //=================AlphaCliping===========================
        _Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        [Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0


        //==================Outline===============================
        _OutlineSize("Outline Size", Range(0.0, 1.0)) = 0.716
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)

        _ID("Stencil ID", Int) = 1

        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_DstBlend ("Dst Blend", Float) = 0		
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1

    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
        }
        LOD 300

        PASS
        {

            //Stencil{
            //    Ref [_ID]
            //    Comp Always
            //    Pass Replace
            //    Fail Keep
            //    ZFail Keep
            //}

            Name "ToonLit"

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]

            Tags
            {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM
            #pragma target 2.0

			#pragma multi_compile_instancing

            #pragma shader_feature_local _CLIPPING

            #pragma vertex ToonLitPassVertex
            #pragma fragment ToonLitPassFragment
            #include "ToonLitPass.hlsl"

            ENDHLSL
        }

   //     PASS
   //     {
   //         Stencil{
   //             Ref [_ID]
   //             Comp NotEqual
   //         }

   //         Name "ToonOutline"

   //         ZWrite Off
   //         ZTest  Always

   //         Tags
   //         {
   //             "LightMode" = "SRPDefaultUnlit"
   //         }

   //         HLSLPROGRAM
   //         #pragma target 2.0

			//#pragma multi_compile_instancing

   //         #pragma vertex ToonOutlinePassVertex
   //         #pragma fragment ToonOutlinePassFragment

   //         #include "ToonOutlinePass.hlsl"

   //         ENDHLSL
   //     }
    }
}