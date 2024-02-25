using GMEngine.Value;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMConditionNode : GMCompositeNode, IChecker
    {
        [HideInNodeBody,SerializeField] public int[] conditions;
        [SerializeField] private GMComparer _comparer;
        public GMComparer Comparer { get
            {
                if(_comparer == null)
                {
                    Debug.LogWarning($"{name} lack of Comparer!");
                    return null;
                }
                return _comparer;
            }}

        protected override void OnStart()
        {
            VerifyCondition();
        }

        protected override void OnStop()
        {

        }

        public void VerifyCondition()
        {
            if(_comparer == null)
            {
                Debug.LogWarning($"{name} lack of comparer!");
            }

            if (conditions == null || conditions.Length != _comparer.ConditionCount)
            {
                conditions = new int[_comparer.ConditionCount];
            }
        }

        protected override ProcessStatus OnUpdate()
        {
            int conditionIndex = _comparer.CheckCondition();
            int executeNode = conditions[conditionIndex];
            if (conditionIndex == -1)
            {
                return ProcessStatus.Failure;
            }
            else
            {
                return GetChild(executeNode).Update();
            }
        }


    }
}

