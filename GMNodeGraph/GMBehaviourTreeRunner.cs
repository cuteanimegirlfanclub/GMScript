using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMNodes;

namespace GMEngine { 
    public class GMBehaviourTreeRunner : MonoBehaviour
    {
        public GMBehaviourTree tree;
        public bool start;

        public void Awake()
        {
            tree = (GMBehaviourTree)tree.DeepCopy();
        }

        public void Update()
        {
            if(start)
            {
                tree.Update();
            }
        }
    }
}
