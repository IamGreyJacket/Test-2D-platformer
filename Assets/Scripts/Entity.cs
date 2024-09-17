using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action<int> HealthChanged;

    [SerializeField, Min(1)]
    protected int _health = 3;
    public int Health
    {
        get => _health;
        protected set
        {
            _health = value;
            OnHealthChanged();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} took {damage} damage.");
#endif
        if (Health <= 0) Die();
    }

    protected void OnHealthChanged()
    {
        HealthChanged?.Invoke(Health);
    }

    public virtual void Die()
    {
#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} died!");
#endif
        Destroy(this.gameObject);
    }
}
