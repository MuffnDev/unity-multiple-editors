using UnityEngine;
using UnityEditor;

namespace MuffinDev.MultipleEditors
{

	///<summary>
	/// 
	///</summary>
	[CustomEditor(typeof(Material))]
	public class MaterialMultipleEditors : MaterialEditor
	{

        #region Properties
        
        protected CustomEditorExtensionsManager ExtensionsManager { get; private set; } = new CustomEditorExtensionsManager();

        #endregion


        #region Lifecycle & GUI

        /// <summary>
        /// This function is called when the object is loaded.
        /// Loads the custom editors extensions, and enable them.
        /// </summary>
        public override void OnEnable()
        {
            base.OnEnable();
            ExtensionsManager.LoadExtensions<Material>(this);
            ExtensionsManager.EnableCustomEditors();
        }

        /// <summary>
        /// This function is called when the scriptable object goes out of scope.
        /// Disables the loaded custom editor extensions.
        /// </summary>
        public override void OnDisable()
        {
            base.OnDisable();
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
            base.OnHeaderGUI();
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

    }

}