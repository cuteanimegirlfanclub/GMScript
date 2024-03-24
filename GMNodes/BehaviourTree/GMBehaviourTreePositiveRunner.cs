using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMNodes;
using Cysharp.Threading.Tasks;
using GMEngine.Value;

namespace GMEngine { 
    public class GMBehaviourTreePositiveRunner : MonoBehaviour
    {
        public GMBehaviourTree tree;
        public FloatReferenceRO updateFrequency;
        public BooleanReferenceRO updateOnStart;

        public void Awake()
        {
            tree = (GMBehaviourTree)tree.DeepCopy();
        }

        private void Start()
        {
            if (updateOnStart.Value)
            {
                StartUpdateTree().Forget();
            }
        }

        public async UniTaskVoid StartUpdateTree()
        {
            while (tree.status == ProcessStatus.Running)
            {
                tree.Update();
                await UniTask.WaitForSeconds(updateFrequency.Value);
            }
        }
    }
}
