using UnityEngine;
using System.Collections;

public class PotalScene : Potal
{
    [SerializeField]
    private string _nextSceneName;

    public string nextSceneName
    {
        get
        {
            return _nextSceneName;
        }
        set
        {
            _nextSceneName = value;
        }
    }
    public override void Enter()
    {
        MapLoader.instacne.SetMap(_nextSceneName);
        LobbyGameManager.Instance.MoveToScene((int)SceneIndex.Dungen);             
    }
    public override Vector3 GetPlayerSpotPosition()
    {
        return Vector3.zero;
    }
}
