using UnityEngine;
using UnityEditor;

namespace MuffinDev.MultipleEditors.Presets
{

    /// <summary>
    /// Handles multiple editors for TextAsset component.
    /// </summary>
    [CustomEditor(typeof(TextAsset))]
    [CanEditMultipleObjects]
    public class TextAssetMultipleEditors : MultipleEditorsHandler<TextAsset>
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            ApplyHeaderOffset = true;
        }

    }

}