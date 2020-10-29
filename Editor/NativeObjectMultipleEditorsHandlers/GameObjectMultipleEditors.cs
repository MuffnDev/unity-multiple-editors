using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for GameObjects.
    /// </summary>
    [CustomEditor(typeof(GameObject))]
    [CanEditMultipleObjects]
    public class GameObjectMultipleEditors : NativeObjectMultipleEditorsHandler<GameObject>
    {

        // Defines the offset to apply after the "Root in Prefab Asset" section in the default header
        private const float ROOT_PREFAB_NOTE_OFFSET = -22f;
        // Defines the offset to apply after drawing the header when a prefab GameObject is selected
        private const float ASSET_HEADER_CONTENT_OFFSET = -7f;
        // Defines the offset to apply after drawing the header when a scene GameObject is selected
        private const float HEADER_CONTENT_OFFSET = -5f;

        protected override void OnHeaderGUI()
        {
            if(target.IsAsset())
            {
                GUILayout.Space(ROOT_PREFAB_NOTE_OFFSET);
                GUIStyle header = new GUIStyle("AC BoldHeader");
                GUILayout.Box("", header, GUILayout.ExpandWidth(true));

                GUILayout.Space(ASSET_HEADER_CONTENT_OFFSET);
            }

            ExtensionsManager.DrawCustomEditorsBeforeHeaderGUI();
            NativeEditor.DrawHeader();
            GUILayout.Space(HEADER_CONTENT_OFFSET);
            ExtensionsManager.DrawCustomEditorsHeaderGUI();
        }

        /// <summary>
        /// Called when this editor is disabled.
        /// Destroys the created native editor properly in order to avoid memory leaks.
        /// Enables the GameObject (ensuring its preview cache has been initialized, and so avoiding errors), then destroys it properly
        /// using Editor.DestroyImmediate().
        /// </summary>
        /// <param name="_NativeEditor">The native editor instance to destroy.</param>
        protected override void DestroyNativeEditor(Editor _NativeEditor)
        {
            // Check if the preview cache is set or not
            object previewCache = _NativeEditor.GetType()
                .GetField("m_PreviewCache", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(_NativeEditor);

            // If the preview cache is not defined, call OnEnable() method to initialize the GameObject editor
            if (previewCache == null)
            {
                MethodInfo enableMethod = _NativeEditor.GetType()
                    .GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (enableMethod != null)
                {
                    enableMethod.Invoke(_NativeEditor, null);
                }
            }

            DestroyImmediate(_NativeEditor);
        }

    }

}