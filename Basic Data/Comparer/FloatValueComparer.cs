using GMEngine.Value;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GMEngine.Value
{
    public interface IChecker
    {
        public GMComparer Comparer { get; }
    }

    public interface ICondition
    {
        public int CheckCondition();
    }

    public abstract class GMComparer : ScriptableObject, ICondition
    {
        /// <summary>
        /// -1 means no condition match
        /// </summary>
        /// <returns></returns>
        public abstract int CheckCondition();
        public abstract int ConditionCount { get; }
#if UNITY_EDITOR
        public abstract string GetConditionName(int conditionIndex);

#endif
    }

    [CreateAssetMenu(menuName = "ScriptableObject/Comparer/FloatValue")]
    public class FloatValueComparer : GMComparer
    {
        [SerializeField] private FloatReferenceRO value;
        [SerializeField] private FloatReferenceRO thresholdValue;

        public override int ConditionCount => 3;

        public override int CheckCondition()
        {
            return CompareValue();
        }

        public override string GetConditionName(int conditionIndex)
        {
            switch(conditionIndex)
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
            if(value.Value == thresholdValue.Value)
            {
                return 0;
            }else if(value.Value < thresholdValue.Value)
            {
                return 1;
            }else if(value.Value > thresholdValue.Value)
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

