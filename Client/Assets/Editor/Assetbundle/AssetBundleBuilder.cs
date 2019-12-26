using UnityEngine;
using UnityEditor;

public class AssetBundleBuilder : ScriptableObject
{
    [MenuItem("Assets/BuildAndroidAssetBundle")]
    static void BuildAndroidBundle()
    {
        BuildPipeline.BuildAssetBundles("Assets\\StreamingAssets\\Android", BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("Assets/BuildWindowAssetBundle")]
    static void BuildWindowBundle()
    {
        BuildPipeline.BuildAssetBundles("Assets\\StreamingAssets\\StandaloneWindows", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}