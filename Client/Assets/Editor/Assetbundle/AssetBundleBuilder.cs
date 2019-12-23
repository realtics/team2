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
    //[MenuItem("Assets/BuildNeedAssetBundle")]
    //static void BuildNeedBundle()
    //{
    //    string bundleName = "dungeon/potal";
    //    AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];
    //    buildBundles[0].assetBundleName = bundleName;
    //    buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);

    //    BuildPipeline.BuildAssetBundles(Application.dataPath + "/StreamingAssets", buildBundles, BuildAssetBundleOptions.None, BuildTarget.Android);
    //}
}