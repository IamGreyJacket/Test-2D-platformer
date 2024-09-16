using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityController
{
    private UserControls _controls;

    public event Action AttackEvent;
    public event Action AltAttackEvent;
    public event Action JumpEvent;


    private void Awake()
    {
        _controls = new UserControls();
        //"Подписываемся" на ивенты UserControls
        _controls.PlayerOnFoot.Attack.performed += OnAttack;
        _controls.PlayerOnFoot.AltAttack.performed += OnAltAttack;
        _controls.PlayerOnFoot.Jump.started += OnJump;
    }

    private void OnAltAttack(InputAction.CallbackContext obj)
    {
        AltAttackEvent?.Invoke();
    }

    private void Update()
    {
        Movement = _controls.PlayerOnFoot.Movement.ReadValue<float>();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        JumpEvent?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        AttackEvent?.Invoke();
    }

    private void OnEnable()
    {
        //Включаем UserControls
        _controls.Enable();
    }

    private void OnDisable()
    {
        //Выключаем UserControls
        _controls.Disable();
    }

    private void OnDestroy()
    {
        //"отписываемся" от ивентов в UserControls
        _controls.PlayerOnFoot.Attack.performed -= OnAttack;
        _controls.PlayerOnFoot.AltAttack.performed -= OnAltAttack;
        _controls.PlayerOnFoot.Jump.started -= OnJump;
        //уничтожаем UserControls
        _controls.Dispose();
    }
}
