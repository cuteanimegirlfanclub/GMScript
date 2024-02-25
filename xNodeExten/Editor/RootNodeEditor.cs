using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using XNodeEditor;

namespace GMEngine.GMXNode.Editor
{
    [CustomNodeEditor(typeof(RootNode))]
    public class RootNodeEditor : NodeEditor
    {
        public override void AddContextMenuItems(GenericMenu menu)
        {

        }

        public override void OnBodyGUI()
        {
            RootNode root = (RootNode)target;
            if (Selection.count > 1)
            {
                int id = root.GetInstanceID();
                if (Selection.Contains(id))
                {
                    var selectionList = Selection.objects.ToList();
                    selectionList.RemoveAll(obj => obj.GetInstanceID() == id);
                    Selection.objects = selectionList.ToArray();
                }
            }
            base.OnBodyGUI();
        }
    }
}

