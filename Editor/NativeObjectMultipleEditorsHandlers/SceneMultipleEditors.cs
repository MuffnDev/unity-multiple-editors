using UnityEditor;

namespace MuffinDev.MultipleEditors
{

    /// <summary>
    /// Handles multiple editors for TextAsset component.
    /// </summary>
    [CustomEditor(typeof(SceneAsset))]
    [CanEditMultipleObjects]
    public class SceneMultipleEditors : MultipleEditorsHandler<SceneAsset>
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            ApplyHeaderOffset = true;
        }

    }

}