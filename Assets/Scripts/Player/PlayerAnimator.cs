using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    protected Animator _weaponAnimator;


    private float _attackSpeed = 1f;
    public float AttackSpeed
    {
        get
        {
            return _attackSpeed;
        }
        set
        {
            _weaponAnimator.speed = value;
            _attackSpeed = value;
        }
    }

    public void PlayAltAttack()
    {
        _weaponAnimator.Play("AltAttack");
    }

    public void PlayAttack()
    {
        _weaponAnimator.Play("Attack");
    }
}

