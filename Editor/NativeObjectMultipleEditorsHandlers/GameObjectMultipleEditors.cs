using UnityEngine;
using UnityEditor;

using MuffinDev.Core.EditorOnly;

using Object = UnityEngine.Object;

namespace MuffinDev.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for GameObjects.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GameObject))]
    public class GameObjectMultipleEditors : NativeObjectMultipleEditorsHandler<GameObject>
    {
        
#if UNITY_2019_1_OR_NEWER
#else
        // Defines the offset to apply after the "Root in Prefab Asset" section in the default header
        private const float ROOT_PREFAB_NOTE_OFFSET = -22f;
        // Defines the offset to apply after drawing the header when a prefab GameObject is selected
        private const float ASSET_HEADER_CONTENT_OFFSET = -7f;
#endif
        // Defines the offset to apply after drawing the header when a scene GameObject is selected
        private const float HEADER_CONTENT_OFFSET = -5f;

        protected override void OnHeaderGUI()
        {
#if UNITY_2019_1_OR_NEWER
#else
            if(target.IsAsset())
            {
                GUILayout.Space(ROOT_PREFAB_NOTE_OFFSET);
                GUIStyle header = new GUIStyle("AC BoldHeader");
                GUILayout.Box("", header, GUILayout.ExpandWidth(true));

                GUILayout.Space(ASSET_HEADER_CONTENT_OFFSET);
            }
#endif
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
            object previewCache = ReflectionUtility.GetFieldValue<object>("m_PreviewCache", _NativeEditor);

            // If the preview cache is not defined, call OnEnable() method to initialize the GameObject editor
            if (previewCache == null)
                ReflectionUtility.CallMethod("OnEnable", _NativeEditor);

            DestroyImmediate(_NativeEditor);
        }

        /// <summary>
        /// Called when the GameObject is dragged to the scene view.
        /// NOTE: This override is necessary in order to load the object correctly when it's dragged into the scene.
        /// Without this, the object cannot be dragged from the Project browser to the Scene View (dragging it to the
        /// Hierarchy works though). See UnityEditor.GameObjectInspector class from Unity CS reference for more
        /// informations: https://github.com/Unity-Technologies/UnityCsReference
        /// </summary>
        /// <param name="_SceneView">The SceneView where this GameObject is dragged to.</param>
        public void OnSceneDrag(SceneView _SceneView)
        {
            if (NativeEditor == null)
                return;

            ReflectionUtility.CallMethod("OnSceneDrag", NativeEditor, new object[] { _SceneView });
        }

        /// <summary>
        /// Called when a list of assets including the target(s) GameObject(s) is displayed.
        /// NOTE: This override is necessary in order to load the asset preview correctly when displayed in the project
        /// browser. See UnityEditor.GameObjectInspector class from Unity CS reference for more informations:
        /// https://github.com/Unity-Technologies/UnityCsReference
        /// </summary>
        public override Texture2D RenderStaticPreview(string _AssetPath, Object[] _SubAssets, int _Width, int _Height)
        {
            if (NativeEditor == null)
                return null;

            return ReflectionUtility.CallMethod<Texture2D>("RenderStaticPreview", NativeEditor, new object[] { _AssetPath, _SubAssets, _Width, _Height });
        }

        /// <summary>
        /// Draws the preview of the GameObject.
        /// NOTE: This override is necessary in order to load the asset preview correctly when displayed in the
        /// inspector. See UnityEditor.GameObjectInspector class from Unity CS reference for more informations:
        /// https://github.com/Unity-Technologies/UnityCsReference
        /// </summary>
        public override void OnPreviewGUI(Rect _Rect, GUIStyle _Background)
        {
            if (NativeEditor == null)
                return;

            ReflectionUtility.CallMethod("OnPreviewGUI", NativeEditor, new object[] { _Rect, _Background });
        }

    }

}