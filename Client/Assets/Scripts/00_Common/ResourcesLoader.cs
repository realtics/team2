using UnityEngine;
using System.Collections;

public enum ResourcesIndex
{
    StoneBar,
    Poatl,
    Tile
}

public class ResourcesLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject StoneBar;
    [SerializeField]
    private GameObject Poatl;
    [SerializeField]
    private GameObject Tile;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
