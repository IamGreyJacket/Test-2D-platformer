using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : EntityUI
{
    [SerializeField]
    private Player _owner;
    [SerializeField, Min(0), Tooltip("Blinks per second")]
    private float _hangBlinksSpeed = 1f;
    private bool _healthHanging = false;
    

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

    private void OnHealthHang(bool isHanging)
    {
        _healthHanging = isHanging;
        if (_healthHanging) StartCoroutine(HangHealth());
        
    }

    private IEnumerator HangHealth()
    {
        float blinkTime = 1 / _hangBlinksSpeed;
        bool heartStatus = true;
        while (_healthHanging)
        {
            heartStatus = !heartStatus;
            _healthUI[_liveHeartCount - 1].Heart.SetActive(heartStatus);
            yield return new WaitForSeconds(blinkTime / 2);
        }
        _healthUI[_liveHeartCount - 1].Heart.SetActive(true);
        yield return null;
    }
}
