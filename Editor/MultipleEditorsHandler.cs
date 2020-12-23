using UnityEngine;
using UnityEngine.UIElements;

using MuffinDev.Core.EditorOnly;

namespace MuffinDev.MultipleEditors
{

    /// <summary>
    /// Base class for creating a custom editor that can display multiple different editors.
    /// </summary>
    /// <typeparam name="TTarget">The type of the inspected object.</typeparam>
    public abstract class MultipleEditorsHandler<TTarget> : TEditor<TTarget>
        where TTarget : Object
    {

        #region Properties

        protected CustomEditorExtensionsManager ExtensionsManager { get; private set; } = new CustomEditorExtensionsManager();
        
        // If true, use an offset after drawing the original header.
        private bool m_ApplyHeaderOffset = false;


        #endregion


        #region Lifecycle & GUI

        /// <summary>
        /// This function is called when the object is loaded.
        /// Loads the custom editors extensions, and enable them.
        /// </summary>
        protected virtual void OnEnable()
        {
            ExtensionsManager.LoadExtensions<TTarget>(this);
            ExtensionsManager.EnableCustomEditors();
        }

        /// <summary>
        /// This function is called when the scriptable object goes out of scope.
        /// Disables the loaded custom editor extensions.
        /// </summary>
        protected virtual void OnDisable()
        {
            ExtensionsManager.DisableCustomEditors();
        }

        /// <summary>
        /// Called when the header of the object being inspected is drawn.
        /// By default, calls OnBeforeHeaderGUI() on loaded custom editor extensions, draws the original header, then call OnHeaderGUI() on
        /// loaded custom editor extensions.
        /// </summary>
        protected override void OnHeaderGUI()
        {
            ExtensionsManager.DrawCustomEditorsBeforeHeaderGUI();
            DrawOriginalHeader();
            if (m_ApplyHeaderOffset)
                GUILayout.Space(MultipleEditorsUtility.COMMON_HEADER_OFFSET);
            ExtensionsManager.DrawCustomEditorsHeaderGUI();
        }

        /// <summary>
        /// Called when the inspector of the object being inspected is drawn.
        /// By default, calls OnBeforeInspectorGUI() on loaded custom editor extensions, draws the original inspector, then call
        /// OnInspectorGUI() on loaded custom editor extensions.
        /// </summary>
        public override void OnInspectorGUI()
        {
            ExtensionsManager.DrawCustomEditorsBeforeInspectorGUI();
            base.OnInspectorGUI();
            ExtensionsManager.DrawCustomEditorsInspectorGUI();
        }

        /// <summary>
        /// Called when the inspector of the object being inspected is drawn. This is an alternative to OnInspectorGUI() that allow you to
        /// use UIElements instead of only IMGUI controls. Note that these methods are not exclusive, so you can use both. In this case,
        /// CreateInspectorGUI() is called before OnInspectorGUI().
        /// </summary>
        /// <returns>Returns the root VisualElement to draw for the inspected object.</returns>
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            ExtensionsManager.DrawCustomEditorsInspectorUIElements(root);
            return root.childCount > 0 ? root : base.CreateInspectorGUI();
        }

        /// <summary>
        /// Handles scene events. Note that this message is sent by Unity only when inspecting scene objects.
        /// </summary>
        protected void OnSceneGUI()
        {
            ExtensionsManager.DrawCustomEditorsSceneGUI();
        }

        /// <summary>
        /// Defines if this Editor should be repainted constantly, (similar to an Update() for an Editor class).
        /// </summary>
        public override bool RequiresConstantRepaint()
        {
            return ExtensionsManager.RequiresConstantRepaint;
        }

        #endregion


        #region Utility Methods

        /// <summary>
        /// Draws the original header of the inspected object.
        /// </summary>
        protected void DrawOriginalHeader()
        {
            base.OnHeaderGUI();
        }

        #endregion


        #region Accessors

        /// <summary>
        /// Defines if this editor should use the header offset or not.
        /// </summary>
        public bool ApplyHeaderOffset
        {
            get { return m_ApplyHeaderOffset; }
            set { m_ApplyHeaderOffset = true; }
        }

        #endregion

    }

}