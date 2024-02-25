using GMEngine.Value;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Behaviours/Value Modify Behaviour")]
    public class ValueModifyBehaviour : BehaviourSO
    {
        public FloatReferenceRW valueToModify;
        public FloatReferenceRO modifyValue;
        public FloatReferenceRO limitValue;

        public ModifyType modifyType = ModifyType.Add;

        public enum ModifyType
        {
            Add,
            Minus
        }

        public override void Execute(StateMachineWrapper controller)
        {
            if (modifyType == ModifyType.Add)
            {
                AddValue();
            }
            else if (modifyType == ModifyType.Minus)
            {
                MinusValue();
            }
        }

        public void AddValue()
        {
            if (valueToModify.Value >= limitValue.Value) { valueToModify.WriteValue(limitValue.Value); return; }
            valueToModify.WriteValue(valueToModify.Value + modifyValue.Value);
        }
        public void MinusValue()
        {
            if (valueToModify.Value <= limitValue.Value) { valueToModify.WriteValue(limitValue.Value); return; }
            valueToModify.WriteValue(valueToModify.Value - modifyValue.Value);
        }
    }

}

