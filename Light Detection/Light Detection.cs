using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.Value;

namespace GMEngine
{
    public class LightDetection : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Show the light value in the log.")]
        public bool m_bLogOriginalLightValue = false;
        [Tooltip("Time between light value updates (default = 0.1f).")]
        public float m_fUpdateTime = 0.5f;

        public float lightIntensity;

        private const int c_iTextureSize = 1;

        private Texture2D m_texLight;
        private RenderTexture m_texTemp;
        private Rect m_rectLight;
        private Color m_LightPixel;
        Camera m_camLightScan;

        //private Receiver m_Receiver;

        [SerializeField]
        public FloatReferenceRW m_lightIntensity;

        /// <summary>
        /// this value needs to be updated as a globle value during GameInitiation
        /// </summary>
        private const float c_baseLightIntensity = 0.0627457f;

        private void Awake()
        {
            //m_Receiver = GetComponentInParent<Receiver>();
            m_camLightScan = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            StartLightDetection(true);
        }

        /// <summary>
        /// Prepare all needed variables and start the light detection coroutine.
        /// </summary>
        private void StartLightDetection(bool key)
        {
            if(key == false) { return; }
            m_texLight = new Texture2D(c_iTextureSize, c_iTextureSize, TextureFormat.RGB24, false);
            m_texTemp = new RenderTexture(c_iTextureSize, c_iTextureSize, 24);
            m_rectLight = new Rect(0f, 0f, c_iTextureSize, c_iTextureSize);

            StartCoroutine(LightDetectionUpdate(m_fUpdateTime));
        }

        /// <summary>
        /// Updates the light value each x seconds.
        /// </summary>
        /// <param name="_fUpdateTime">Time in seconds between updates.</param>
        /// <returns></returns>
        private IEnumerator LightDetectionUpdate(float _fUpdateTime)
        {
            while (true)
            {
                //Set the target texture of the cam.
                m_camLightScan.targetTexture = m_texTemp;
                //Render into the set target texture.
                m_camLightScan.Render();

                //Set the target texture as the active rendered texture.
                RenderTexture.active = m_texTemp;
                //Read the active rendered texture.
                m_texLight.ReadPixels(m_rectLight, 0, 0);

                //Reset the active rendered texture.
                RenderTexture.active = null;
                //Reset the target texture of the cam.
                m_camLightScan.targetTexture = null;

                //Read the pixel in middle of the texture.
                m_LightPixel = m_texLight.GetPixel(c_iTextureSize / 2, c_iTextureSize / 2);

                //Calculate light value, based on color intensity, 0.01 level capacity
                lightIntensity = 0.2126f * m_LightPixel.r + 0.7152f * m_LightPixel.g + 0.0722f * m_LightPixel.b;

                if (m_bLogOriginalLightValue)
                {
                    Debug.Log("Light Value: " + lightIntensity);
                }

                lightIntensity = IntensityMasharlling(lightIntensity);
                //m_Receiver.receivedLightIntensity = lightIntensity;
                m_lightIntensity.SetValue(lightIntensity);


                yield return new WaitForSeconds(_fUpdateTime);
            }
        }


        //marshaling intensity value, make it easy to interact with health system,based on 5 level of intensity
        private float IntensityMasharlling(float intensity)
        {
            float baseOffset = 100f;
            float marshalledIntensity = (intensity - c_baseLightIntensity) * baseOffset;

            //no need of baseOffset, just divide those value by 100f
            float level0 = 0.4f;
            float level1 = 1.1f;
            float level2 = 1.9f;
            float level3 = 2.3f;

            if (marshalledIntensity < 0) { return -1f; }
            if (marshalledIntensity - level0 < 0) { return 0.1f; }
            if (marshalledIntensity - level1 < 0) { return 0.15f; }
            if (marshalledIntensity - level2 < 0) { return 0.2f; }
            if (marshalledIntensity - level3 < 0) { return 0.5f; }
            else
                return 0.7f;
        }
    }


}
