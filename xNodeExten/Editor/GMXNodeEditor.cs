using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNodeEditor;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode.Editor
{
    [CustomNodeEditor(typeof(GMXNode))]
    public class GMXNodeEditor : NodeEditor
    {
        protected GMXNode Node { get => (GMXNode)target;}
        protected GMXBehaviourTree Graph { get => Node.graph as GMXBehaviourTree; }
        protected NodeEditorWindow NodeEditorWindow { get => NodeEditorWindow.current; }
        protected UnityEngine.Event CurrentEvent { get => UnityEngine.Event.current;}
        protected GUIStyle TextStyle { get => _textStyle != null? _textStyle : _textStyle = new GUIStyle(GUI.skin.label);}
        private GUIStyle _textStyle;
        protected Vector2 NodePosition { get => NodeEditorWindow.GridToWindowPosition(Node.position);}

        //protected string StatusText { get => Node.status.ToString(); set => StatusText = $"{StatusText} {value}"; }

        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            DrawStateInformation();
            base.OnBodyGUI();
        }

        protected virtual void FindProperties()
        {

        }

        protected virtual void InitializeDrawer() 
        {
        }

        protected virtual void DrawStateInformation()
        {
            if (!Node.started)
            {
                DrawWaitingState();
                return;
            }

            ProcessStatus nodeState = Node.status;
            switch(nodeState)
            {
                case ProcessStatus.Running:
                    DrawRunningState();
                    break;
                case ProcessStatus.Success:
                    DrawSuccessState();
                    break;
                case ProcessStatus.Failure:
                    DrawFailureState();
                    break;
            }
        }

        protected virtual void DrawWaitingState()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Status:");
            TextStyle.normal.textColor = Color.cyan;
            TextStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Waiting",TextStyle);
            GUILayout.EndHorizontal();
        }

        protected virtual void DrawRunningState()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Status:");
            TextStyle.normal.textColor = Color.yellow;
            TextStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Running", TextStyle);
            GUILayout.EndHorizontal();
        }

        protected virtual void DrawSuccessState()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Status:");
            TextStyle.normal.textColor = Color.green;
            TextStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Successed", TextStyle);
            GUILayout.EndHorizontal();
        }

        protected virtual void DrawFailureState()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Status:");
            TextStyle.normal.textColor = Color.red;
            TextStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Failure", TextStyle);
            GUILayout.EndHorizontal();
        }
    }
}

