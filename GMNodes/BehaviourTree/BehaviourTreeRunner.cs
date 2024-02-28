using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GMEngine.GMBTree
{
    public class BehaviourTreeRunner : GMBehaviourTreeRunner
    {

        protected override void OnAwake()
        {

        }

        public void MoveNext()
        {
            tree.Update();
        }
    }
}

