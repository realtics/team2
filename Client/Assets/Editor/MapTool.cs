using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ALIGN
{ 
    LeftTop = 0,
    Top,
    RightTop,
    Left,
    Middle,
    Right,
    LeftBottom,
    Bottom,
    RightBottom,
};

public class MapTool : EditorWindow
{
    public static MapTool Instance { get; set; }

    static MapTool mapToolWIndow;

    private Vector2 _scrollPos;

    //Tool Function.
    public static bool isActivateTools;
    public static bool overWrite;
    public static bool snapping;

    //align
    private Vector2 _alignPos;
    private int _alignId;

    private bool _isPlaying;

    // Tile.
    static SpriteRenderer gizmoTileSpriteRenderer;

    public float layerDepthMultiplier = 0.1f;

    public static int currentLayer;
    public static Layer activeLayer;

    private Tool _currentTool;

    public static bool mouseDown;

    public static GameObject gizmoCursor, gizmoTile;

    private Vector2 mousePos;

    private bool _showConsole = false;

    static List<GameObject> allPrefabs;
    private GameObject _currentPrefab;

    private List<Layer> _layers;

    static int selectGrid = 0;

    private int _controlID;

    private bool _showAlign = true;


    [MenuItem("Window/MapTool/Open Editor %m", false, 1)]
    static void InitWindow()
    {
        mapToolWIndow = (MapTool)EditorWindow.GetWindow(typeof(MapTool));
        mapToolWIndow.Show();

        mapToolWIndow.minSize = new Vector2(200, 315);
        mapToolWIndow.titleContent = new GUIContent("MDNF MapTool");
    }

    private void OnEnable()
    {
        Init();
        Load_layers();

        SceneView.duringSceneGui += SceneGUI;
    }
    private void OnDisable()
    {
        Tools.current = _currentTool;
        DestroyGizmo();
        SceneView.duringSceneGui -= SceneGUI;
    }

    private void DestroyGizmo()
    {
        DestroyImmediate(gizmoCursor);
        DestroyImmediate(gizmoTile);
    }

    private void OnFocus()
    {
        LoadPrefabs();
        if (Tools.current != Tool.None)
            _currentTool = Tools.current;

        Tools.current = Tool.None;
        if (gizmoTile != null)
            gizmoTile.SetActive(true);
        if (gizmoCursor != null)
            gizmoCursor.SetActive(true);
        isActivateTools = true;

        if (gizmoTileSpriteRenderer == null && gizmoTile != null)
            gizmoTileSpriteRenderer = gizmoTile.GetComponent<SpriteRenderer>();
    }
    private void Init()
    {
        _alignId = (int)ALIGN.Middle;

        layerDepthMultiplier = 0.1f;
        _currentTool = Tools.current;

        selectGrid = 0;
        Instance = this;

        _alignPos = Vector2.zero;
        overWrite = true;
        snapping = true;
        isActivateTools = true;

        _layers = new List<Layer>();
    }
    private void SceneGUI(SceneView sceneView)
    {
        if (Application.isPlaying)
        {
            DestroyGizmo();
            isActivateTools = false;
        }
        else if (Application.isPlaying == false && _isPlaying == true)
        {
            DestroyGizmo();
            isActivateTools = true;
        }

        _isPlaying = Application.isPlaying;

        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.M)
        {
            ActivateTools();
            isActivateTools = true;
            Tools.current = Tool.None;
        }

        if (Tools.current != Tool.None)
            isActivateTools = false;

        mousePos = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;

        if (isActivateTools == false)
        {
            if (gizmoTile != null)
                gizmoTile.SetActive(false);
            if (gizmoCursor != null)
                gizmoCursor.SetActive(false);
            return;
        }

        _controlID = GUIUtility.GetControlID(FocusType.Passive);
        if (currentEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(_controlID);

        }

