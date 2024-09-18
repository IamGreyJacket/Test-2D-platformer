using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public abstract void Attack();
    public abstract void AltAttack();
    public abstract IEnumerator AttackCooldown();
}
