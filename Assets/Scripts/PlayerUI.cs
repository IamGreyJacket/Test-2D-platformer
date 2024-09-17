using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : EntityUI
{
    [SerializeField]
    private Player _owner;

    private void Awake()
    {
        SetUpHealth(_owner);
    }

    private void OnDestroy()
    {
        BeforeDestroy();
    }

    protected override void SetUpHealth(Entity owner)
    {
        base.SetUpHealth(owner);
        _owner.HealthHangEvent += OnHealthHang;
    }

    private void BeforeDestroy()
    {
        _owner.HealthChanged -= OnHealthChanged;
        _owner.HealthHangEvent -= OnHealthHang;
    }

    private void OnHealthHang()
    {
        //реализация моргающих сердечек
        
    }
}
