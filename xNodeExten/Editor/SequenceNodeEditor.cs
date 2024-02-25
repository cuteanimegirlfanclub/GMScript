using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using System.Linq;

namespace GMEngine.GMXNode.Editor
{
    [CustomNodeEditor(typeof(SequenceGMXNode))]
    public class SequenceNodeEditor : GMXNodeEditor
    {
        public override void OnBodyGUI()
        {
            SequenceGMXNode node = (SequenceGMXNode)target;

            if (NodeEditorWindow.currentActivity == NodeEditorWindow.NodeActivity.DragNode &&
                node.children.Any(child => Selection.objects.Contains(child)))
            {
                //sort the children list according to the child's yposition
                List<GMXNode> sortedChildren = node.children.OrderBy(child => child.position.y).ToList();
                node.children = sortedChildren;
            }
            base.OnBodyGUI();
        }
    }
}

