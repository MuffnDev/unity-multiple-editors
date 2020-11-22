using UnityEngine;
using UnityEditor;

namespace MuffinDev.MultipleEditors
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