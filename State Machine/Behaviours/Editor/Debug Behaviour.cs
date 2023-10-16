using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Behaviours/Debug Behaviour")]
public class DebugBehaviour : BehaviourSO
{
    public string message;
    public override void Execute(StateMachineController controller)
    {
        Debug.Log(controller.name + "says" + message);
    }

}
