using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : EntityUI
{
    [SerializeField]
    private Enemy _owner;

    private void Awake()
    {
        SetUpHealth(_owner);
    }

    private void OnDestroy()
    {
        BeforeDestroy();
    }

    private void BeforeDestroy()
    {
        _owner.HealthChanged -= OnHealthChanged;
    }
}
