using UnityEngine;
using UnityEditor;

using MuffinDev.MultipleEditors.Utilities;

namespace MuffinDev.MultipleEditors.Utilities.EditorOnly
{

    ///<summary>
    /// This utility allows you to force Unity to recompile code.
    /// You can use Recompiler.Recompile() to do it through code, or from Assets > Recompile.
    ///</summary>
    public class Recompiler : ScriptableObject
    {
        
        /// <summary>
        /// Recompiles code.
        /// </summary>
        public static void Recompile()
        {
            string scriptPath = ScriptableObjectExtension.GetScriptPath<Recompiler>();
            AssetDatabase.ImportAsset(scriptPath, ImportAssetOptions.ForceUpdate);
        }

    }

}