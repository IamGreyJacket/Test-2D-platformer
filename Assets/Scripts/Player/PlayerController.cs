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
        //"�������������" �� ������ UserControls
        _controls.PlayerOnFoot.Attack.performed += Attack;
        _controls.PlayerOnFoot.AltAttack.performed += AltAttack;
        _controls.PlayerOnFoot.Jump.started += Jump;
    }

    private void AltAttack(InputAction.CallbackContext obj)
    {
        AltAttackEvent?.Invoke();
    }

    private void Update()
    {
        Movement = _controls.PlayerOnFoot.Movement.ReadValue<float>();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        JumpEvent?.Invoke();
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        AttackEvent?.Invoke();
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
        _controls.PlayerOnFoot.Attack.performed -= Attack;
        _controls.PlayerOnFoot.AltAttack.performed -= AltAttack;
        _controls.PlayerOnFoot.Jump.started -= Jump;
        //���������� UserControls
        _controls.Dispose();
    }
}
