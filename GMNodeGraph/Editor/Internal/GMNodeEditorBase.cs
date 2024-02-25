using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using GMEngine.NodeGraph;
using XNodeEditor;

namespace GMEngine.GMNodes
{
    public abstract class GMNodeEditorBase<T,A,K> where T : GMNodeEditorBase<T,A,K> where A : CustomNodeEditorAttribute where K : GMNode
    {
        public SerializedObject serializedObject;
        public K target;

        private static Dictionary<K, T> editors = new Dictionary<K, T>();
        private static Dictionary<Type, Type> editorTypes;

        public GMBehaviourTreeEditorWindow window;
        public GMNodeView nodeView;

        public static T GetEditor(K target, GMNodeView nodeView)
        {
            if(target == null) { throw new NullReferenceException($"{target.GetType()} you used to get editor is null!"); }
            T editor;
            if(!editors.TryGetValue(target, out editor))
            {
                Type type = target.GetType();
                Debug.Log(type);
                Type editorType = GetEditorType(type);
                Debug.Log(editorType);
                editor = Activator.CreateInstance(editorType) as T;
                editor.window = nodeView.editorWindow;
                editor.nodeView = nodeView;
                editors.Add(target, editor);
            }
            if (editor.target == null) editor.target = target;
            if (editor.window != nodeView.editorWindow) editor.window = nodeView.editorWindow;
            if (editor.serializedObject == null) editor.serializedObject = new SerializedObject(target);

            //Debug.Log(target.name);
            //Debug.Log(editor.serializedObject);
            editor.OnCreated();
            return editor;
        }

        private static Type GetEditorType(Type type)
        {
            if (type == null) return null;
            if (editorTypes == null) CacheCustomEditors();
            Type result;
            if (editorTypes.TryGetValue(type, out result)) return result;
            return GetEditorType(type.BaseType);
        }

        private static void CacheCustomEditors()
        {
            editorTypes = new Dictionary<Type, Type>();

            Type[] nodeEditors = typeof(T).GetDerivedTypes();
            for (int i = 0; i < nodeEditors.Length; i++)
            {
                if (nodeEditors[i].IsAbstract) continue;
                object[] attribs = nodeEditors[i].GetCustomAttributes(typeof(A), false);
                if (attribs == null || attribs.Length == 0) continue;
                A attrib = attribs[0] as A;

                //Debug.Log(attrib);
                //Debug.Log(attrib.GetInspectingType());
                //Debug.Log(nodeEditors[i]);

                editorTypes.Add(attrib.GetInspectingType(), nodeEditors[i]);
            }
        }

        protected virtual void OnCreated()
        {

        }
    }

}
