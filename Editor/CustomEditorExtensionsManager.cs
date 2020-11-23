using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    ///<summary>
    /// Utility class for loading custom editor extensions and use them.
    ///</summary>
    [System.Serializable]
	public class CustomEditorExtensionsManager
    {

        public ICustomEditorExtension[] LoadedExtensions { get; private set; } = null;

        public bool RequiresConstantRepaint { get; private set; } = false;

        /// <summary>
        /// Loads the custom editor extensions that targte the given type.
        /// </summary>
        /// <typeparam name="TTarget">The type to target.</typeparam>
        /// <param name="_Editor">The calling Editor instance.</param>
        public void LoadExtensions<TTarget>(Editor _Editor)
            where TTarget : Object
        {
            LoadedExtensions = MultipleEditorsManager.CreateEditors<TTarget>(_Editor, out bool requiresConstantRepaint);
            RequiresConstantRepaint = requiresConstantRepaint;
        }

        /// <summary>
        /// Calls OnEnable() on each loaded custom editor extensions.
        /// </summary>
        public void EnableCustomEditors()
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                customObjectEditor.OnEnable();
            }
        }

        /// <summary>
        /// Calls OnDisable() on each loaded custom editor extensions.
        /// </summary>
        public void DisableCustomEditors()
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                customObjectEditor.OnDisable();
            }
        }

        /// <summary>
        /// Calls OnBeforeHeaderGUI() on each loaded custom editor extensions.
        /// </summary>
        public void DrawCustomEditorsBeforeHeaderGUI()
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                customObjectEditor.OnBeforeHeaderGUI();
            }
        }

        /// <summary>
        /// Calls OnHeaderGUI() on each loaded custom editor extensions.
        /// </summary>
        public void DrawCustomEditorsHeaderGUI()
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                customObjectEditor.OnHeaderGUI();
            }
        }

        /// <summary>
        /// Calls OnBeforeInspectorGUI() on each loaded custom editor extensions.
        /// </summary>
        public void DrawCustomEditorsBeforeInspectorGUI()
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                customObjectEditor.OnBeforeInspectorGUI();
            }
        }

        /// <summary>
        /// Calls OnInspectorGUI() on each loaded custom editor extensions.
        /// </summary>
        public void DrawCustomEditorsInspectorGUI()
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                customObjectEditor.OnInspectorGUI();
            }
        }

        /// <summary>
        /// Calls CreateInspectorGUI() on each loaded custom editor extensions.
        /// Note that each VisualElement returned by the custom editor implementations are added to the root panel.
        /// </summary>
        /// <param name="_Root">The root VisualElement, which is the main panel that will be drawn in the inspector.</param>
        public void DrawCustomEditorsInspectorUIElements(VisualElement _Root)
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                VisualElement ui = customObjectEditor.CreateInspectorGUI();
                if (ui != null && ui.childCount > 0)
                    _Root.Add(ui);
            }
        }

        /// <summary>
        /// Calls OnSceneGUI() on each loaded custom editor extensions.
        /// </summary>
        public void DrawCustomEditorsSceneGUI()
        {
            foreach (ICustomEditorExtension customObjectEditor in LoadedExtensions)
            {
                customObjectEditor.OnSceneGUI();
            }
        }

    }

}