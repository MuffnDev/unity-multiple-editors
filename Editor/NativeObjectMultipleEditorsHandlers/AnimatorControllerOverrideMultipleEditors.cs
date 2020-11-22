using System;
using UnityEngine;
using UnityEditor;

namespace MuffinDev.MultipleEditors.Presets
{

    /// <summary>
    /// Handles multiple editors for AnimatorOverrideController assets.
    /// </summary>
    [CustomEditor(typeof(AnimatorOverrideController))]
    [CanEditMultipleObjects]
    public class AnimatorControllerOverrideMultipleEditors : NativeObjectMultipleEditorsHandler<AnimatorOverrideController>
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            ApplyHeaderOffset = true;
        }

    }

}