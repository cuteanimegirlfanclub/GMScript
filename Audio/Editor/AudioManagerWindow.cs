using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GMEngine
{
    public class AudioManagerWindow : UnityEditor.EditorWindow
    {
        [UnityEditor.MenuItem("GMEngine/Audio Setting")]
        public static void Open()
        {
            AudioManagerWindow window = GetWindow<AudioManagerWindow>();
            window.titleContent = new UnityEngine.GUIContent("Aduio Setting");
            window.Show();
        }

        private VisualElement root => rootVisualElement;

        private void CreateGUI()
        {
            root.Clear();
            AudioManagerEditor editor = (AudioManagerEditor)UnityEditor.Editor.CreateEditor(AudioManager.Instance);
            VisualElement editorRoot = editor.CreateInspectorGUI();
            editorRoot.Bind(editor.serializedObject);
            root.Add(editorRoot);
        }
    }
}
