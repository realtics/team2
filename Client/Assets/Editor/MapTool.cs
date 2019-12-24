using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class MapTool : EditorWindow
{

    public static MapTool Instance { get; set; }

    static MapTool mapToolWIndow;

    private Vector2 _scrollPos;

    //Tool Function.
    public static bool isActivateTools;
    public static bool overWrite;
    public static bool snapping;

    public bool areaDeletion;
    public bool areaInsertion;

    //align
    private Vector2 _alignPos;

    private bool _isPlaying;

    private bool _switchTool;

    // Tile.
    static SpriteRenderer gizmoTileSpriteRenderer;

    public float layerDepthMultiplier = 0.1f;

    public Vector3 beginPos;
    public Vector3 endPos;

    public static int currentLayer;
    public static Layer activeLayer;

    private Tool _currentTool;

    //Holding certain keys
    public bool holdingR, holdingEscape, holdingRightMouse;
    public bool holdingTab, holdingS, holdingA;

    public static bool eraseTool;
    public static bool mouseDown;

    public static GameObject gizmoCursor, gizmoTile;

    private Vector2 mousePos;

    private bool _showConsole = false;

    private List<Layer> _layers;

    static int selectGrid = 0;

    private int _controlID;

    static List<GameObject> allPrefabs;
    private GameObject _currentPrefab;

    private int _selectRoomGrid = 0;
    private int _preSelectRoomGrid = 0;
    private TextAsset _dungeon;
    private JsonManagement _jsonManagement;
    private MapToolLoader _mapToolLoader;
    private MapToolSpawn _mapToolSpawn;

    public static int currentZOrder = 0;

    private bool _isLoadDungeon = false;

    private string _fileName = "";

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
        _jsonManagement = new JsonManagement();
        _mapToolLoader = new MapToolLoader();
        _mapToolSpawn = new MapToolSpawn();

        SceneView.duringSceneGui += SceneGUI;
    }
    private void OnDisable()
    {
        Tools.current = _currentTool;
        DestroyGizmo();
        _mapToolSpawn.DestoryAllObjects();
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
        ShowLog("MapMaker Activated");
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
        layerDepthMultiplier = 0.1f;
        _currentTool = Tools.current;

        selectGrid = 0;
        Instance = this;

        _alignPos = Vector2.zero;
        overWrite = true;
        snapping = true;
        isActivateTools = true;

        _layers = new List<Layer>();

        _switchTool = false;

        beginPos = Vector3.zero;
        endPos = Vector3.zero;

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
            ActivateTools(true);
            isActivateTools = true;
            Tools.current = Tool.None;
        }
        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.Escape)
        {
            holdingEscape = true;
        }

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
        {
            holdingEscape = true;
        }
        if (currentEvent.type == EventType.MouseUp && currentEvent.button == 1)
        {
            holdingEscape = true;
            _switchTool = false;
        }
        if (currentEvent.type == EventType.KeyUp && currentEvent.keyCode == KeyCode.Escape)
        {
            holdingEscape = false;

            _switchTool = false;
        }
        if (holdingEscape && _switchTool == false)
        {
            isActivateTools = !isActivateTools;

            if (isActivateTools)
            {
                ActivateTools(true);
            }
            else
                Tools.current = _currentTool;
            _switchTool = true;
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

        InputMouse(ref currentEvent);

        CursorUpdate();
        Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        using (new EditorGUILayout.HorizontalScope())
        {
            _fileName = EditorGUILayout.TextField(new GUIContent("FileName", "FileName .Json"), _fileName);
            if (GUILayout.Button("New"))
            {
                if(_fileName != null)
                {
                    _jsonManagement.NewJson(_fileName);
                    AssetDatabase.Refresh();
                }
            }
        }
        EditorGUILayout.LabelField("Select a Dungeon");

        EditorGUI.BeginChangeCheck();
        _dungeon = (TextAsset)EditorGUILayout.ObjectField(_dungeon, typeof(TextAsset), false);

        if (EditorGUI.EndChangeCheck())
        {
            _isLoadDungeon = false;
        }
        if (_dungeon != null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Save"))
                {
                    ActivateTools(false);
                    _mapToolLoader.SaveRoom(_preSelectRoomGrid);
                    _jsonManagement.SaveJson(_mapToolLoader.dungeonList, _dungeon.name);
                    AssetDatabase.Refresh();
                }
                if (GUILayout.Button("Load"))
                {
                    _mapToolSpawn.DestoryAllObjects();
                    _mapToolLoader.LoaderDungeon(_dungeon);
                    _mapToolSpawn.Spawn(_mapToolLoader.dungeonList.dungeonObjectList[_selectRoomGrid], SpawnTile);
                    _isLoadDungeon = true;
                }
                if (GUILayout.Button("export"))
                {
                    _jsonManagement.ExportJson(_mapToolLoader.dungeonList, _dungeon.name);
                    AssetDatabase.Refresh();
                }
            }
        }
        if(_isLoadDungeon)
        {
            EditorGUILayout.LabelField("Select a RoomSlot");

            int count = _mapToolLoader.dungeonList.dungeonObjectList.Count;
            GUIContent[] dungeonSlot = new GUIContent[count];
            for (int i = 0; i < count; i++)
            {
                if(_mapToolLoader.dungeonList.dungeonObjectList[i].isBoss)
                    dungeonSlot[i] = new GUIContent("Boss");
                else
                    dungeonSlot[i] = new GUIContent(i.ToString());

                if (dungeonSlot[i] == null)
                    dungeonSlot[i] = GUIContent.none;
            }

            EditorGUI.BeginChangeCheck();

            _selectRoomGrid = GUILayout.SelectionGrid(_selectRoomGrid, dungeonSlot, 5,
                GUILayout.Height(50 * (Mathf.Ceil(count / (float)5))),
                GUILayout.Width(this.position.width - 30));
 
            if (EditorGUI.EndChangeCheck())
            {
                ActivateTools(false);
                _mapToolLoader.SaveRoom(_preSelectRoomGrid);
                _mapToolSpawn.DestoryAllObjects();
                _mapToolSpawn.Spawn(_mapToolLoader.dungeonList.dungeonObjectList[_selectRoomGrid], SpawnTile);
                _preSelectRoomGrid = _selectRoomGrid;
            }
            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Add"))
                {
                    _mapToolLoader.AddRoom();
                }
                if (GUILayout.Button("Delete"))
                {
                    if(_mapToolLoader.dungeonList.dungeonObjectList.Count > 1)
                    {
                        _mapToolLoader.DeleteRoom(_selectRoomGrid);
                        _selectRoomGrid = 0;
                        ActivateTools(false);
                        _mapToolSpawn.DestoryAllObjects();
                        _mapToolSpawn.Spawn(_mapToolLoader.dungeonList.dungeonObjectList[_selectRoomGrid], SpawnTile);
                        _preSelectRoomGrid = _selectRoomGrid;
                    }
                }
            }
            bool on = _mapToolLoader.GetDungeonInfo(_selectRoomGrid).isBoss;
            _mapToolLoader.GetDungeonInfo(_selectRoomGrid).isBoss = GUILayout.Toggle(on, on ? "Boss on" : "Boss off", "button");
        }

        EditorGUILayout.Space();


        _scrollPos =
            EditorGUILayout.BeginScrollView(_scrollPos, false, false);
        EditorGUILayout.LabelField("Select a prefab");

        if (allPrefabs != null && allPrefabs.Count > 0)
        {
            GUIContent[] contents = new GUIContent[allPrefabs.Count];

            for (int i = 0; i < allPrefabs.Count; i++)
            {
                if (allPrefabs[i] != null && allPrefabs[i].name != "")
                    contents[i] = new GUIContent(AssetPreview.GetAssetPreview(allPrefabs[i]));
                    //contents[i] = new GUIContent(allPrefabs[i].name, AssetPreview.GetAssetPreview(allPrefabs[i]));
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
        currentZOrder = EditorGUILayout.IntField("Zorder", currentZOrder);

        snapping = EditorGUILayout.Toggle(new GUIContent("Snapping", "Should tiles snap to the grid"), snapping);
        overWrite = EditorGUILayout.Toggle(new GUIContent("OverWrite", "Do you want to overwrite tile in the same layer and position"), overWrite);
        _showConsole = EditorGUILayout.Toggle(new GUIContent("Show in Console", "Show Whats happening on the console"), _showConsole);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(Instance, "Name");
        }
        EditorGUILayout.Space();

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
        foreach (var item in FindObjectsOfType<Layer>())
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

            if (artPos == null)
            {
                if (gameObject.transform.localPosition != (Vector3)tilePos)
                {
                    continue;
                }
                if(gameObject.name == "gizmoCursor" || gameObject.name == "gizmoTile")
                {
                    continue;
                }

                if (gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<Layer>().priority == currentLayer)
                {
                    if (gameObject.transform.parent.parent == null)
                    {
                        return gameObject;
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

    private void ActivateTools(bool active)
    {
        Tools.current = Tool.None;
        if (gizmoTile != null)
            gizmoTile.SetActive(active);
        if (gizmoCursor != null)
            gizmoCursor.SetActive(active);
    }

    private void InputMouse(ref Event currentEvent)
    {
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

        if (mouseDown && currentEvent.shift == false && areaInsertion == false)
        {
            if (snapping == false)
                mouseDown = false;

            AddTile(gizmoCursor.transform.position, currentLayer);
        }
        if (mouseDown && currentEvent.shift == true && areaInsertion == false && currentEvent.control == false)
        {
            areaInsertion = true;
            beginPos = gizmoCursor.transform.position;
        }

        //Draws Rectangle
        if (areaInsertion || areaDeletion)
        {
            DrawAreaRectangle();
            SceneView.RepaintAll();
        }
        
		//Cancel Area insertion if shift in released
		if (mouseDown && currentEvent.shift == false && areaInsertion == true)
			areaInsertion =false;

		//Starts AreaDeletion
		if (mouseDown && currentEvent.shift == true && areaDeletion==false && currentEvent.control==true) {
			areaDeletion = true;
			beginPos = gizmoTile.transform.position;
			ShowLog("StartedAreaDELETION");
		}


		//Deletes Elements in that area
		if (mouseDown == false && areaDeletion == true && currentEvent.shift && currentEvent.control) {
			ShowLog("AreaDELETION");
			AreaDeletion();
			areaDeletion = false;
		}

		//Intantiates elements in that area
		if(mouseDown==false && areaInsertion==true && currentEvent.shift && currentEvent.control==false)
		{
			
			AreaInsertion();
			areaInsertion=false;

		}

		//Removes single tile
		if (mouseDown&& currentEvent.control && areaDeletion==false) {
		
			RemoveTile();
		}
    }

    private void RemoveTile()
    {
        GameObject GOtoDelete = isObjectAt(new Vector3(gizmoCursor.transform.position.x, gizmoCursor.transform.position.y,
            currentLayer * layerDepthMultiplier), currentLayer);
        Undo.DestroyObjectImmediate(GOtoDelete);
        DestroyImmediate(GOtoDelete);
    }

    private void AreaInsertion()
    {
        Vector2 topLeft;
        Vector2 downRight;

        endPos = gizmoTile.transform.position;

        topLeft.y = endPos.y > beginPos.y ? endPos.y : beginPos.y;

        topLeft.x = endPos.x < beginPos.x ? beginPos.x : endPos.x;

        downRight.y = endPos.y > beginPos.y ? beginPos.y : endPos.y;

        downRight.x = endPos.x < beginPos.x ? endPos.x : beginPos.x;

        ShowLog(downRight);
        ShowLog(topLeft);
        for (float y = downRight.y; y <= topLeft.y; y++)
        {
            for (float x = downRight.x; x <= topLeft.x; x++)
            {

                GameObject go = isObjectAt(new Vector3(x, y, currentLayer * layerDepthMultiplier), currentLayer);

                //If there no object than create it
                if (go == null)
                {

                    InstantiateTile(new Vector3(x, y, layerDepthMultiplier), currentLayer);


                }//in this case there is go in there 
                else if (overWrite)
                {
                    Undo.DestroyObjectImmediate(go);
                    DestroyImmediate(go);

                    InstantiateTile(new Vector3(x, y), currentLayer);

                }
            }
        }
        ShowLog("Area Inserted");
    }

    private void AreaDeletion()
    {
        Vector2 topLeft;
        Vector2 downRight;

        endPos = gizmoTile.transform.position;

        topLeft.y = endPos.y > beginPos.y ? endPos.y : beginPos.y;

        topLeft.x = endPos.x < beginPos.x ? beginPos.x : endPos.x;

        downRight.y = endPos.y > beginPos.y ? beginPos.y : endPos.y;

        downRight.x = endPos.x < beginPos.x ? endPos.x : beginPos.x;

        ShowLog(downRight);
        ShowLog(topLeft);

        //Goes througt all units
        for (float y = downRight.y; y <= topLeft.y; y++)
        {

            for (float x = downRight.x; x <= topLeft.x; x++)
            {

                GameObject GOtoDelete = isObjectAt(new Vector3(x, y, currentLayer * layerDepthMultiplier), currentLayer);
                //If theres something then delete it
                if (GOtoDelete != null)
                {
                    Undo.DestroyObjectImmediate(GOtoDelete);
                    DestroyImmediate(GOtoDelete);
                }
            }
        }
        ShowLog("Area Deleted");
    }

    private void DrawAreaRectangle()
    {
        Vector4 area = GetAreaBounds();
        //topline
        Handles.DrawLine(new Vector3(area[3] + 0.5f, area[0] + 0.5f, 0), new Vector3(area[1] - 0.5f, area[0] + 0.5f, 0));
        //downline
        Handles.DrawLine(new Vector3(area[3] + 0.5f, area[2] - 0.5f, 0), new Vector3(area[1] - 0.5f, area[2] - 0.5f, 0));
        //leftline
        Handles.DrawLine(new Vector3(area[3] + 0.5f, area[0] + 0.5f, 0), new Vector3(area[3] + 0.5f, area[2] - 0.5f, 0));
        //rightline
        Handles.DrawLine(new Vector3(area[1] - 0.5f, area[0] + 0.5f, 0), new Vector3(area[1] - 0.5f, area[2] - 0.5f, 0));
    }
    private Vector4 GetAreaBounds()
    {
        Vector2 topLeft;
        Vector2 downRight;

        endPos = gizmoCursor.transform.position;

        topLeft.y = endPos.y > beginPos.y ? endPos.y : beginPos.y;

        topLeft.x = endPos.x < beginPos.x ? beginPos.x : endPos.x;

        downRight.y = endPos.y > beginPos.y ? beginPos.y : endPos.y;

        downRight.x = endPos.x < beginPos.x ? endPos.x : beginPos.x;

        return new Vector4(topLeft.y, downRight.x, downRight.y, topLeft.x);
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

        GameObject metaTile = (GameObject)PrefabUtility.InstantiatePrefab(_currentPrefab);
        metaTile.name = _currentPrefab.name;
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
    GameObject SpawnTile(UnityEngine.Object prefab , Vector2 pos)
    {
        GameObject metaTile = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        metaTile.name = prefab.name;
        metaTile.transform.SetParent(FindLayer(currentLayer).transform);
        metaTile.transform.localPosition = (Vector3)pos + metaTile.transform.InverseTransformVector(OffsetWeirdTiles());

        if (metaTile.transform.localPosition != (Vector3)pos)
        {
            ArtificialPosition artPos = metaTile.AddComponent<ArtificialPosition>();
            artPos.position = pos;
            artPos.offset = artPos.position - (Vector2)metaTile.transform.position;
            artPos.layer = currentLayer;
        }
        Undo.RegisterCreatedObjectUndo(metaTile, "Created go");
        return metaTile;
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
            foreach (Layer item in FindObjectsOfType<Layer>())
            {
                _layers.Add(item);
            }
        }
        foreach (Layer item in FindObjectsOfType<Layer>())
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

        ColorRecorrenciaSpriteRender(gizmoTile);
    }

    static void ColorRecorrenciaSpriteRender(GameObject gameObject)
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }

        foreach (Transform t in gameObject.transform)
        {
            ColorRecorrenciaSpriteRender(t.gameObject);
        }
    }
    static void ZorderRecorrenciaSpriteRender(GameObject gameObject)
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = currentZOrder;
        }

        foreach (Transform t in gameObject.transform)
        {
            ZorderRecorrenciaSpriteRender(t.gameObject);
        }
    }
    void ShowLog(object msg)
    {
        if (_showConsole)
        {
            Debug.Log(msg);
        }
    }
    [MenuItem("Window/MapTool/Increment Layer &d", false, 35)]
    static void IncrementLayer()
    { 
        if (Instance == null)
            return;
        currentLayer++;
        Undo.RecordObject(Instance, "Snapping");
    }
    [MenuItem("Window/MapTool/Decrement Layer &#d", false, 36)]
    static void DecrementLayer()
    {
        if (Instance == null)
            return;
        currentLayer--;
        Undo.RecordObject(Instance, "Snapping");
    }
    [MenuItem("Window/MapTool/Increment Zorder &z", false, 37)]
    static void IncrementZorder()
    {
        if (Instance == null)
            return;
        currentZOrder++;
        Undo.RecordObject(Instance, "Snapping");
    }
    [MenuItem("Window/MapTool/Decrement Zorder &#z", false, 38)]
    static void DecrementZorder()
    {
        if (Instance == null)
            return;
        currentZOrder--;
        Undo.RecordObject(Instance, "Snapping");
    }
}
