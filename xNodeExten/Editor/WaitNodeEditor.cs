using GMEngine.GMXNode.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMEngine.GMXNode
{
    [CustomNodeEditor(typeof(WaitXNode))]
    public class WaitNodeEditor : GMXNodeEditor
    {
        protected override void DrawStateInformation()
        {
            base.DrawStateInformation();
        }

        protected override void DrawWaitingState()
        {
            base.DrawWaitingState();
        }

        protected override void DrawRunningState()
        {
            base.DrawRunningState();
        }

        protected override void DrawSuccessState()
        {
            base.DrawSuccessState();
        }

        protected override void DrawFailureState()
        {
            base.DrawFailureState();
        }

    }

}
