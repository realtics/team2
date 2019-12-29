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

    public void LoadScene(int Scene)
    {
        if(Scene == (int)SceneIndex.Lobby)
        {
            SpawnManager.Instance.ClearListdungeonObject();
        }
        else if (Scene == (int)SceneIndex.LobbySingle)
        {
            SpawnManager.Instance.ClearListdungeonObject();
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
        DungeonGameManager.Instance.FadeOut();
        SpawnManager.Instance.RoomSetActive(false, _currentDungeonIndex);
        DungeonInfo dungeonInfo = MapLoader.Instance.GetDungeonInfo(index);
        SpawnManager.Instance.Spawn(dungeonInfo);
        MapLoader.Instance.DeleteAfterInstantiateMonster(_currentDungeonIndex);
        _currentDungeonIndex = index;

        MoveRoomPotalActive();

        MiniMapManager.instance.movePlayerCursor(dungeonInfo.position);

        DungeonGameManager.Instance.FindCameraCollider();
        DungeonGameManager.Instance.MoveToPlayer(PotalManager.instance.FindGetArrowPotalPosition(FlipArrow(arrow)));
    }
    public void Loader()
    {
        DungeonGameManager.Instance.FadeOut();
        MapLoader.Instance.LoaderDungeon();
        DungeonInfo dungeonInfo = MapLoader.Instance.GetDungeonInfo(_startDungeonIndex);

        SpawnManager.Instance.Spawn(dungeonInfo);
        _currentDungeonIndex = _startDungeonIndex;

        MoveRoomPotalActive();

        UIHelper.Instance.SetDungeonTitle(MapLoader.Instance.GetDungeonName());
        DungeonGameManager.Instance.FindCameraCollider();
        DungeonGameManager.Instance.MoveToPlayer(dungeonInfo.PlayerStartPosition);
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

}
