using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IAttack
{
    public void Attack()
    {
        //����������� �������� �����, � �������� ����������� Event-�, ����� �������� � ��������� ���������,
    }

    public IEnumerator AttackDelay(float delay)
    {
        yield return null;
    }
}
