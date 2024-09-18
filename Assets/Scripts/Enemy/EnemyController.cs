using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : EntityController
{
    public event Action AttackEvent;
    public event Action AltAttackEvent;

    private void Update()
    {
        ChooseAttack();
    }

    //since we have 2 of the attacks, i thought it'd be better to randomize it
    private void ChooseAttack()
    {
        float random = UnityEngine.Random.Range(-1, 1);
        if (random >= 0) Attack();
        else AltAttack();
    }

    private void Attack()
    {
        AttackEvent?.Invoke();
    }

    private void AltAttack()
    {
        AltAttackEvent?.Invoke();
    }
}
