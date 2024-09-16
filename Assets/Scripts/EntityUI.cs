using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityUI : MonoBehaviour
{
    [SerializeField]
    private Entity _owner;
    [SerializeField]
    private HealthUI[] _playerHeatlh;
    private int _liveHeartCount;

    private void Awake()
    { 
        _liveHeartCount = _owner.Health;
#if UNITY_EDITOR
        if (_playerHeatlh.Length != _liveHeartCount) Debug.LogError($"Entity Health ({_liveHeartCount}) and PlayerHealth quantity ({_playerHeatlh.Length}) are not matching each other!");
#endif
        _owner.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth)
    {
        if(currentHealth < _liveHeartCount)
        {
            for(int i = currentHealth; i < _liveHeartCount; i++)
            {
                _playerHeatlh[i].Heart.SetActive(false);
            }
        }
        else
        {
            for (int i = _liveHeartCount; i < currentHealth; i++)
            {
                _playerHeatlh[i].Heart.SetActive(true);
            }
        }
        _liveHeartCount = currentHealth;
    }

}
