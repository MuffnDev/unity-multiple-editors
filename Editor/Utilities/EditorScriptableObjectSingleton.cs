using System.IO;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuffinDev.MultipleEditors.Utilities
{

    ///<summary>
    /// Implements singleton pattern for assets used in the Editor (tool settings for example).
    /// 
    /// WARNING: This type of asset is meant to be used only in the editor. Anyway, this script is not placed in an Editor folder, in order
    /// to let the possibility for non-editor objects (like MonoBehaviour) to interact with it if needed. but out of the editor context,
    /// you won't be able to get its instance.
    ///</summary>
    public abstract class EditorScriptableObjectSingleton<TSingletonType> : ScriptableObject
        where TSingletonType : ScriptableObject
    {

#if UNITY_EDITOR
        private static TSingletonType s_Instance = null;
#endif

        /// <summary>
        /// Gets the unique instance of this asset, or creates it if it doesn't exist yet.
        /// WARNING: This method is meant to be used in the editor. Using it out of this context will always return null.
        /// </summary>
        public static TSingletonType Instance
        {
            get
            {
#if !UNITY_EDITOR
                return null;
#else
                // Get the instance of the target singleton class from project assets if no one has been loaded before
                if (s_Instance == null)
                {
                    string[] managers = AssetDatabase.FindAssets($"t:{typeof(TSingletonType)}");
                    if (managers.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(managers[0]);
                        s_Instance = AssetDatabase.LoadAssetAtPath<TSingletonType>(path);
                    }
                }

                // Create the asset if no one exists in this project
                if (s_Instance == null)
                {
                    string scriptPath = ScriptableObjectExtension.GetScriptPath<TSingletonType>();
                    if (scriptPath != null)
                    {
                        s_Instance = CreateInstance<TSingletonType>();
                        string assetPath = $"{Path.GetDirectoryName(scriptPath)}/{typeof(TSingletonType).Name}.asset";
                        AssetDatabase.CreateAsset(s_Instance, assetPath);
#if MUFFINDEV_PROJECT
                        Debug.Log($"No {typeof(TSingletonType)} asset found in the project: a new one has been created at " + scriptPath);
                        EditorApplication.delayCall += () => { EditorGUIUtility.PingObject(s_Instance); };
#endif
                    }
                }
                return s_Instance;
#endif
            }
        }

    }

}