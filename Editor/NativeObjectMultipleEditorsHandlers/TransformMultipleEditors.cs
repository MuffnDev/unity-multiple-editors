﻿using UnityEngine;
using UnityEditor;

namespace MuffinDev.MultipleEditors.Presets
{

    /// <summary>
    /// Handles multiple editors for Transform component.
    /// </summary>
    [CustomEditor(typeof(Transform))]
    [CanEditMultipleObjects]
    public class TransfromMultipleEditors : NativeObjectMultipleEditorsHandler<Transform> { }

}