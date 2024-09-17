using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityUI : MonoBehaviour
{
    [SerializeField]
    protected HealthUI[] _healthUI;
    protected int _liveHeartCount;

    protected virtual void SetUpHealth(Entity owner)
    {
        _liveHeartCount = owner.Health;
#if UNITY_EDITOR
        if (_healthUI.Length != _liveHeartCount) Debug.LogError($"Entity ({owner.gameObject.name}) Health ({_liveHeartCount}) and HealthUI quantity ({_healthUI.Length}) are not matching each other!");
#endif
        owner.HealthChanged += OnHealthChanged;
    }

    protected virtual void OnHealthChanged(int currentHealth)
    {
        if(currentHealth < _liveHeartCount)
        {
            if (currentHealth < 0) currentHealth = 0;
            for(int i = currentHealth; i < _liveHeartCount; i++)
            {
                _healthUI[i].Heart.SetActive(false);
            }
        }
        else
        {
            for (int i = _liveHeartCount; i < currentHealth; i++)
            {
                _healthUI[i].Heart.SetActive(true);
            }
        }
        _liveHeartCount = currentHealth;
    }

}
