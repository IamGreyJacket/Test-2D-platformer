using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityUI : MonoBehaviour
{
    [SerializeField]
    protected HealthUI[] _heatlhUI;
    protected int _liveHeartCount;

    protected virtual void SetUpHealth(Entity owner)
    {
        _liveHeartCount = owner.Health;
#if UNITY_EDITOR
        if (_heatlhUI.Length != _liveHeartCount) Debug.LogError($"Entity Health ({_liveHeartCount}) and PlayerHealth quantity ({_heatlhUI.Length}) are not matching each other!");
#endif
        owner.HealthChanged += OnHealthChanged;
    }

    protected virtual void OnHealthChanged(int currentHealth)
    {
        if(currentHealth < _liveHeartCount)
        {
            for(int i = currentHealth; i < _liveHeartCount; i++)
            {
                _heatlhUI[i].Heart.SetActive(false);
            }
        }
        else
        {
            for (int i = _liveHeartCount; i < currentHealth; i++)
            {
                _heatlhUI[i].Heart.SetActive(true);
            }
        }
        _liveHeartCount = currentHealth;
    }

}
