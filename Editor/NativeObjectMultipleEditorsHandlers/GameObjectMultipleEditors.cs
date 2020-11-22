using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace MuffinDev.EditorUtils.MultipleEditors
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
            if (!ReflectionUtility.CallMethod<bool>("HasStaticPreview", NativeEditor) || !ShaderUtil.hardwareSupportsRectRenderTexture)
                return null;
            object previewData = ReflectionUtility.CallMethod<object>("GetPreviewData", NativeEditor);
            PreviewRenderUtility renderUtility = (PreviewRenderUtility)ReflectionUtility.GetNestedType("PreviewData", NativeEditor)
                .GetField("renderUtility").GetValue(previewData);
            renderUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) _Width, (float) _Height));
            ReflectionUtility.CallMethod("DoRenderPreview", NativeEditor);
            return renderUtility.EndStaticPreview();
        }
        
        /// <summary>
        /// Draws the preview of the GameObject.
        /// NOTE: This override is necessary in order to load the asset preview correctly when displayed in the
        /// inspector. See UnityEditor.GameObjectInspector class from Unity CS reference for more informations:
        /// https://github.com/Unity-Technologies/UnityCsReference
        /// </summary>
        public override void OnPreviewGUI(Rect _Rect, GUIStyle _Background)
        {
            if (!ShaderUtil.hardwareSupportsRectRenderTexture)
            {
                if (Event.current.type != UnityEngine.EventType.Repaint)
                    return;
                EditorGUI.DropShadowLabel(new Rect(_Rect.x, _Rect.y, _Rect.width, 40f), "Preview requires\nrender texture support");
            }
            else
            {
                Vector2 vector2 = (Vector2)Type.GetType("PreviewGUI, UnityEditor")
                    .GetMethod("Drag2D", ReflectionUtility.STATIC)
                    .Invoke(null, new object[] { ReflectionUtility.GetFieldValue("m_PreviewDir", NativeEditor), _Rect });
                
                if (vector2 != ReflectionUtility.GetFieldValue<Vector2>("m_PreviewDir", NativeEditor))
                {
                    ReflectionUtility.CallMethod("ClearPreviewCache", NativeEditor);
                    ReflectionUtility.SetFieldValue("m_PreviewDir", vector2, NativeEditor);
                }

                if (Event.current.type != UnityEngine.EventType.Repaint)
                    return;

                if (ReflectionUtility.GetFieldValue<Rect>("m_PreviewRect", NativeEditor) != _Rect)
                {
                    ReflectionUtility.CallMethod("ClearPreviewCache", NativeEditor);
                    ReflectionUtility.SetFieldValue("m_PreviewRect", _Rect, NativeEditor);
                }
                
                object previewData = ReflectionUtility.CallMethod<object>("GetPreviewData", NativeEditor);
                PreviewRenderUtility renderUtility = (PreviewRenderUtility) ReflectionUtility.GetNestedType("PreviewData", NativeEditor).GetField("renderUtility").GetValue(previewData);

                Dictionary<int, Texture> previewCache = ReflectionUtility.GetFieldValue<Dictionary<int, Texture>>("m_PreviewCache", NativeEditor);
                int referenceTargetIndex = ReflectionUtility.GetPropertyValue<int>("referenceTargetIndex", NativeEditor);
                if (previewCache.TryGetValue(referenceTargetIndex, out Texture texture))
                {
                    typeof(PreviewRenderUtility).GetMethod("DrawPreview", ReflectionUtility.STATIC)
                        .Invoke(null, new object[] { _Rect, texture });
                }
                else
                {
                    renderUtility.BeginPreview(_Rect, _Background);
                    ReflectionUtility.CallMethod("DoRenderPreview", NativeEditor);
                    renderUtility.EndAndDrawPreview(_Rect);
                    RenderTexture dest = (RenderTexture) (renderUtility.GetType().GetProperty("renderTexture", ReflectionUtility.INSTANCE).GetValue(renderUtility));
                    RenderTexture active = RenderTexture.active;
                    Graphics.Blit((RenderTexture) (renderUtility.GetType().GetProperty("renderTexture", ReflectionUtility.INSTANCE).GetValue(renderUtility)), dest);
                    RenderTexture.active = active;
                    previewCache.Add(referenceTargetIndex, dest);
                }
            }
        }
        
    }

}