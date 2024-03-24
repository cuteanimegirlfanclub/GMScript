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
        IntReferenceRO m_lightLevel;

        private void Awake()
        {
            InitiateReceiverSO(transform);
        }

        private void InitiateReceiverSO(Transform parent)
        {
            GameObject DetectorGO = Instantiate(m_detectorPrefab, parent);
            m_detector = DetectorGO.GetComponent<LightDetection>();
            //send intensityvalueSO to the detector
            m_detector.m_lightLevel.Variable = m_lightLevel.GetVariableSO() as IntVariable;
        }
    }
}

