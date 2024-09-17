using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    [SerializeField]
    private Enemy _owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������, ����� �� ������� �����, ���� ��, �� ���� ��������� Entity � ������� �����
        Entity entity;
        if (collision.gameObject.TryGetComponent<Entity>(out entity))
        {
            if (entity != _owner)
            {
                entity.TakeDamage(_damage);
#if UNITY_EDITOR
                Debug.Log($"Hit {collision.name} Health = {entity?.Health}");
#endif
            }
        }
    }
}
