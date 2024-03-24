using System.Collections;
using UnityEngine;

namespace GMEngine.Value
{
    [CreateAssetMenu(menuName = "ScriptableObject/Comparer/IntValue")]
    public class IntValueComparer : GMComparer
    {
        [SerializeField] private IntReferenceRO value;
        [SerializeField] private IntReferenceRO thresholdValue;

        public override int ConditionCount => 3;

        public override int CheckCondition()
        {
            return CompareValue();
        }

        public override string GetConditionName(int conditionIndex)
        {
            switch (conditionIndex)
            {
                case 0:
                    return "Equals";
                case 1:
                    return "Smaller";
                case 2:
                    return "Bigger";
            }
            return "MissMatch";
        }

        private int CompareValue()
        {
            if (value.Value == thresholdValue.Value)
            {
                return 0;
            }
            else if (value.Value < thresholdValue.Value)
            {
                return 1;
            }
            else if (value.Value > thresholdValue.Value)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
    }
}