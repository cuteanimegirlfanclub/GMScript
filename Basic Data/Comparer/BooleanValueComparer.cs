using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Value
{
    [CreateAssetMenu(menuName = "ScriptableObject/Comparer/BooleanValue")]
    public class BooleanValueComparer : GMComparer
    {
        [SerializeField] private BooleanReferenceRO value;
        public override int ConditionCount => 2;

        public override int CheckCondition()
        {
            return CompareValue();
        }

        public override string GetConditionName(int conditionIndex)
        {
            switch(conditionIndex)
            {
                case 0:
                    return "True";
                case 1:
                    return "False";
            }
            return "MissMatch";
        }

        private int CompareValue()
        {
            if(value == null)
            {
                throw new NullReferenceException($"{name} lack of value reference!");
            }

            if(value.Value == true)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

}
