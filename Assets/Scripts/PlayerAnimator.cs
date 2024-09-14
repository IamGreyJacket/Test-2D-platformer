using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : EntityAnimator
{
    public void AltAttack()
    {
        _animator.Play("AltAttack");
    }

    public void Attack()
    {
        _animator.Play("Attack");
    }
}

