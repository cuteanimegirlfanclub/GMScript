using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using GMEngine.NodeGraph;
using System.Linq;

namespace GMEngine.GMNodes
{
    [CustomNodeEditor(typeof(GMNode))]
    public class GMNodeEditor : GMNodeEditorBase<GMNodeEditor, CustomNodeEditorAttribute, GMNode>
    {
        private InspectorElement bodyElement;
        public InspectorElement BodyElement { get { return bodyElement; } }

        private InspectorElement inspectorElement;
        public InspectorElement InspectorElement { get { return inspectorElement; } }

        protected VisualElement stateContainer;

        protected override void OnCreated()
        {
            stateContainer = nodeView.Q<VisualElement>("state");

            bodyElement = CreateBodyGUI();
            inspectorElement = CreateInspectorGUI();

            Init();
        }

        private InspectorElement CreateBodyGUI()
        {
            bodyElement = new InspectorElement(serializedObject);
            bodyElement = DoCreateInspectorElement(bodyElement);
            bodyElement.name = "NodeViewBody";
            serializedObject.Update();
            //Debug.Log($"Creating {nodeView.name} NodeBody");

            List<string> hiddenProps = serializedObject.GetPropertiesWithAttribute(typeof(HideInNodeBodyAttribute));
            hiddenProps.Add("PropertyField:m_Script");

            //hiddenProps.ForEach(prop => Debug.Log(prop));

            bodyElement.Children()
                   .First(child => child.name == "")
                   .Children()
                   .Where(child => hiddenProps.Contains(child.name))
                   .ToList()
                   .ForEach(hiddenChild => hiddenChild.RemoveFromHierarchy());


            bodyElement.styleSheets.Add(GMNodeResources.InspectorElementStyle);
            bodyElement = ModifyBodyGUI(bodyElement);
            serializedObject.ApplyModifiedProperties();
            return bodyElement;
        }

        private InspectorElement CreateInspectorGUI()
        {
            inspectorElement = new InspectorElement(serializedObject);
            inspectorElement = DoCreateInspectorElement(inspectorElement);
            inspectorElement.name = "NodeViewInspector";
            serializedObject.Update();

            inspectorElement.Children()
                   .First(child => child.name == "")
                   .Children()
                   .First(child => child.name == "PropertyField:m_Script").RemoveFromHierarchy();

            var statusProp = inspectorElement.Q<PropertyField>("PropertyField:status");
            statusProp.RegisterValueChangeCallback(DoStatusChangeEditor);

            inspectorElement.styleSheets.Add(GMNodeResources.InspectorElementStyle);
            inspectorElement.style.alignContent = Align.Stretch; 
            inspectorElement.style.alignSelf = Align.Stretch;
            inspectorElement.Q("").style.alignSelf = Align.Stretch;

            inspectorElement = ModifyInspectorGUI(inspectorElement);
            serializedObject.ApplyModifiedProperties();
            return inspectorElement;
        }

        private InspectorElement DoCreateInspectorElement(InspectorElement element)
        {
            element = CreateGUI(element);
            return element;
        }

        /// <summary>
        /// Modify Both Body and Inspector VisualElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected virtual InspectorElement CreateGUI(InspectorElement element)
        {
            return element;
        }

        /// <summary>
        /// Modify Body VisualElement
        /// </summary>
        /// <param name="bodyElement"></param>
        /// <returns></returns>
        protected virtual InspectorElement ModifyBodyGUI(InspectorElement bodyElement)
        {
            return bodyElement;
        }

        /// <summary>
        /// Modify Inpector VisualElement
        /// </summary>
        /// <param name="inspectorElement"></param>
        /// <returns></returns>
        protected virtual InspectorElement ModifyInspectorGUI(InspectorElement inspectorElement)
        {
            return inspectorElement;
        }


        public virtual void Init()
        {

        }

        private void DoStatusChangeEditor(SerializedPropertyChangeEvent evt)
        {
            ProcessStatus newState = (ProcessStatus)evt.changedProperty.enumValueIndex;
            var label = stateContainer.Q<Label>("state-declaier");
            if (!nodeView.Node.started)
            {
                label.text = "Waiting";
            }
            else
            {
                label.text = newState.ToString();
            }
            OnStatusChangeEditor(newState, label, stateContainer);
        }

        protected virtual void OnStatusChangeEditor(ProcessStatus newState, Label stateLabel, VisualElement stateContainer)
        {

        }

        public virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {

        }

        public virtual void OnInputChanged(GMNode node)
        {

        }

        public virtual void OnOutputChanged(GMNode node)
        {

        }
    }

}
