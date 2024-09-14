using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IAttack
{
    public void Attack()
    {
        //проигрывает анимацию атаки, в анимации проставл€ем Event-ы, чтобы включать и отключать коллайдер,
    }

    public IEnumerator AttackDelay(float delay)
    {
        yield return null;
    }
}