        InputMouse();
        CursorUpdate();

        Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        _scrollPos =
            EditorGUILayout.BeginScrollView(_scrollPos, false, false);
        EditorGUILayout.LabelField("Select a prefab");

        if (allPrefabs != null && allPrefabs.Count > 0)
        {
            GUIContent[] contents = new GUIContent[allPrefabs.Count];

            for (int i = 0; i < allPrefabs.Count; i++)
            {
                if (allPrefabs[i] != null && allPrefabs[i].name != "")
                    contents[i] = new GUIContent(allPrefabs[i].name, AssetPreview.GetAssetPreview(allPrefabs[i]));
                if (contents[i] == null)
                    contents[i] = GUIContent.none;
            }
            EditorGUI.BeginChangeCheck();
            while (selectGrid >= allPrefabs.Count)
                selectGrid--;

            selectGrid = GUILayout.SelectionGrid(selectGrid, contents, 5,
                GUILayout.Height(50 * (Mathf.Ceil(allPrefabs.Count / (float)5))),
                GUILayout.Width(this.position.width - 30));

            if (EditorGUI.EndChangeCheck())
            {
                ChangeGizmoTile();
            }
            _currentPrefab = allPrefabs[selectGrid];
        }

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        currentLayer = EditorGUILayout.IntField("Layer", currentLayer);

