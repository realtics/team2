using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGoblin : Goblin
{
    [SerializeField]
    private HellItemDatabase _hellItemDatabase;

    protected override void Awake()
    {
        base.Awake();
        OnSuperArmor();
    }

    //DieState
    public override void EnterDieState()
    {
        StartCoroutine(TimeScaleSlow());
        EffectManager.Instance.SpawnClearCircle(HitBoxCenter);

        Item hellItem;
        if (NetworkManager.Instance.IsSingle)
        {
            hellItem = _hellItemDatabase.GetItemCopy(_hellItemDatabase.GetRandomItemID());
        }
        else
        {
            hellItem = _hellItemDatabase.GetItemCopyByNetId(NetworkInventoryInfoSaver.Instance.ItemID);
        }
       
        EffectManager.Instance.SpawnHellItem(HitBoxCenter, hellItem);

        base.EnterDieState();
    }

    public override void UpdateDieState()
    {
        base.UpdateDieState();
    }


    public override void ExitDieState()
    {
        base.ExitDieState();
    }

    public override void NoticeDie()
    {
        MonsterManager.Instance.ReceiveBossMonsterDie(this);
    }

    private IEnumerator TimeScaleSlow()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }
}
