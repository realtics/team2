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
    private AssetBundle _LoadedMapAssetBundle;
    private AssetBundle _LoadedMonsterAssetBundle;
    private AssetBundle _LoadedPotalAssetBundle;
    private AssetBundle _LoadedMaterialAssetBundle;
    private AssetBundle _LoadedAtlasAssetBundle;
    private AssetBundleManifest _manifest;
    private string _atlasBundle = "atlas";
    private string _materialBundle = "material";
    private string _loadMonseterAssetName = "monster";
    private string _loadPotalAssetName = "dungeon/potal";

    private bool _firstLoadMapAssetbundle = false;
    private bool _firstLoadMonseterAssetbundle = false;
    private bool _firstLoadPotalAssetbundle = false;
    private bool _firstLoadAtlas = false;

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
        Debug.Log(tag);
        if(!_firstLoadAtlas)
        {
            _LoadedAtlasAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, _atlasBundle)).assetBundle;
            _firstLoadAtlas = true;
        }

        var sa = _LoadedAtlasAssetBundle.LoadAsset<SpriteAtlas>(tag);
        action(sa);
    }

    //private void RequestLateBindingAtlas(string tag, System.Action<SpriteAtlas> action)
    //{
    //    StartCoroutine(LoadSpriteAtlasFromAssetBundle(tag, action));
    //}
    //private IEnumerator LoadSpriteAtlasFromAssetBundle(string tag, System.Action<SpriteAtlas> action)
    //{
    //    Debug.Log(tag);
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
        _LoadedMaterialAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, _materialBundle)).assetBundle;
        _LoadedMaterialAssetBundle.LoadAllAssets();
    }
    //private IEnumerator LoadMaterialFromAssetBundle()
    //{
    //    //LoadAssetBundleManifest(maBundle);
    //    _LoadedMaterialAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, maBundle));
    //    yield return _LoadedMaterialAssetBundle;
    //    _LoadedMaterialAssetBundle.LoadAllAssets();
    //}
    public void LoadMapAssetFromLocalDisk(string assetBundleName)
    {
        Debug.Log(assetBundleName);
        if(!_firstLoadMapAssetbundle)
        {
            _LoadedMapAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName)).assetBundle;
            _firstLoadMapAssetbundle = true;
        }
#if UNITY_EDITOR
        if (_LoadedMapAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }
        else
            Debug.Log("Successed to load AssetBundle!");
#endif
    }
    public void LoadMonsterAssetFromLocalDisk(string assetBundleName)
    {
        if (!_firstLoadMonseterAssetbundle)
        {
            _LoadedMonsterAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName)).assetBundle;
            _firstLoadMonseterAssetbundle = true;
        }

#if UNITY_EDITOR
        if (_LoadedMonsterAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }
        else
            Debug.Log("Successed to load AssetBundle!");
#endif
    }
    public void LoadPotalAssetFromLocalDisk(string assetBundleName)
    {
        Debug.Log(assetBundleName);
        _LoadedPotalAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName)).assetBundle;
#if UNITY_EDITOR
        if (_LoadedPotalAssetBundle == null)
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

    public GameObject LoadPotalAsset(string name)
    {
        return LoadAssetFromCache(name,ref _LoadedPotalAssetBundle);
    }

    public GameObject LoadMonsterAsset(string name)
    {
        return LoadAssetFromCache(name,ref _LoadedMonsterAssetBundle);
    }

    public Object LoadMapObjectAsset(string name)
    {
        return _LoadedMapAssetBundle.LoadAsset<Object>(name);
    }

    public void UnLoadAssetBundle(bool isUnloadAll)
    {
        _LoadedMapAssetBundle.Unload(isUnloadAll);
    }
    public void UnLoadMonserAssetBundle(bool isUnloadAll)
    {
        _LoadedMonsterAssetBundle.Unload(isUnloadAll);
    }
    public void UnLoadPotalAssetBundle(bool isUnloadAll)
    {
        _LoadedPotalAssetBundle.Unload(isUnloadAll);
    }

    private GameObject LoadAssetFromCache(string name, ref AssetBundle assetBundle)
    {
        GameObject resourceObj = null;
        _cache.TryGetValue(name, out resourceObj);
        if (resourceObj == null)
        {
            resourceObj = assetBundle.LoadAsset<GameObject>(name);
            if (resourceObj != null)
            {
                _cache.Add(name, resourceObj);
            }
        }
        return resourceObj;
    }
    public void ClearCache()
    {
        _cache.Clear();
    }
}