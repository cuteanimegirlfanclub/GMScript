using GMEngine.Value;
using System;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public class ConditionGMXNode : CompositeGMXNode, IChecker
    {
        public GMComparer Comparer => _comparer;
        [SerializeField] private GMComparer _comparer;

        //public IntDeviderGroup Conditions { get => _conditions != null ? _conditions : _conditions = new IntDeviderGroup(); }
        //[SerializeField] private IntDeviderGroup _conditions;

        public int[] conditions;
        public override void OnConnectionChanged()
        {
            base.OnConnectionChanged();
            Init();
        }

        protected override void OnStart()
        {
            if (!VerifyCondition())
            {
                Debug.LogWarning($"{name}'s condition missmatch!!");
            }
        }

        protected override void OnStop()
        {

        }

        private bool VerifyCondition()
        {
            if(_comparer != null)
            {
                return children.Count <= Comparer.ConditionCount;
            }
            else
            {
                throw new NullReferenceException($"{name}'s condition not assign!");
            }
        }

        protected override ProcessStatus OnUpdate()
        {
            int conditionIndex = _comparer.CheckCondition();
            int executeNode = conditions[conditionIndex];
            if(conditionIndex == -1)
            {
                return ProcessStatus.Failure;
            }
            else
            {
                return GetChild(executeNode).Update();
            }
        }

        public override IGMNode DeepCopy(IGMBehaviourTree treeHotfix, IGMNode parentRuntime)
        {
            return base.DeepCopy(treeHotfix, parentRuntime);
        }

        protected override void Init()
        {
            base.Init();
            if (!VerifyCondition())
            {
                Debug.LogWarning($"{name}'s condition missmatch!!");
            }
            conditions = new int[Comparer.ConditionCount];
#if UNITY_EDITOR
            //Conditions.devidedValue = comparer.ConditionCount;
            childrenName.Clear();
            foreach(GMXNode child in children)
            {
                childrenName.Add(child.name);
            }

            childrenIndex = new int[children.Count];
            for(int i = 0; i < children.Count; i++)
            {
                childrenIndex[i] = i;
            }
        }

        [HideInInspector]
        public List<string> childrenName = new List<string>();
        [HideInInspector]
        public int[] childrenIndex;
#endif
    }
}

