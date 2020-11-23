using UnityEngine;
using UnityEditor;

namespace MuffinDev.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for MonoScript component.
    /// </summary>
    [CustomEditor(typeof(MonoScript))]
    [CanEditMultipleObjects]
    public class MonoScriptMultipleEditors : NativeObjectMultipleEditorsHandler<MonoScript>
    {

        private const float IMPORTED_OBJECT_NOTE_OFFSET = -22f;
        private const float HEADER_CONTENT_OFFSET = -5f;

        protected override void OnHeaderGUI()
        {
            GUILayout.Space(IMPORTED_OBJECT_NOTE_OFFSET);
            GUIStyle header = new GUIStyle("AC BoldHeader");
            GUILayout.Box("", header, GUILayout.ExpandWidth(true));

            GUILayout.Space(HEADER_CONTENT_OFFSET);
            ExtensionsManager.DrawCustomEditorsBeforeHeaderGUI();
            ExtensionsManager.DrawCustomEditorsHeaderGUI();
        }

    }

}