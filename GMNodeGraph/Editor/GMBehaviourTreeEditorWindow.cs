using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using GMEngine.GMNodes;
using UnityEditor.IMGUI.Controls;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using System;

namespace GMEngine.NodeGraph
{
    public class GMBehaviourTreeEditorWindow : EditorWindow
    {
        private GMGraphView graphView;
        public GMGraphView GraphView { get => graphView;}

        private VisualElement inspectorView;
        public ScrollView nodeInspectorView;
        private GMNodeView inspectingNode;

        public GMNodeView InspectingNode
        {
            get => inspectingNode;
            set
            {
                nodeInspectorView.Clear();
                if (value == null)
                {
                    inspectingNode = null;
                    return;
                }

                Debug.Log($"Showing {value.name} To the Inspecstor");
                inspectingNode = value; 
                nodeInspectorView.Add(new IMGUIContainer(() =>
                {
                    EditorGUILayout.LabelField(value.name, EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                }));
                inspectingNode.ApplyProperty(DisplayTarget.Inspector);
            }
        }

        private ObjectField objectField;

        public static string AssetPath => "Assets/GMEngine/Scripts/GMNodeGraph/Resources";
        public static string AssetName => nameof(GMBehaviourTreeEditorWindow);

        public Action<GMNodeView> OnNodeSelected;

        [MenuItem("GMEngine/Node Editor Window")]
        public static void OpenWindow()
        {
            GMBehaviourTreeEditorWindow window = GetWindow<GMBehaviourTreeEditorWindow>();
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is GMBehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{AssetPath}/{AssetName}.uxml");
            visualTree.CloneTree(root);

            graphView = root.Q<GMGraphView>();
            graphView.editorWindow = this;

            inspectorView = root.Q<VisualElement>("InspectorView");

            objectField = inspectorView.Q<ObjectField>("TreeAssetView");
            objectField.objectType = typeof(GMBehaviourTree);
            objectField.RegisterValueChangedCallback(OnTreeObjectFieldChanged);
            if(Selection.activeObject is GMBehaviourTree tree)
            {
                objectField.value = tree;
            }

            nodeInspectorView = inspectorView.Q<ScrollView>("NodeInspectorView");
        }

        private void OnTreeObjectFieldChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            if(evt.newValue != null)
            {
                graphView.Tree = (GMBehaviourTree)evt.newValue;
                //Debug.Log($"{graphView.name} now have {graphView.Tree.name}");
            }
            else
            {
                graphView.Tree = null;
                //Debug.Log($"{graphView.name} now have no tree asset");
            }
        }


        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
        }

        private void OnSelectionChange()
        {
            GMBehaviourTree tree = Selection.activeObject as GMBehaviourTree;
            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    GMBehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<GMBehaviourTreeRunner>();
                    if (runner)
                    {
                        tree = runner.tree;
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (tree)
                {
                    graphView.Tree = tree;
                }
            }
            else if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                graphView.Tree = tree;
            }
        }

        private void OnPlayModeStateChange(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }
    }
}

