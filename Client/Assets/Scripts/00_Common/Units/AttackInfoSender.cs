using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoSender
{
    public AttackInfoSender(Transform attacker)
    {
        _attacker = attacker;
    }

    private Transform _attacker;

    // poroperties
    public float Damage { get; set; }
    public float HorizontalExtraMoveValue { get; set; }
    public float HorizontalExtraMoveDuration { get; set; }
    public float VerticalExtraMoveValue { get; set; }
    public float VerticalExtraMoveDuration { get; set; }
    public float StunDuration { get; set; }
    public Transform Attacker { get { return _attacker; } }
}
