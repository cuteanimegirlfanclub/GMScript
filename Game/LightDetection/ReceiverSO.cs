using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.Value;


namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Light Receiver")]
    public class ReceiverSO : ScriptableObject
    {
        public GameObject m_detectorPrefab; 

        LightDetection m_detector;

        [SerializeField]
        FloatReferenceRO m_lightIntensity;

        [SerializeField]
        StateMachineWrapper m_lightMachineController;

        public void InitiateReceiverSO(Transform parent)
        {
            GameObject DetectorGO = Instantiate(m_detectorPrefab, parent);
            m_detector = DetectorGO.GetComponent<LightDetection>();
            //send intensityvalueSO to the detector
            m_detector.m_lightIntensity.Variable = m_lightIntensity.GetVariableSO() as FloatVariable;
        }

        public void UpdateReceiver()
        {
            m_lightMachineController.UpdateState();
        }

        public void PhysicsUpdate()
        {
            m_lightMachineController.PhysicsUpdate();
        }
    }
}

