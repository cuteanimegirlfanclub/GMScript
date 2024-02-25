using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes { 
    public class GMBehaviourTreeRunner : MonoBehaviour
    {
        public GMBehaviourTree tree;

        public void OnEnable()
        {
            tree = (GMBehaviourTree)tree.DeepCopy();
        }

        public void Update()
        {
            tree.Update();
        }
    }
}
