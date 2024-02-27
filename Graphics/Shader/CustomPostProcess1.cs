using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Collections;

public class CustomPostProcess1 : ScriptableRenderPass
{
    LightData lightData;
    public Material mat;
    public ScriptableObject LightSensitiveSO;
    Color lightIntensityCount = Color.clear;
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        foreach (VisibleLight light in renderingData.lightData.visibleLights)
        {
            Debug.Log("i am " + light.lightType + " my color is" + light.finalColor);
            lightIntensityCount += light.finalColor;
        }
        Debug.Log(lightIntensityCount);
        lightIntensityCount = Color.clear;
    }
}
