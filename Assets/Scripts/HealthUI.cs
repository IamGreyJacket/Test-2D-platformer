using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _healthIcon;
    public GameObject HealthIcon => _healthIcon;
    [SerializeField]
    private GameObject _healthIconBackground;
    public GameObject HealthIconBackground => _healthIconBackground;
}
