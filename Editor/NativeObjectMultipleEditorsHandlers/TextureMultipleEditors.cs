using UnityEditor;

namespace MuffinDev.MultipleEditors
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

#if UNITY_2019_1_OR_NEWER
            ExtensionsManager.DrawCustomEditorsInspectorGUI();
#else
            EditorGUILayout.EndHorizontal();
            ExtensionsManager.DrawCustomEditorsInspectorGUI();
            //EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUIUtility.ExitGUI();
#endif
        }

    }

}