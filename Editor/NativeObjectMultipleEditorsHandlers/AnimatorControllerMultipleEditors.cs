using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace MuffinDev.EditorUtils.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for AnimatorController assets.
    /// </summary>
    [CustomEditor(typeof(AnimatorController))]
    [CanEditMultipleObjects]
    public class AnimatorControllerMultipleEditors : MultipleEditorsHandler<AnimatorController>
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            ApplyHeaderOffset = true;
        }

    }

}