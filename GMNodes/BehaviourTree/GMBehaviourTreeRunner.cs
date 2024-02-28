using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMNodes;
using Cysharp.Threading.Tasks;
using GMEngine.Value;

namespace GMEngine { 
    public class GMBehaviourTreeRunner : MonoBehaviour
    {
        public GMBehaviourTree tree;
        public FloatReferenceRO updateFrequency;
        public BooleanReferenceRO updateOnStart;


        public void Awake()
        {
            tree = (GMBehaviourTree)tree.DeepCopy();
            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        public void Start()
        {
            UpdateTree().Forget();
        }

        private async UniTaskVoid UpdateTree()
        {
            if (updateOnStart.Value)
            {
                StartUpdateTree().Forget();
            }

        }

        private async UniTaskVoid StartUpdateTree()
        {
            while (tree.status == ProcessStatus.Running)
            {
                tree.Update();
                await UniTask.WaitForSeconds(updateFrequency.Value);
            }
        }
    }
}
