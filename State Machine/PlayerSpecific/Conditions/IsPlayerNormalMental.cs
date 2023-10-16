using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine;
using GMEngine.Value;

[CreateAssetMenu(menuName = "Scriptable Object/Player/Conditions/isNormalMental")]
public class IsPlayerNormalMental : ConditionSO
{
    [SerializeField]
    FloatReferenceRO CurrentMentalPoint;
    [SerializeField]
    FloatReferenceRO PlayerTerrorThershold;
    public override bool CheckCondition(StateMachineController controller)
    {
        return CurrentMentalPoint.Value >= PlayerTerrorThershold.Value;
    }
}



