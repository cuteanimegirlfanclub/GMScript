using GMEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public abstract class Receiver : MonoBehaviour
    {
        [SerializeField]
        GameObject m_detectorPrefab;
        LightDetection m_detector;

        public bool isUnderLight;
        public float Vulnerability { get; set; } = 0.8f;
        public float receivedLightIntensity;

        protected void FixedUpdate()
        {
            LuminancedCheck();
            //Debug.Log("fixed update");
        }

        protected virtual void InitiateReceiver()
        {
            //Debug.Log("initiating receiver...");
            m_detectorPrefab = Instantiate(m_detectorPrefab, transform);
            m_detector = m_detectorPrefab.GetComponent<LightDetection>();
        }

        //need to be optimized
        public void LuminancedCheck()
        {

            if (receivedLightIntensity > 0)
            {
                isUnderLight = true;
                UnderLight(receivedLightIntensity);
            }
            else
            {
                isUnderLight=false;
                UnderDark();
            }
        }

        protected abstract void UnderLight(float lightIntensity); 
        protected abstract void UnderDark();
    }

}

