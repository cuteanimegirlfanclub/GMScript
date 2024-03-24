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
        IntReferenceRO m_lightLevel;

        [SerializeField]
        StateMachineWrapper m_lightMachineController;

        public void InitiateReceiverSO(Transform parent)
        {
            GameObject DetectorGO = Instantiate(m_detectorPrefab, parent);
            m_detector = DetectorGO.GetComponent<LightDetection>();
            //send intensityvalueSO to the detector
            m_detector.m_lightLevel.Variable = m_lightLevel.GetVariableSO() as IntVariable;
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

