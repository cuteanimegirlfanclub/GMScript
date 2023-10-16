using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.Value;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Conditions/ValueBasedCondition")]
    public class ValueBasedContidionSO : ConditionSO
    {
        [SerializeField]
        FloatReferenceRO ConditionValue;
        [SerializeField]
        FloatReferenceRO ThersholdValue;
        [SerializeField]
        bool smallerComparision = false;

        public override bool CheckCondition(StateMachineController controller)
        {
            if(smallerComparision) { return !CheckValueCondition(); }
            else return CheckValueCondition();
        }

        private bool CheckValueCondition()
        {
            return ConditionValue.Value >= ThersholdValue.Value;
        }
    }

}

