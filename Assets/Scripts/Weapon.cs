using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Entity _owner;
    [SerializeField]
    protected int _damage = 1;
    [SerializeField]
    protected Collider2D _damagingCollider;
    public Collider2D DamagingCollider => _damagingCollider;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // проверить, можно ли нанести дамаг, если да, то ищем компонент Entity и наносим дамаг
        Entity entity;
        if(collision.gameObject.TryGetComponent<Entity>(out entity))
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

    public void TurnOffCollider()
    {
        _damagingCollider.enabled = false;
    }
    public void TurnOnCollider()
    {
        _damagingCollider.enabled = true;
    }
}
