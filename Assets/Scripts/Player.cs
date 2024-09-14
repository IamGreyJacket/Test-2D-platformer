using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity, IAttack
{
    [Space, SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private PlayerAnimator _playerAnimator;

    [SerializeField]
    private float _attackDelay = 1f;

    private void Awake()
    {
        _playerController.OnAttackEvent += OnAttack;
        _playerController.OnAltAttackEvent += OnAltAttack;
    }

    private void OnDestroy()
    {
        _playerController.OnAttackEvent -= OnAttack;
        _playerController.OnAltAttackEvent -= OnAltAttack;
    }

    private void OnAttack()
    {
        Attack();
    }

    private void OnAltAttack()
    {
        AltAttack();
    }

    public IEnumerator HangHealth()
    {
        yield return null;
    }
    public override async void TakeDamage(int damage)
    {

    }

    public IEnumerator OnHealthHung()
    {
        yield return null;
    }



    public void Attack()
    {
        //проигрывает анимацию атаки, в анимации проставл€ем Event-ы, чтобы включать и отключать коллайдер,
        _playerAnimator.Attack();
    }

    public void AltAttack()
    {
        //проигрывает анимацию атаки, в анимации проставл€ем Event-ы, чтобы включать и отключать коллайдер,
        _playerAnimator.AltAttack();
    }

    public IEnumerator AttackDelay(float delay)
    {
        yield return null;
    }
}
