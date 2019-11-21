using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    private GameObject _attackBox;
    private BaseUnit _movement;

    // HACK : 절대 바꿔
    [SerializeField]
    private GameObject _jingongcham;

    private void Start()
    {
        _attackBox = transform.GetChild(0).gameObject;
        _attackBox.SetActive(false);
        _movement = transform.root.GetComponent<BaseUnit>();
    }

    private void Update()
    {
    }

    public void OnAttackBox()
    {
        if (!_movement.IsAttackable())
            return;

        _attackBox.SetActive(true);
    }

    public void OffAttackBox()
    {
        _attackBox.SetActive(false);
    }

    public void OnSkill()
    {
        // TODO : (안병욱) 오브젝트 풀로 바꿔야됨
        GameObject skill = Instantiate(_jingongcham).gameObject;
        skill.transform.position = transform.root.position;
        Vector3 skillPos = skill.transform.position;
        skillPos.y += 2.0f;
        skill.transform.position = skillPos;

        Vector3 skillScale = skill.transform.localScale;
        skillScale.x *= _movement.Forward;
        skill.transform.localScale = skillScale;

    }
}
