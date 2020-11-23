using UnityEngine;
using UnityEditor;

namespace MuffinDev.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for Rigidbody component.
    /// </summary>
    [CustomEditor(typeof(Rigidbody))]
    [CanEditMultipleObjects]
    public class RigidbodyMultipleEditors : MultipleEditorsHandler<Rigidbody> { }

}