using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum SceneIndex
{
    MainMenu,
    Lobby,
    Dungen
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
    // Use this for initialization
    void Start()
    {
        _instacne = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LoadScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
}