        snapping = EditorGUILayout.Toggle(new GUIContent("Snapping", "Should tiles snap to the grid"), snapping);
        overWrite = EditorGUILayout.Toggle(new GUIContent("OverWrite", "Do you want to overwrite tile in the same layer and position"), overWrite);
        _showConsole = EditorGUILayout.Toggle(new GUIContent("Show in Console", "Show Whats happening on the console"), _showConsole);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(Instance, "Name");
        }
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        _showAlign = EditorGUILayout.Foldout(_showAlign, "Alignment");

        if (EditorGUI.EndChangeCheck()) { }

        if (_showAlign)
        {
            EditorGUI.BeginChangeCheck();

            _alignId = GUILayout.SelectionGrid(_alignId, new string[9], 3, GUILayout.MaxHeight(100), GUILayout.MaxWidth(100));
            if (EditorGUI.EndChangeCheck())
            {
                _alignPos = _alignId2Vec(_alignId);
            }
        }

        EditorGUI.BeginChangeCheck();

        _currentPrefab = (GameObject)EditorGUILayout.ObjectField("Currnet Prefab", _currentPrefab, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            if (allPrefabs != null)
            {
                int activePre = allPrefabs.IndexOf(_currentPrefab);

                if (activePre > 0)
                {
                    selectGrid = activePre;
                }
            }

        }
        Texture2D previewImage = AssetPreview.GetAssetPreview(_currentPrefab);
        GUILayout.Box(previewImage);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

    }

    private void Load_layers()
    {
        if (_layers != null)
            _layers.Clear();
        foreach (var item in Object.FindObjectsOfType<Layer>())
        {
            _layers.Add(item);
        }
    }

    private void LoadPrefabs()
    {
        if (allPrefabs == null)
            allPrefabs = new List<GameObject>();
        allPrefabs.Clear();

        var loadedObjects = Resources.LoadAll("");

        foreach (var loadedObject in loadedObjects)
        {
            if (loadedObject.GetType() == typeof(GameObject))
                allPrefabs.Add(loadedObject as GameObject);
        }

    }
    private GameObject isObjectAt(Vector2 tilePos, int currentLayer)
    {
        object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object item in objects)
        {
            GameObject gameObject = (GameObject)item;

            ArtificialPosition artPos = gameObject.GetComponent<ArtificialPosition>();

            if (artPos != null)
            {
                if (artPos.position == tilePos && gameObject.name != "gizmoCursor")
                {
                    return gameObject;
                }
            }

            if (gameObject.transform.localPosition != (Vector3)tilePos)
            {
                return null;
            }
            if (gameObject.name == "gizmoCursor" || gameObject.name == "gizmoTile")
            {
                return null;
            }

            if (gameObject.transform.parent != null
                && gameObject.transform.parent.GetComponent<Layer>().priority == currentLayer)
            {
                if (gameObject.transform.parent.parent == null)
                {
                    return gameObject;
                }
            }
        }
        return null;
    }

    private void ActivateTools()
    {
        Tools.current = Tool.None;
        if (gizmoTile != null)
            gizmoTile.SetActive(true);
        if (gizmoCursor != null)
            gizmoCursor.SetActive(true);
    }

    private void InputMouse()
    {
        Event currentEvent = Event.current;
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                {
                    if (currentEvent.button == 0) //LEFT CLICK DOWN
                    {
                        mouseDown = true;
                    }
                    break;
                }
            case EventType.MouseUp:
                {
                    if (currentEvent.button == 0) //LEFT CLICK UP
                    {
                        mouseDown = false;
                    }
                    break;
                }
            case EventType.KeyDown:
                {
                    if (currentEvent.keyCode == KeyCode.M)
                    {
                        isActivateTools = true;
                        Tools.current = Tool.None;
                    }
                }
                break;
        }

        if (mouseDown && currentEvent.shift == false)
        {
            if (snapping == false)
                mouseDown = false;

            AddTile(gizmoCursor.transform.position, currentLayer);
        }
    }
    private void CursorUpdate()
    {
        if (gizmoCursor == null)
        {
            GameObject pointer = (GameObject)Resources.Load("TilePointerGizmo", typeof(GameObject));
            if (pointer != null)
                gizmoCursor = (GameObject)Instantiate(pointer);
            else
                gizmoCursor = new GameObject();

            gizmoCursor.name = "gizmoCursor";
        }
        if (gizmoTile == null)
        {
            if (allPrefabs != null && allPrefabs.Count > 0)
                ChangeGizmoTile();
            else
                gizmoTile = new GameObject();
        }

        if (gizmoCursor != null)
        {
            if (snapping)
            {
                Vector2 gizmoPos = Vector2.zero;
                if (mousePos.x - Mathf.Floor(mousePos.x) < 0.5f)
                {
                    gizmoPos.x = Mathf.Floor(mousePos.x) + 0.5f;
                }
                else if (Mathf.Ceil(mousePos.x) - mousePos.x < 0.5f)
                {
                    gizmoPos.x = Mathf.Ceil(mousePos.x) - 0.5f;
                }
                if (mousePos.y - Mathf.Floor(mousePos.y) < 0.5f)
                {
                    gizmoPos.y = Mathf.Floor(mousePos.y) + 0.5f;
                }
                else if (Mathf.Ceil(mousePos.y) - mousePos.y < 0.5f)
                {
                    gizmoPos.y = Mathf.Ceil(mousePos.y) - 0.5f;
                }

                gizmoCursor.transform.position = gizmoPos;
                gizmoTile.transform.position = gizmoPos + (Vector2)gizmoTile.transform.InverseTransformVector(OffsetWeirdTiles());
            }
            else
            {
                gizmoCursor.transform.position = mousePos;
                gizmoTile.transform.position = mousePos;
            }

            if (_currentPrefab != null)
                gizmoTile.transform.localScale = _currentPrefab.transform.localScale;
        }
    }

    void AddTile(Vector2 pos, int layer)
    {
        GameObject gameObject = isObjectAt(pos, layer);

        if (gameObject == null)
        {
            InstantiateTile(pos, layer);
        }
        else if (overWrite)
        {
            Undo.DestroyObjectImmediate(gameObject);
            DestroyImmediate(gameObject);

            InstantiateTile(pos, layer);
        }
    }
    void InstantiateTile(Vector2 pos, int layer)
    {
        if (_currentPrefab == null)
            return;

        GameObject metaTile = (GameObject)Instantiate(_currentPrefab);

        metaTile.transform.SetParent(FindLayer(layer).transform);
        metaTile.transform.localPosition = (Vector3)pos + metaTile.transform.InverseTransformVector(OffsetWeirdTiles());

        if (metaTile.transform.localPosition != (Vector3)pos)
        {
            ArtificialPosition artPos = metaTile.AddComponent<ArtificialPosition>();
            artPos.position = pos;
            artPos.offset = artPos.position - (Vector2)metaTile.transform.position;
            artPos.layer = currentLayer;
        }
        Undo.RegisterCreatedObjectUndo(metaTile, "Created go");
    }

    Vector3 OffsetWeirdTiles()
    {
        if (gizmoTileSpriteRenderer != null && gizmoTileSpriteRenderer.sprite != null && (gizmoTileSpriteRenderer.sprite.bounds.extents.x != 0.5f || gizmoTileSpriteRenderer.sprite.bounds.extents.y != 0.5f))
            return new Vector3(-_alignPos.x * (gizmoTileSpriteRenderer.sprite.bounds.extents.x - 0.5f), _alignPos.y * (gizmoTileSpriteRenderer.sprite.bounds.extents.y - 0.5f), 0);

        return Vector3.zero;
    }

    GameObject FindLayer(int currentLayer)
    {
        bool create = true;

        GameObject layer = null;

        if (_layers.Count == 0)
        {
            foreach (Layer item in Object.FindObjectsOfType<Layer>())
            {
                _layers.Add(item);
            }
        }
        foreach (Layer item in Object.FindObjectsOfType<Layer>())
        {
            if (item.priority == currentLayer)
            {
                layer = item.gameObject;
                create = false;
                break;
            }
        }

        if (create)
        {
            layer = new GameObject("Layer" + currentLayer + " (" + currentLayer + ")");
            layer.AddComponent<Layer>();
            layer.GetComponent<Layer>().priority = currentLayer;
            layer.GetComponent<Layer>().id = layer.transform.GetSiblingIndex();
            layer.transform.position = Vector3.forward * layerDepthMultiplier * currentLayer;
        }

        int i;
        for (i = 0; i < _layers.Count && currentLayer > _layers[i].priority; i++) { }

        _layers.Insert(i, layer.GetComponent<Layer>());

        for (int j = 0; j < _layers.Count; j++)
        {
            for (int k = 0; k < _layers.Count - 1; k++)
            {
                if (_layers[k].transform.GetSiblingIndex() > _layers[k + 1].transform.GetSiblingIndex())
                {
                    int siblingindex = _layers[k].transform.GetSiblingIndex();
                    _layers[k].transform.SetSiblingIndex(_layers[k + 1].transform.GetSiblingIndex());
                    _layers[k + 1].transform.SetSiblingIndex(siblingindex);
                }
            }
        }
        return layer;
    }

    private static void ChangeGizmoTile()
    {

        if (gizmoTile != null)
            DestroyImmediate(gizmoTile);
        if (allPrefabs != null && allPrefabs.Count > selectGrid && allPrefabs[selectGrid] != null)
            gizmoTile = Instantiate(allPrefabs[selectGrid]) as GameObject;
        else
            gizmoTile = new GameObject();

        gizmoTile.name = "gizmoTile";
        
        if (gizmoTileSpriteRenderer == null)
            gizmoTileSpriteRenderer = gizmoTile.GetComponent<SpriteRenderer>();

        RecorrenciaSpriteRender(gizmoTile);
    }

    static void RecorrenciaSpriteRender(GameObject gameObject)
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }

        foreach (Transform t in gameObject.transform)
        {
            RecorrenciaSpriteRender(t.gameObject);
        }
    }

    Vector2 _alignId2Vec(int alignIndex)
    {
        Vector2 aux;

        aux.x = alignIndex % 3 - 1;
        aux.y = alignIndex / 3 - 1;

        return aux;
    }

}
