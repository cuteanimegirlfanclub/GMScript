using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public class LightReceiver : MonoBehaviour
    {
        public GameObject m_detectorPrefab;

        LightDetection m_detector;

        [SerializeField]
        FloatReferenceRO m_lightIntensity;

        private void Awake()
        {
            InitiateReceiverSO(transform);
        }

        private void InitiateReceiverSO(Transform parent)
        {
            GameObject DetectorGO = Instantiate(m_detectorPrefab, parent);
            m_detector = DetectorGO.GetComponent<LightDetection>();
            //send intensityvalueSO to the detector
            m_detector.m_lightIntensity.Variable = m_lightIntensity.GetVariableSO() as FloatVariable;
        }
    }
}

