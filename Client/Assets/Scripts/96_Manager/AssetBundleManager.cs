using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AssetBundleManager
{
    private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();
    AssetBundle myLoadedAssetBundle;
    public void LoadAssetFromLocalDisk(string assetBundleName)
    {
        myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }
        else
            Debug.Log("Successed to load AssetBundle!");
    }

    public GameObject LoadAsset(string name)
    {
        return LoadAssetFromCache(name);
    }
    public void UnLoadAssetBundle(bool isAllUnload)
    {
        ClearCache();
        myLoadedAssetBundle.Unload(isAllUnload);
    }

    private GameObject LoadAssetFromCache(string name)
    {
        GameObject resourceObj = null;
        _cache.TryGetValue(name, out resourceObj);
        if (resourceObj == null)
        {
            resourceObj = myLoadedAssetBundle.LoadAsset<GameObject>(name);
            if (resourceObj != null)
            {
                _cache.Add(name, resourceObj);
            }
        }
        return resourceObj;
    }
    private void ClearCache()
    {
        _cache.Clear();
    }
}
