using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace GMEngine.GMAddressables {
    [CustomEditor(typeof(AddressableMono))]
    public class AddressableMonoEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Address"))
            {
                GenerateAddress();
            }
        }

        private void GenerateAddress()
        {
            AddressableMono mono = (AddressableMono)target;
            if (mono.address == null || mono.address.useConstant)
            {
                mono.address.constantValue = GetInstanceID().ToString();
            }
            else
            {
                mono.address.GetVariableSO().SetValue(GetInstanceID().ToString());
            }
        }
    }
}


