using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _heart;
    public GameObject Heart => _heart;
    [SerializeField]
    private GameObject _heartOutline;
    public GameObject HeartOutline => _heartOutline;
}
