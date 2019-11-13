using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ALIGN
{ 
    ALIGN_LT = 0,
    ALIGN_T,
    ALIGN_RT,
    ALIGN_L,
    ALIGN_M,
    ALIGN_R,
    ALIGN_LB,
    ALIGN_B,
    ALIGN_RB,
};

public class MapTool : EditorWindow
{
    public static MapTool instance;

    static MapTool mapToolWIndow;

    Vector2 scrollPos;

    public static bool IsActivateTools;
    public static bool overWrite;
    public static bool snapping;

    //align
    Vector2 alignPos;
    int alignId;

    bool IsPlaying;

    // Tile.
    static SpriteRenderer gizmoTilesr;

    public float layerDepthMultiplier = 0.1f;

    public static int curLayer;
    public static Layer activeLayer;

    public Tool currentTool;

    public static bool mouseDown;

    public static GameObject gizmoCursor, gizmoTile;

    public Vector2 mousePos;

    public bool showConsole = false;

    static List<GameObject> allPrefabs;
    public GameObject curPrefab;

    List<Layer> layers;
    static int selGridInt = 0;

    public int controlID;

    bool showAlign = true;


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
        LoadLayers();

        SceneView.duringSceneGui += SceneGUI;
    }
    private void OnDisable()
    {
        Tools.current = currentTool;
        DestroyGizmo();
        SceneView.duringSceneGui -= SceneGUI;
    }

    private void DestroyGizmo()
    {
        DestroyImmediate(GameObject.Find("gizmoTile"));
        DestroyImmediate(GameObject.Find("gizmoCursor"));
    }

    private void OnFocus()
    {
        LoadPrefabs();
        if (Tools.current != Tool.None)
            currentTool = Tools.current;

        Tools.current = Tool.None;
        if (gizmoTile != null)
            gizmoTile.SetActive(true);
        if (gizmoCursor != null)
            gizmoCursor.SetActive(true);
        IsActivateTools = true;

        if (gizmoTilesr == null && gizmoTile != null)
            gizmoTilesr = gizmoTile.GetComponent<SpriteRenderer>();
    }
    private void Init()
    {
        alignId = (int)ALIGN.ALIGN_M;

        layerDepthMultiplier = 0.1f;
        currentTool = Tools.current;

        selGridInt = 0;
        instance = this;

        alignPos = Vector2.zero;
        overWrite = true;
        snapping = true;
        IsActivateTools = true;

        layers = new List<Layer>();
    }
    private void SceneGUI(SceneView sceneView)
    {
        if (Application.isPlaying)
        {
            DestroyGizmo();
            IsActivateTools = false;
        }
        else if (Application.isPlaying == false && IsPlaying == true)
        {
            DestroyGizmo();
            IsActivateTools = true;
        }

        IsPlaying = Application.isPlaying;

        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.M)
        {
            ActivateTools();
            IsActivateTools = true;
            Tools.current = Tool.None;
        }

        if (Tools.current != Tool.None)
            IsActivateTools = false;

        mousePos = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;

        if (IsActivateTools == false)
        {
            if (gizmoTile != null)
                gizmoTile.SetActive(false);
            if (gizmoCursor != null)
                gizmoCursor.SetActive(false);
            return;
        }

        controlID = GUIUtility.GetControlID(FocusType.Passive);
        if (currentEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(controlID);

        }

        InputMouse();
        CursorUpdate();

        Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, false, false);
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
            while (selGridInt >= allPrefabs.Count)
                selGridInt--;

            selGridInt = GUILayout.SelectionGrid(selGridInt, contents, 5,
                GUILayout.Height(50 * (Mathf.Ceil(allPrefabs.Count / (float)5))),
                GUILayout.Width(this.position.width - 30));

            if (EditorGUI.EndChangeCheck())
            {
                ChangeGizmoTile();
            }
            curPrefab = allPrefabs[selGridInt];
        }

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        curLayer = EditorGUILayout.IntField("Layer", curLayer);

        snapping = EditorGUILayout.Toggle(new GUIContent("Snapping", "Should tiles snap to the grid"), snapping);
        overWrite = EditorGUILayout.Toggle(new GUIContent("OverWrite", "Do you want to overwrite tile in the same layer and position"), overWrite);
        showConsole = EditorGUILayout.Toggle(new GUIContent("Show in Console", "Show Whats happening on the console"), showConsole);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(instance, "Name");
        }
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        showAlign = EditorGUILayout.Foldout(showAlign, "Alignment");

        if (EditorGUI.EndChangeCheck()) { }

        if (showAlign)
        {
            EditorGUI.BeginChangeCheck();

            alignId = GUILayout.SelectionGrid(alignId, new string[9], 3, GUILayout.MaxHeight(100), GUILayout.MaxWidth(100));
            if (EditorGUI.EndChangeCheck())
            {
                alignPos = alignId2Vec(alignId);
            }
        }

        EditorGUI.BeginChangeCheck();

        curPrefab = (GameObject)EditorGUILayout.ObjectField("Currnet Prefab", curPrefab, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            if (allPrefabs != null)
            {
                int activePre = allPrefabs.IndexOf(curPrefab);

                if (activePre > 0)
                {
                    selGridInt = activePre;
                }
            }

        }
        Texture2D previewImage = AssetPreview.GetAssetPreview(curPrefab);
        GUILayout.Box(previewImage);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

    }

    private void LoadLayers()
    {
        if (layers != null)
            layers.Clear();
        foreach (var item in Object.FindObjectsOfType<Layer>())
        {
            layers.Add(item);
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

    private GameObject isObjectAt(Vector2 tilePos, int curLayer)
    {
        object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object item in objects)
        {
            GameObject gameObject = (GameObject)item;

            ArtificialPosition artPos = gameObject.GetComponent<ArtificialPosition>();

            if (artPos == null)
            {
                if (gameObject.transform.localPosition == (Vector3)tilePos &&
                    (gameObject.name != "gizmoCursor" && gameObject.name != "gizmoTile"))
                {
                    if (gameObject.transform.parent != null
                        && gameObject.transform.parent.GetComponent<Layer>().priority == curLayer)
                    {
                        if (gameObject.transform.parent.parent == null)
                        {
                            return gameObject;
                        }
                    }
                }

            }
            else
            {
                if (artPos.position == tilePos && gameObject.name != "gizmoCursor")
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
                        IsActivateTools = true;
                        Tools.current = Tool.None;
                    }
                }
                break;
        }

        if (mouseDown && currentEvent.shift == false)
        {
            if (snapping == false)
                mouseDown = false;

            AddTile(gizmoCursor.transform.position, curLayer);
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

            if (curPrefab != null)
                gizmoTile.transform.localScale = curPrefab.transform.localScale;
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
        if (curPrefab == null)
            return;

        GameObject metaTile = (GameObject)Instantiate(curPrefab);

        metaTile.transform.SetParent(FindLayer(layer).transform);
        metaTile.transform.localPosition = (Vector3)pos + metaTile.transform.InverseTransformVector(OffsetWeirdTiles());

        if (metaTile.transform.localPosition != (Vector3)pos)
        {
            ArtificialPosition artPos = metaTile.AddComponent<ArtificialPosition>();
            artPos.position = pos;
            artPos.offset = artPos.position - (Vector2)metaTile.transform.position;
            artPos.layer = curLayer;
        }
        Undo.RegisterCreatedObjectUndo(metaTile, "Created go");
    }

    Vector3 OffsetWeirdTiles()
    {
        if (gizmoTilesr != null && gizmoTilesr.sprite != null && (gizmoTilesr.sprite.bounds.extents.x != 0.5f || gizmoTilesr.sprite.bounds.extents.y != 0.5f))
            return new Vector3(-alignPos.x * (gizmoTilesr.sprite.bounds.extents.x - 0.5f), alignPos.y * (gizmoTilesr.sprite.bounds.extents.y - 0.5f), 0);

        return Vector3.zero;
    }

    GameObject FindLayer(int currentLayer)
    {
        bool create = true;

        GameObject layer = null;

        if (layers.Count == 0)
        {
            foreach (Layer item in Object.FindObjectsOfType<Layer>())
            {
                layers.Add(item);
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
        for (i = 0; i < layers.Count && currentLayer > layers[i].priority; i++) { }

        layers.Insert(i, layer.GetComponent<Layer>());

        for (int j = 0; j < layers.Count; j++)
        {
            for (int k = 0; k < layers.Count - 1; k++)
            {
                if (layers[k].transform.GetSiblingIndex() > layers[k + 1].transform.GetSiblingIndex())
                {
                    int siblingindex = layers[k].transform.GetSiblingIndex();
                    layers[k].transform.SetSiblingIndex(layers[k + 1].transform.GetSiblingIndex());
                    layers[k + 1].transform.SetSiblingIndex(siblingindex);
                }
            }
        }
        return layer;
    }

    private static void ChangeGizmoTile()
    {

        if (gizmoTile != null)
            DestroyImmediate(gizmoTile);
        if (allPrefabs != null && allPrefabs.Count > selGridInt && allPrefabs[selGridInt] != null)
            gizmoTile = Instantiate(allPrefabs[selGridInt]) as GameObject;
        else
            gizmoTile = new GameObject();

        gizmoTile.name = "gizmoTile";
        
        if (gizmoTilesr == null)
            gizmoTilesr = gizmoTile.GetComponent<SpriteRenderer>();

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

    Vector2 alignId2Vec(int alignIndex)
    {
        Vector2 aux;

        aux.x = alignIndex % 3 - 1;
        aux.y = alignIndex / 3 - 1;

        return aux;
    }

}
