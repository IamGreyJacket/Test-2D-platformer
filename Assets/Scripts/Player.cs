using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity, IAttack
{
    public event Action HealthHangEvent;

    [Space, SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [Space, SerializeField]
    private float _moveSpeed = 3f;
    [SerializeField]
    private float _jumpForce = 400f;

    [SerializeField, Tooltip("Attacks per second")]
    private float _attackSpeed = 1f;
    [SerializeField]
    private float _attackDelay = 1f;
    private bool _canAttack = true;

    private bool _healthHanging = false;
    public bool HealthHanging => _healthHanging;

    private void Awake()
    {
        _playerController.AttackEvent += OnAttack;
        _playerController.AltAttackEvent += OnAltAttack;
        _playerController.JumpEvent += OnJump;
    }

    private void OnDestroy()
    {
        _playerController.AttackEvent -= OnAttack;
        _playerController.AltAttackEvent -= OnAltAttack;
        _playerController.JumpEvent -= OnJump;
    }

    private void Update()
    {
        Move();
        CheckAnimSpeed();
    }

    private void CheckAnimSpeed()
    {
        if (_attackSpeed != _playerAnimator.AttackSpeed) _playerAnimator.AttackSpeed = _attackSpeed;
    }

    private void OnAttack()
    {
        Attack();
    }

    private void OnAltAttack()
    {
        AltAttack();
    }

    private void OnJump()
    {
        Jump();
    }

    public override async void TakeDamage(int damage)
    {

        base.TakeDamage(damage);
    }

    public async Task<bool> OnHealthHung()
    {

        //Таймер подвешенного ХП
        return false;
    }

    private void Move()
    {
        Vector3 moveVector = new Vector3(_moveSpeed, 0, 0) * _playerController.Movement * Time.deltaTime;
        var rot = transform.eulerAngles;
        if (moveVector.x < 0)
        {
            rot.y = -180;
        }
        else if(moveVector.x > 0)
        {
            rot.y = 0;
        }
        transform.eulerAngles = rot;
        transform.Translate(moveVector, Space.World);
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce);
    }

    public void Attack()
    {
        //проигрывает анимацию атаки, в анимации проставляем Event-ы, чтобы включать и отключать коллайдер,
        if (_canAttack)
        {
            _playerAnimator.PlayAttack();
            StartCoroutine(AttackDelay());
        }
    }

    public void AltAttack()
    {
        //проигрывает анимацию атаки, в анимации проставляем Event-ы, чтобы включать и отключать коллайдер,
        if (_canAttack)
        {
            _playerAnimator.PlayAltAttack();
            StartCoroutine(AttackDelay());
        }
    }

    public IEnumerator AttackDelay()
    {
        float delay = _attackDelay;
        _canAttack = false;
        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        _canAttack = true;
        yield return null;
    }
}
