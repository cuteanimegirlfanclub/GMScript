using GMEngine.GMXNode.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMXNode
{
    [CustomNodeEditor(typeof(StateNode))]
    public class StateNodeEditor : GMXNodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            DrawStartLine();
            DrawFinishLine();
            Measure();
        }

        private void DrawStartLine()
        {
            //throw new NotImplementedException();
        }
        private void DrawFinishLine()
        {
            //throw new NotImplementedException();
        }

        private void Measure()
        {

        }

    }

}
