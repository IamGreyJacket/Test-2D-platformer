using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField, Min(0)]
    protected int _damage = 1;
    //Collider must have isTrigger = true;
    [SerializeField]
    protected Collider2D _damagingCollider;
    public Collider2D DamagingCollider => _damagingCollider;

    //used in Unity Animation with UnityAnimationEvent, but also can be used in scripts
    public void TurnOffCollider()
    {
        _damagingCollider.enabled = false;
    }
    public void TurnOnCollider()
    {
        _damagingCollider.enabled = true;
    }
}
