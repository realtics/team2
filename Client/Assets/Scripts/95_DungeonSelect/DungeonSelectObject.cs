using UnityEngine;
using System.Collections;

public class DungeonSelectObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIHelper.Instance.SetDungeonSelectMenu(true);
    }
}
