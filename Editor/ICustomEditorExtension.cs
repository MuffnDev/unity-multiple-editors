using UnityEditor;

using UnityEngine.UIElements;

namespace MuffinDev.MultipleEditors
{

    ///<summary>
    /// Represents a custom editor extension that can be initialized from an actual Unity Editor instance.
    ///</summary>
    public interface ICustomEditorExtension
    {

        /// <summary>
        /// Called by the MultipleEditorsManager when this editor is used by an open Unity Editor instance.
        /// </summary>
        /// <param name="_Editor">The Editor instance that uses this custom editor.</param>
        void Init(Editor _Editor);

        void OnEnable();

        void OnDisable();

        void OnBeforeHeaderGUI();

        void OnHeaderGUI();

        void OnBeforeInspectorGUI();

        void OnInspectorGUI();

        VisualElement CreateInspectorGUI();

        void OnSceneGUI();

    }

}