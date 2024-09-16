using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : EntityController
{
    public event Action OnAttackEvent;
    public event Action OnAltAttackEvent;

    private void Update()
    {
        ChooseAttack();
    }

    private void ChooseAttack()
    {
        float random = UnityEngine.Random.Range(-1, 1);
        if (random >= 0) Attack();
        else AltAttack();
    }

    private void Attack()
    {
        OnAttackEvent?.Invoke();
    }

    private void AltAttack()
    {
        OnAltAttackEvent?.Invoke();
    }
}
