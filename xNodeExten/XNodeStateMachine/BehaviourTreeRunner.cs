using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMXNode;
using XNode;

public class BehaviourTreeRunner : MonoBehaviour
{
    public GMXBehaviourTree behaviourTree;

    private void Start()
    {
        behaviourTree = (GMXBehaviourTree)behaviourTree.DeepCopy();
    }

    private void Update()
    {
        behaviourTree.Update();
    }
}
