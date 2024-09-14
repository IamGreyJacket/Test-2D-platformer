using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected int _health = 3;
    public int Health => _health;

    public virtual void TakeDamage(int damage)
    {
        _health -= damage;
#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} took {damage} damage.");
#endif
    }
}
