using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using GMEngine.NodeGraph;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using static UnityEditor.Rendering.FilterWindow;

namespace GMEngine.GMNodes
{
    public enum DisplayTarget
    {
        NodeBody,
        Inspector
    }

    public class GMNodeView : UnityEditor.Experimental.GraphView.Node
    {
        private GMNode node;
        public GMNode Node { get => node; }

        public GMNodeEditor editor;

        public Port input;
        public Port output;

        //public VisualElement extensionContainer;

        public TextField nameField;

        public Action<GMNodeView> OnNodeSelected;

        public GMBehaviourTreeEditorWindow editorWindow;

        public GMNodeView(GMNode node) : base(GMNodeResources.NodeViewUxmlFilePath)
        {
            Initiate(node);
            CreateInputPort();
            CreateOutputPort();
            SetupClasses();
        }

        private void Initiate(GMNode node)
        {
            this.node = node;
            this.title = GMNodeGraphUtility.NameStripping(node.GetType().Name);
            this.viewDataKey = node.GUID;

            style.left = node.uiPosition.x;
            style.top = node.uiPosition.y;

            nameField = this.Q<TextField>("rename");
            nameField.RegisterCallback<KeyDownEvent>(OnEnterKeyDown);
            nameField.value = node.name;

        }

        private void CreateInputPort()
        {
            if (node is GMActionNode)
            {
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
            else if (node is GMCompositeNode)
            {
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
            else if (node is GMDecoratorNode)
            {
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
            else if (node is GMRootNode)
            {
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }

            if (input != null)
            {
                input.portName = "";
                inputContainer.Add(input);
                Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
        }

        private void CreateOutputPort()
        {
            if (node is GMActionNode)
            {
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
            else if (node is GMCompositeNode)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
            else if (node is GMDecoratorNode)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
            else if (node is GMRootNode)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                //Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }

            if (output != null)
            {
                output.portName = "";
                outputContainer.Add(output);
                Debug.Log($"Node is {node.GetType().Name}, Creating port...");
            }
        }

        public void Awake()
        {
            GenerateEditorObject();
            CreatePropertyContainer();
        }

        private void GenerateEditorObject()
        {
            editor = GMNodeEditor.GetEditor(node, this);
        }

        private void CreatePropertyContainer()
        {
            ApplyProperty(DisplayTarget.NodeBody);
        }

        public void ApplyProperty(DisplayTarget target)
        {
            switch (target)
            {
                case DisplayTarget.NodeBody:
                    extensionContainer.Add(editor.BodyElement);
                    break;
                case DisplayTarget.Inspector:
                    editorWindow.nodeInspectorView.Add(editor.InspectorElement);
                    break;
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Behaviour Tree(Set Position)");
            node.uiPosition.x = newPos.xMin;
            node.uiPosition.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();

            editorWindow.InspectingNode = this;


            if (OnNodeSelected != null)
            {
                OnNodeSelected.Invoke(this);
            }
        }

        public void OnRemove()
        {
            if(editorWindow.InspectingNode == this)
            {
                editorWindow.InspectingNode = null;
            }
        }

        private void OnEnterKeyDown(KeyDownEvent evt)
        {
            if (!nameField.visible)
            {
                Debug.Log($"{name} rename event has been misstriggered");
                return;
            }else if(evt.keyCode == KeyCode.Return)
            {
                title = nameField.text;
                Undo.RecordObject(node, $"Node Graph (Rename {node.GetType()})");
                node.name = nameField.text;
                ShowRenameField(false);
                AssetDatabase.SaveAssets();
            }
        }

        public void ShowRenameField(bool key)
        {
            Label titleLabel = this.Q<Label>("title-label");

            titleLabel.visible = !key;
            nameField.visible = key;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            editor.BuildContextualMenu(evt);
        }

        private void SetupClasses()
        {
            if (node is GMActionNode)
            {
                AddToClassList("action");
            }
            else if (node is GMCompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is GMDecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is GMRootNode)
            {
                AddToClassList("root");
            }
        }

        public void SortChildren()
        {
            GMCompositeNode composite = node as GMCompositeNode;
            if (composite)
            {
                composite.GetChildrenList().Sort(SortByVerticalPosition);
            }
        }

        private int SortByVerticalPosition(GMNode left, GMNode right)
        {
            return left.uiPosition.y < right.uiPosition.y ? -1 : 1;
        }
    }
}
