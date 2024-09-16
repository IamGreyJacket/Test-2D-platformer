using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityController
{
    private UserControls _controls;

    public event Action OnAttackEvent;
    public event Action OnAltAttackEvent;
    public event Action OnJumpEvent;


    private void Awake()
    {
        _controls = new UserControls();
        //"�������������" �� ������ UserControls
        _controls.PlayerOnFoot.Attack.performed += OnAttack;
        _controls.PlayerOnFoot.AltAttack.performed += OnAltAttack;
        _controls.PlayerOnFoot.Jump.started += OnJump;
    }

    private void OnAltAttack(InputAction.CallbackContext obj)
    {
        OnAltAttackEvent?.Invoke();
    }

    private void Update()
    {
        Movement = _controls.PlayerOnFoot.Movement.ReadValue<float>();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        OnJumpEvent?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        OnAttackEvent?.Invoke();
    }

    private void OnEnable()
    {
        //�������� UserControls
        _controls.Enable();
    }

    private void OnDisable()
    {
        //��������� UserControls
        _controls.Disable();
    }

    private void OnDestroy()
    {
        //"������������" �� ������� � UserControls
        _controls.PlayerOnFoot.Attack.performed -= OnAttack;
        _controls.PlayerOnFoot.AltAttack.performed -= OnAltAttack;
        _controls.PlayerOnFoot.Jump.started -= OnJump;
        //���������� UserControls
        _controls.Dispose();
    }
}
