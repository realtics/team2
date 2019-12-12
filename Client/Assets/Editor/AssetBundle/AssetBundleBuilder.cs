using UnityEngine;
using UnityEditor;

public class AssetBundleBuilder : ScriptableObject
{
    [MenuItem("Assets/BuildAssetBundle")]
    static void BuildBundle()
    {
        BuildPipeline.BuildAssetBundles("Assets\\StreamingAssets", 
            BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}