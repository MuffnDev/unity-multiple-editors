using System;

using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace MuffinDev.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for MonoScript component.
    /// </summary>
    //[CustomEditor(typeof(ModelImporter))]
    [CanEditMultipleObjects]
    public class ModelMultipleEditors : NativeObjectMultipleEditorsHandler<ModelImporter>
    {

        protected override void CreateNativeEditor(string _CreateEditorType = null)
        {
            NativeEditor = CreateEditor(target, Type.GetType("UnityEditor.ModelImporterEditor, UnityEditor"));
        }

#if UNITY_2019_1_OR_NEWER
        protected override void DestroyNativeEditor(Editor _NativeEditor)
        {
            MethodInfo enableMethod = _NativeEditor.GetType()
                .GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (enableMethod != null)
            {
                enableMethod.Invoke(_NativeEditor, null);
            }

            DestroyImmediate(_NativeEditor);
        }
#endif

        protected override void OnEnable()
        {
            base.OnEnable();
            ApplyHeaderOffset = true;
        }

    }

}