using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ARROW
{
    UP = 0,
    DOWN,
    LEFT,
    RIGHT
}

public class Potal : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public ARROW arrow;
    public GameObject crossPotal;

    public bool isPotalBlock;

    const int UpRange = 0;
    const int DownRange = 2;
    const int LeftRange = 1;
    const int RightRange = 1;

    public Potal()
    {
        isPotalBlock = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPotalBlock)
        {
            TranslatePlayer(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isPotalBlock = false;
    }


    private void TranslatePlayer(GameObject player)
    {
        Vector3 playerVector = player.transform.position;

        switch (crossPotal.GetComponent<Potal>().arrow)
        {
            case ARROW.UP:
                {
                    playerVector.y = crossPotal.transform.position.y + UpRange;
                }
                break;
            case ARROW.DOWN:
                {
                    playerVector.y = crossPotal.transform.position.y - DownRange;
                }
                break;
            case ARROW.LEFT:
                {
                    playerVector.x = crossPotal.transform.position.x - LeftRange;
                }
                break;
            case ARROW.RIGHT:
                {
                    playerVector.x = crossPotal.transform.position.x + RightRange;
                }
                break;
        }


        player.transform.position = playerVector;
        crossPotal.GetComponent<Potal>().isPotalBlock = true;
    }

}
