using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.Value;

namespace GMEngine.GMNodes
{
    public class GMValueModifier : GMActionNode
    {
        [SerializeField] private FloatReferenceRW value;
        [SerializeField] private FloatReferenceRO modifier;
        public enum ValueModifyType
        {
            Add,
            Minus
        }

        public ValueModifyType ModifyType = ValueModifyType.Add;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override ProcessStatus OnUpdate()
        {
            switch (ModifyType)
            {
                case ValueModifyType.Add:
                    value.Value += modifier.Value;
                    return ProcessStatus.Success;
                case ValueModifyType.Minus:
                    value.Value -= modifier.Value;
                    return ProcessStatus.Success;
                default: 
                    return ProcessStatus.Failure;
            }
        }
    }
}

