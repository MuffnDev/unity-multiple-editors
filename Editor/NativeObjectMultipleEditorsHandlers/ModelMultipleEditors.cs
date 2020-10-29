using System;

using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for MonoScript component.
    /// </summary>
    [CustomEditor(typeof(ModelImporter))]
    [CanEditMultipleObjects]
    public class ModelMultipleEditors : NativeObjectMultipleEditorsHandler<ModelImporter>
    {

        protected override void CreateNativeEditor(string _CreateEditorType = null)
        {
            NativeEditor = CreateEditor(target, Type.GetType("UnityEditor.ModelImporterEditor, UnityEditor"));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ApplyHeaderOffset = true;
        }

    }

}