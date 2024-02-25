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
        bool smallerThan = false;

        public override bool CheckCondition(StateMachineWrapper controller)
        {
            if(smallerThan) { return !BiggerThan(); }
            else return BiggerThan();
        }

        private bool BiggerThan()
        {
            return ConditionValue.Value >= ThersholdValue.Value;
        }
    }

}

