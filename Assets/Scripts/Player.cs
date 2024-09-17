using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity, IAttack
{
    /// <summary>
    /// if true - health is hanging, if false - health is NOT hanging
    /// </summary>
    public event Action<bool> HealthHangEvent;

    [Space, SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [Space, SerializeField, Min(0f)]
    private float _moveSpeed = 3f;
    [SerializeField, Min(0f)]
    private float _jumpForce = 400f;
    [SerializeField, Min(0f)]
    private float _wallSlideSpeed = 1f;
    private bool _canJump = true;
    private bool _isWallSlide;

    [SerializeField, Min(0f), Tooltip("Attacks per second")]
    private float _attackSpeed = 1f;
    [SerializeField, Min(0f)]
    private float _attackDelay = 1f;
    private bool _canAttack = true;

    [SerializeField, Min(0f)]
    private float _healthHangTime = 3f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y >= -0.01f)
            {
                _canJump = true;
                if (contact.normal.x > 0.7f || contact.normal.x < -0.7f) //todo
                {
                    BeginWallSlide();
                }
            }
        }
        //если contact.normal.y меньше 0, то давать прыжок нельзя
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StopWallSlide();
    }

    private void BeginWallSlide()
    {
        _isWallSlide = true;
        StartCoroutine(WallSlide());
    }

    private void StopWallSlide()
    {
        StopCoroutine(WallSlide());
        _isWallSlide = false;
    }

    private IEnumerator WallSlide()
    {
        while (_isWallSlide)
        {
            _rigidbody.velocity = Vector2.down * _wallSlideSpeed;
            yield return null;
        }
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
        if (_healthHanging)
        {
            _healthHanging = false;
            await Task.Yield();
            base.TakeDamage(damage + 1);
            return;
        }
        var hangComplete = await HangHealth();
        if(hangComplete) base.TakeDamage(damage);
    }

    public void OnHitSuccess()
    {
        _healthHanging = false;
    }

    /// <summary>
    /// If timer was not broken - returns true, if something interrupted the timer - returns false
    /// </summary>
    /// <returns></returns>
    public async Task<bool> HangHealth()
    {
        _healthHanging = true;
        float time = _healthHangTime;
        HealthHangEvent?.Invoke(true);
        while (time > 0)
        {
            if (_healthHanging == false)
            {
                HealthHangEvent?.Invoke(false);
                return false;
            }
            time -= Time.deltaTime;
            await Task.Yield();
        }
        //Таймер подвешенного ХП
        _healthHanging = false;
        HealthHangEvent?.Invoke(false);
        return true;
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
        if (!_canJump) return;
        StopWallSlide();
        _rigidbody.velocity = new Vector2(0f, 0f);
        _rigidbody.AddForce(Vector2.up * _jumpForce);
        _canJump = false;
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
