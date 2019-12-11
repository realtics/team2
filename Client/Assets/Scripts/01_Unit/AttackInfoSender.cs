using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttackInfoSender
{
    private Transform _attacker;

	[SerializeField] private float _damage;
    [SerializeField] private float _horizontalExtraMoveDuration;
    [SerializeField] private float _horizontalExtraMoveValue;
    [SerializeField] private float _verticalExtraMoveDuration;
    [SerializeField] private float _verticalExtraMoveValue;
    [SerializeField] private float _extraHeightValue;
    //[SerializeField] private float _extraHeightDuration;
    [SerializeField] private float _stunDuration;

    // poroperties
    public float Damage { get { return _damage; } set { _damage = value; } }
    public float HorizontalExtraMoveValue { get { return _horizontalExtraMoveValue; } set { _horizontalExtraMoveValue = value; } }
    public float HorizontalExtraMoveDuration { get { return _horizontalExtraMoveDuration; } set { _horizontalExtraMoveDuration = value; } }
    public float VerticalExtraMoveValue { get { return _verticalExtraMoveValue; } set { _verticalExtraMoveValue = value; } }
    public float VerticalExtraMoveDuration { get { return _verticalExtraMoveDuration; } set { _verticalExtraMoveDuration = value; } }
    public float ExtraHeightValue { get { return _extraHeightValue; } set { _extraHeightValue = value; } }
    //public float ExtraHeightDuration { get { return _extraHeightDuration; } set { _extraHeightDuration = value; } }
    public float StunDuration { get { return _stunDuration; } set { _stunDuration = value; } }

    public Transform Attacker { get { return _attacker; } set { _attacker = value; } }
}
