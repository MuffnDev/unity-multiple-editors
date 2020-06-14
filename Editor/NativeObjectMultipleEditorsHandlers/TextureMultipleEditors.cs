using System;

using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for TextAsset component.
    /// </summary>
    [CustomEditor(typeof(TextureImporter))]
    [CanEditMultipleObjects]
    public class TextureMultipleEditors : NativeObjectMultipleEditorsHandler<TextureImporter>
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            ApplyHeaderOffset = true;
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnInspectorGUI()
        {
            ExtensionsManager.DrawCustomEditorsBeforeInspectorGUI();
            if (NativeEditor != null)
                NativeEditor.OnInspectorGUI();
            EditorGUILayout.EndHorizontal();
            ExtensionsManager.DrawCustomEditorsInspectorGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUIUtility.ExitGUI();
        }

    }

}