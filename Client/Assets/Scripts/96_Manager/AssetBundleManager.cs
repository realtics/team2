using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.U2D;
using System.Threading.Tasks;

public class AssetBundleManager : MonoBehaviour
{
    private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();
    private AssetBundle _LoadedAssetBundle;
    private AssetBundle _LoadedMaterialAssetBundle;
    private AssetBundle _LoadedshBundleAssetBundle;
    private AssetBundleManifest _manifest;
    private string saBundle = "monster";
    private string maBundle = "material";

    private static AssetBundleManager _instacne;
    public static AssetBundleManager instacne
    {
        get
        {
            return _instacne;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _instacne = this;
    }

    void OnEnable()
    {
        SpriteAtlasManager.atlasRequested += RequestLateBindingAtlas;
        LoadMaterial();
    }
    void OnDisable()
    {
        SpriteAtlasManager.atlasRequested -= RequestLateBindingAtlas;
        _LoadedMaterialAssetBundle.Unload(false);
    }
    private void RequestLateBindingAtlas(string tag, System.Action<SpriteAtlas> action)
    {
        var loadOp = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, saBundle));
        var sa = loadOp.assetBundle.LoadAsset<SpriteAtlas>(tag);
        action(sa);
    }

    //private void RequestLateBindingAtlas(string tag, System.Action<SpriteAtlas> action)
    //{
    //    StartCoroutine(LoadSpriteAtlasFromAssetBundle(tag, action));
    //}
    //private IEnumerator LoadSpriteAtlasFromAssetBundle(string tag, System.Action<SpriteAtlas> action)
    //{
    //    var loadOp = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, saBundle));
    //    yield return loadOp;
    //    var sa = loadOp.LoadAsset<SpriteAtlas>(tag);
    //    action(sa);
    //}
    //private void LoadMaterial()
    //{
    //    StartCoroutine(LoadMaterialFromAssetBundle());
    //}
    private void LoadMaterial()
    {
        _LoadedMaterialAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, maBundle)).assetBundle;
        _LoadedMaterialAssetBundle.LoadAllAssets();
    }
    //private IEnumerator LoadMaterialFromAssetBundle()
    //{
    //    //LoadAssetBundleManifest(maBundle);
    //    _LoadedMaterialAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, maBundle));
    //    yield return _LoadedMaterialAssetBundle;
    //    _LoadedMaterialAssetBundle.LoadAllAssets();
    //}
    public void LoadAssetFromLocalDisk(string assetBundleName)
    {
        _LoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
#if UNITY_EDITOR
        if (_LoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }
        else
            Debug.Log("Successed to load AssetBundle!");
#endif
    }
    private void LoadAssetBundleManifest(string assetBundleName)
    {
        if(_manifest == null)
        {
            AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "StreamingAssets"));
            _manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
        }
        string[] dependencies = _manifest.GetAllDependencies(assetBundleName);


        foreach (string dependency in dependencies)
        {
            _LoadedMaterialAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependency));
            _LoadedMaterialAssetBundle.LoadAllAssets();
        }

        //AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "monster"));
        //Hash128 hash = assetBundleManifest.GetAssetBundleHash(assetBundle.name);
    }
    public Object GetMaterial(string name)
    {
        return _LoadedMaterialAssetBundle.LoadAsset<Object>(name);
    }

    public GameObject LoadAsset(string name)
    {
        return LoadAssetFromCache(name);
    }

    public Object LoadUnCacheObjectAsset(string name)
    {
        return _LoadedAssetBundle.LoadAsset<Object>(name);
    }

    public void UnLoadAssetBundle(bool isUnloadAll)
    {
        ClearCache();
        _LoadedAssetBundle.Unload(isUnloadAll);
    }

    private GameObject LoadAssetFromCache(string name)
    {
        GameObject resourceObj = null;
        _cache.TryGetValue(name, out resourceObj);
        if (resourceObj == null)
        {
            resourceObj = _LoadedAssetBundle.LoadAsset<GameObject>(name);
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