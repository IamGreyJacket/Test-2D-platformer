using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    [SerializeField]
    private Player _owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity;
        if (collision.gameObject.TryGetComponent<Entity>(out entity))
        {
            if (entity != _owner)
            {
                if (_owner.HealthHanging) entity.TakeDamage(_damage + 1);
                else entity.TakeDamage(_damage);
                OnHitSuccess();
#if UNITY_EDITOR
                Debug.Log($"{_owner.gameObject.name} hit {collision.name}. Health = {entity?.Health}");
#endif
            }
        }
    }

    private void OnHitSuccess()
    {
        _owner.OnHitSuccess();
    }
}
