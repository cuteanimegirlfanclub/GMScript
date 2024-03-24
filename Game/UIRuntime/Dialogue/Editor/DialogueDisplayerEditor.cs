using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using GMEngine.UI;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace GMEngine.Editor.UI
{
    [CustomEditor(typeof(DialogueDisplayer))]
    public class DialogueDisplayerEditor : UnityEditor.Editor
    {
        VisualElement root {  get; set; }
        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement();
            root.AddDefaultInspector(this);
            //root.AddButton("Display");
            root.Add("Display", InternalDisplay);
            return root;
        }

        private void InternalDisplay()
        {
            DialogueDisplayer displayer = (DialogueDisplayer)target;
            if (EditorApplication.isPlaying)
            {
                displayer.DisplayDialogue(displayer.editorMessage).Forget();
            }
            else
            {
                displayer.DisplayDialogueEditor(displayer.editorMessage).Forget();
            }
        }
    }
}

