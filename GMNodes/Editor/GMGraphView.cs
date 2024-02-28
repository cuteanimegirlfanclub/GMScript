using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using GMEngine.GMNodes;
using System;
using System.Linq;

namespace GMEngine.NodeGraph
{
    public class GMGraphView : GraphView
    {
        private GMBehaviourTree tree;
        public GMBehaviourTree Tree { get => tree; 
            set 
            { 
                if(value == null)
                {
                    tree = null;
                }

                tree = value;
                PopulateView(tree);
            }}

        public new class UxmlFactory : UxmlFactory<GMGraphView, GraphView.UxmlTraits> { }

        public GMBehaviourTreeEditorWindow editorWindow;

        public Action<GMNodeView> OnNodeSelected;

        public GMGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GMNodeResources.ResourcesPath}/{nameof(GMGraphView)}.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node
            ).ToList();
        }

        public void PopulateView(GMBehaviourTree tree)
        {
            if(tree == null)
            {
                Debug.Log("can find tree in graph view!");
                return;
            }

            this.tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree.RootNode == null)
            {
                tree.RootNode = tree.CreateNode(typeof(GMRootNode)) as GMRootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            //Create NodeView
            tree.nodes.ForEach(n => CreateNodeView(n, editorWindow));

            //Connect Edges According To Hierarchy
            tree.nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);
                children.ForEach(c =>
                {
                    GMNodeView parentView = FindNodeView(n);
                    GMNodeView childView = FindNodeView(c);

                    Edge edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                });
            });
        }

        public GMNodeView FindNodeView(GMNode n)
        {
            return GetNodeByGuid(n.GUID) as GMNodeView;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(n =>
                {
                    GMNodeView node = n as GMNodeView;

                    if (node != null)
                    {
                        Debug.Log($"Deleting {node.name}");
                        node.OnRemove();
                        tree.DeleteNode(node.Node);
                    }
                    Edge edge = n as Edge;
                    if (edge != null)
                    {
                        GMNodeView parentView = edge.output.node as GMNodeView;
                        GMNodeView childView = edge.input.node as GMNodeView;

                        tree.RemoveChild(parentView.Node, childView.Node);
                        parentView.editor.OnOutputChanged(childView.Node);
                        childView.editor.OnInputChanged(parentView.Node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    GMNodeView parentView = edge.output.node as GMNodeView;
                    GMNodeView childView = edge.input.node as GMNodeView;

                    tree.AddChild(parentView.Node, childView.Node);
                    parentView.editor.OnOutputChanged(childView.Node);
                    childView.editor.OnInputChanged(parentView.Node);
                });
            }

            if (graphViewChange.movedElements != null)
            {
                nodes.ForEach(n =>
                {
                    GMNodeView nodeView = n as GMNodeView;
                    nodeView.SortChildren();
                });
            }
            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {

            if (!tree)
            {
                return;
            }

            var localMousePosition = evt.mousePosition;

            if(selection.Count == 1 && selection[0] is GMNodeView nodeView)
            {
                evt.menu.AppendAction("Delete", (a) => DeleteSelection());
                evt.menu.AppendAction("Duplicate", (a) => DuplicateNode(nodeView));
                evt.menu.AppendAction("Rename", (a) => RenameNode(nodeView));
                evt.menu.AppendAction("Explore", (a) => EditorGUIUtility.PingObject(nodeView.Node));
            }

            if(selection.Count > 1)
            {
                List<GMNodeView> nodeViews = selection
                    .Where(selected => selected is GMNodeView)
                    .Select(selected => selected as GMNodeView)
                    .ToList();

                evt.menu.AppendAction("Delete", (a) => DeleteSelection());
                evt.menu.AppendAction("Duplicate", (a) => DuplicateNodes(nodeViews));
            }

            evt.menu.AppendSeparator();

            {
                var types = TypeCache.GetTypesDerivedFrom<GMActionNode>();
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"Create/[{GMNodeGraphUtility.NameStripping(t.BaseType.Name)}]{GMNodeGraphUtility.NameStripping(t.Name)}", 
                        (a) => CreateNode(t,localMousePosition));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<GMCompositeNode>();
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"Create/[{GMNodeGraphUtility.NameStripping(t.BaseType.Name)}]{GMNodeGraphUtility.NameStripping(t.Name)}", 
                        (a) => CreateNode(t,localMousePosition));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<GMDecoratorNode>();
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"Create/[{GMNodeGraphUtility.NameStripping(t.BaseType.Name)}]{GMNodeGraphUtility.NameStripping(t.Name)}", 
                        (a) => CreateNode(t,localMousePosition));
                }
            }
        }


        private GMNodeView CreateNode(System.Type type)
        {
            GMNode node = CreateNodeInternal(type);
            return CreateNodeView(node, editorWindow);
        }

        private GMNodeView CreateNode(Type type, Vector2 position)
        {
            GMNode node = CreateNodeInternal(type);
            return CreateNodeView(node, editorWindow, position);
        }

        private GMNode CreateNodeInternal(Type type)
        {
            GMNode node = tree.CreateNode(type);
            node.name = GMNodeGraphUtility.NameStripping(node.name).GetUniqueNodeName(tree.nodes);
            return node;
        }

        private GMNodeView CreateNodeView(GMNode node, GMBehaviourTreeEditorWindow editorWindow)
        {
            return CreateNodeView(node, editorWindow, node.uiPosition);
        }

        private GMNodeView CreateNodeView(GMNode node, GMBehaviourTreeEditorWindow editorWindow, Vector2 position)
        {
            GMNodeView nodeView = new GMNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.name = node.name;
            nodeView.title = node.name;
            nodeView.editorWindow = editorWindow;
            Debug.Log($"{nodeView.name} Created");
            nodeView.SetPosition(new Rect(position, nodeView.GetPosition().size));
            Debug.Log($"{nodeView.name} Awake");
            nodeView.Awake();
            AddElement(nodeView);
            return nodeView;
        }

        private void DuplicateNode(GMNodeView nodeView)
        {
            GMNodeView duplicatedNode = CreateNode(nodeView.Node.GetType());
            duplicatedNode.SetPosition(new Rect(nodeView.GetPosition().position + new Vector2(20, 20), nodeView.GetPosition().size));
        }

        private void DuplicateNodes(List<GMNodeView> nodeViews)
        {
            nodeViews.ForEach(n => DuplicateNode(n));
        }

        private void RenameNode(GMNodeView nodeView)
        {
            nodeView.ShowRenameField(true);
        }
    }
}

