using GMEngine.Value;
using UnityEngine;

namespace GMEngine
{
    public class Battery : MonoBehaviour
    {
        public FloatReferenceRW charge;

        public void Awake()
        {
            charge.Variable.DeepCopy();
        }

        public void OnEnable()
        {
            
        }

        public void OnDisable()
        {
            
        }

        public void FixedUpdate()
        {
            
        }
    }
}

