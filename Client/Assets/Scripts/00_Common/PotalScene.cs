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
        LobbyGameManager.Instance.MoveToScene(1);
        //MapLoader.instacne.Loader(_nextSceneName);
    }
}
