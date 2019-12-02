using UnityEngine;
using System.Collections;

public class DungeonSelectObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIHelper.Instance.SetDungeonSelectMenu(true);
        }
    }
}
