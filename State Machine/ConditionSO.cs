using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.Value;

namespace GMEngine
{
    public abstract class ConditionSO : ScriptableObject
    {
        public abstract bool CheckCondition(StateMachineController controller);
    }
   
}
