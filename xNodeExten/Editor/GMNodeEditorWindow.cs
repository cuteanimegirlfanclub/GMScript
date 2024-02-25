using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace GMEngine.GMXNode
{
    public class GMNodeEditorWindow : NodeEditorWindow
    {
        protected override void OnGUI()
        {
            base.OnGUI();
        }

        protected void OnInspectorUpdate()
        {
            if (Application.isPlaying)
            {

            }
        }
    }
}

