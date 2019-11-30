using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instacne;
    public static SpawnManager instacne
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
    public void DungeonObjectInstantiate()
    {

    }
}
