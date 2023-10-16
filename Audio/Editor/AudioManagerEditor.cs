using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMEngine
{
    [CustomEditor(typeof(AudioManager), true)]
    public class AudioManagerEditor : UnityEditor.Editor
    {
        private VisualElement root { get; set; }
        private SerializedProperty propertyGlobleVolume { get; set; }
        private PropertyField globalVolumeField { get; set; }

        public override VisualElement CreateInspectorGUI()
        {
            FindSerializedProperty();
            InitializeEditor();
            Compose();
            return root;
        }

        private void FindSerializedProperty()
        {
            propertyGlobleVolume = serializedObject.FindProperty(nameof(AudioManager.globalVolume));
        }

        private void InitializeEditor()
        {
            root = new VisualElement();

            globalVolumeField = new PropertyField(propertyGlobleVolume);
        }

        private void Compose()
        {
            root.Add(new Label("Audio Setting"));
            root.Add(globalVolumeField);
        }

    }

}
