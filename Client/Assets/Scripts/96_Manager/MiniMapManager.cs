using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tilePrefab;
    private float _tileSize;

    private int maxNum;
    private DungeonJsonData dungeonData;

    // Use this for initialization
    void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        dungeonData = MapLoader.instacne.dungeonData;
        maxNum = dungeonData.DungeonInfos.Length;
        Transform parent = UIHelper.Instance.miniMap.gameObject.transform;

        _tileSize = _tilePrefab.GetComponent<Image>().rectTransform.rect.width;

        for (int i = 0; i < maxNum; i++)
        {
            GameObject @object = Instantiate(_tilePrefab, parent);
            @object.transform.position = new Vector3(parent.transform.localPosition.x - i * _tileSize, parent.transform.localPosition.y, 0);
            //RectTransform rectTransform = (RectTransform)@object.transform.localPosition;
            //rectTransform.localPosition

        }


    }

}
