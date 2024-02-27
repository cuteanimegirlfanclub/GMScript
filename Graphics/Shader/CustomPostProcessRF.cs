using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Collections;
using Unity.Jobs;

public class CustomPostProcessRF : ScriptableRendererFeature
{
    private CustomPostProcess1 m_custompass;
    public ScriptableObject m_lightSensitiveSO;
    public Material m_mat;
    public Camera m_camera;
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        renderer.EnqueuePass(m_custompass);
    }

    public override void Create()
    {
        //Camera.onPostRender += TrackLightReciversCallback;

        m_custompass = new CustomPostProcess1();
    }

    //public void TrackLightReciversCallback(Camera cam)
    //{
    //    return;
    //}

    public class CustomPostProcess1 : ScriptableRenderPass
    {
        Color lightIntensityCount = Color.clear;
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            foreach (VisibleLight light in renderingData.lightData.visibleLights)
            {
                Debug.Log("i am " + light.lightType + " my color is" + light.finalColor + " my name is "+ light.light.name);
                Debug.Log("there are" + renderingData.lightData.mainLightIndex + "there are" + renderingData.lightData.additionalLightsCount);
                lightIntensityCount += light.finalColor;
            }

            //foreach(GameObject gbject in renderingData.cullResults.)


            Debug.Log(lightIntensityCount);
            lightIntensityCount = Color.clear;
        }
    }
}
