using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum SceneIndex
{
    MainMenu,
    Lobby,
    Dungen,
	LobbySingle,
	Inventory
}

public class DNFSceneManager : Single.Singleton<DNFSceneManager>
{
    private int _currentDungeonIndex;
    private const int _startDungeonIndex = 0;

    // Use this for initialization

    public void LoadScene(int Scene)
    {
        if(Scene == (int)SceneIndex.Lobby)
        {
            SpawnManager.instance.ClearListdungeonObject();
        }
        SceneManager.LoadScene(Scene);
    }

	public void LoadSceneAddtive(int Scene)
	{
		SceneManager.LoadScene(Scene,LoadSceneMode.Additive);
	}

	public void UnLoadScene(int Scene)
	{
		SceneManager.UnloadSceneAsync(Scene);
	}

    public void ChangeRoom(int index, ARROW arrow)
    {
        GameManager gameManager = GetGameManager();
        gameManager.FadeOut();

        SpawnManager.instance.RoomSetActive(false, _currentDungeonIndex);
        DungeonInfo dungeonInfo = MapLoader.instance.GetDungeonInfo(index);
        SpawnManager.instance.Spawn(dungeonInfo);
        MapLoader.instance.DeleteAfterInstantiateMonster(_currentDungeonIndex);
        _currentDungeonIndex = index;

        MoveRoomPotalActive();

        MiniMapManager.instance.movePlayerCursor(dungeonInfo.position);

        gameManager.FindCameraCollider();
        gameManager.MoveToPlayer(PotalManager.instance.FindGetArrowPotalPosition(FlipArrow(arrow)));
    }
    public void Loader()
    {
        GameManager gameManager = GetGameManager();
        gameManager.FadeOut();

        MapLoader.instance.LoaderDungeon();
        DungeonInfo dungeonInfo = MapLoader.instance.GetDungeonInfo(_startDungeonIndex);

        SpawnManager.instance.Spawn(dungeonInfo);
        _currentDungeonIndex = _startDungeonIndex;

        MoveRoomPotalActive();

        gameManager.FindCameraCollider();
        gameManager.MoveToPlayer(dungeonInfo.PlayerStartPosition);
    }

    private void MoveRoomPotalActive()
    {
        PotalManager.instance.FIndPotals();

        if (MonsterManager.Instance.IsExistMonster())
            PotalManager.instance.BlockPotals();
        else
            PotalManager.instance.ResetPotals();
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
