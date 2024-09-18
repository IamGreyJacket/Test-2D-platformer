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

    //Sets the owner and sets the method to wait for health hang
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

    //if true - starts health hang in UI, if false - stops health hang in UI
    private void OnHealthHang(bool isHanging)
    {
        _healthHanging = isHanging;
        if (_healthHanging) StartCoroutine(HangHealth());
    }

    //Hanged health start blinking
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
        _healthUI[_liveHeartCount - 1].Heart.SetActive(true); //we keep it ON, in case we hit an enemy and did NOT lose our health.
        //If we did not hit anyone, Entity.TakeDamage() => OnHealthChanged will turn off needed HP icons anyway
        yield return null;
    }
}
