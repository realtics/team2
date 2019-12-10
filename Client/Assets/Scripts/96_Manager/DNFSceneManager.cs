using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum SceneIndex
{
    MainMenu,
    Lobby,
    Dungen,
	LobbySingle,
}

public class DNFSceneManager : MonoBehaviour
{
    private static DNFSceneManager _instacne;

    public static DNFSceneManager instacne
    {
        get
        {
            return _instacne;
        }
    }
    private int _currentDungeonIndex;
    private const int _startDungeonIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        _instacne = this;
    }

    public void LoadScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void ChangeRoom(int index, ARROW arrow)
    {
        GameManager gameManager = GetGameManager();
        gameManager.FadeOut();

        SpawnManager.instacne.RoomSetActive(false, _currentDungeonIndex);
        DungeonInfo dungeonInfo = MapLoader.instacne.GetDungeonInfo(index);
        SpawnManager.instacne.Instantiate(dungeonInfo);
        MapLoader.instacne.AfterInstantiateMonsterDelete(_currentDungeonIndex);
        _currentDungeonIndex = index;

        //FIXME : 리팩토링
        PotalManager potalManager = PotalManager.instance;
        potalManager.FIndPotals();

        if (MonsterManager.Instance.IsExistMonster())
            potalManager.BlockPotals();
        else
            potalManager.ResetPotals();

        MiniMapManager.instance.movePlayerCursor(dungeonInfo.position);

        gameManager.FindCameraCollider();
        gameManager.MoveToPlayer(potalManager.FindGetArrowPotalPosition(FlipArrow(arrow)));
    }
    public void Loader()
    {
        GameManager gameManager = GetGameManager();
        gameManager.FadeOut();

        MapLoader.instacne.LoaderDungeon();
        DungeonInfo dungeonInfo = MapLoader.instacne.GetDungeonInfo(_startDungeonIndex);

        _currentDungeonIndex = _startDungeonIndex;
        SpawnManager.instacne.Instantiate(dungeonInfo);

        //FIXME : 리팩토링
        PotalManager potalManager = PotalManager.instance;
        potalManager.FIndPotals();

        if (MonsterManager.Instance.IsExistMonster())
            potalManager.BlockPotals();
        else
            potalManager.ResetPotals();

        gameManager.FindCameraCollider();
        gameManager.MoveToPlayer(dungeonInfo.PlayerStartPosition);
    }

    private ARROW FlipArrow(ARROW arrow)
    {
        switch (arrow)
        {
            case ARROW.UP:
                {
                    return ARROW.DOWN;
                }
            case ARROW.DOWN:
                {
                    return ARROW.UP;
                }
            case ARROW.LEFT:
                {
                    return ARROW.RIGHT;
                }
            case ARROW.RIGHT:
                {
                    return ARROW.LEFT;
                }
            default:
                {
                    return ARROW.NULL;
                }
        }
    }
    private GameManager GetGameManager()
    {
        if (DungeonGameManager.Instance != null)
        {
            return DungeonGameManager.Instance;
        }
        else if (LobbyGameManager.Instance != null)
        {
            return LobbyGameManager.Instance;
        }
        return null;
    }
}
