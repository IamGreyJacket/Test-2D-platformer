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
    private float _attackDelay = 1f; //seconds
    private bool _canAttack = true;

    [SerializeField, Min(0f)]
    private float _healthHangTime = 3f; //seconds
    private bool _healthHanging = false;
    public bool HealthHanging => _healthHanging;

    private void Awake()
    {
        _playerController.AttackEvent += Attack;
        _playerController.AltAttackEvent += AltAttack;
        _playerController.JumpEvent += Jump;
    }

    private void OnDestroy()
    {
        _playerController.AttackEvent -= Attack;
        _playerController.AltAttackEvent -= AltAttack;
        _playerController.JumpEvent -= Jump;
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
            //to check, if we landed and did not hit a roof/ceiling. 
            if (contact.normal.y >= -0.01f)
            {
                _canJump = true;
                //checks if the surface is steep enough to begin wall slide;
                if (contact.normal.x > 0.7f || contact.normal.x < -0.7f) //todo
                {
                    BeginWallSlide();
                }
            }
        }
    }
    //when we jump off a wall or anything else, we should make sure we don't keep "sliding" (See IEnumerator WallSlide() to understand why)
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
    //we set a constant velocity to our player, to simulate that we're sliding off of a wall or steep surface
    private IEnumerator WallSlide() 
    {
        while (_isWallSlide)
        {
            _rigidbody.velocity = Vector2.down * _wallSlideSpeed;
            yield return null;
        }
    }

    //synchronizes animation speed with set attack speed;
    private void CheckAnimSpeed()
    {
        if (_attackSpeed != _playerAnimator.AttackSpeed) _playerAnimator.AttackSpeed = _attackSpeed;
    }

    //Starts hanging health and waits for hanging results. If hang timer is out, then takes damage.
    //if hit again, while hang timer is still going, then takes (damage + 1)
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

    //To nullify HangHealth. If we hit an enemy, while our health is hanging - we return it and don't take damage we received before
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
        _healthHanging = false;
        HealthHangEvent?.Invoke(false);
        return true;
    }

    //moves the player and also turns him into direction he's moving
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

    //Plays the animation of attack. Collider of a weapon is turned on and off with Unity Animation Events
    public void Attack()
    {
        if (_canAttack)
        {
            _playerAnimator.PlayAttack();
            StartCoroutine(AttackCooldown());
        }
    }
    public void AltAttack()
    {
        if (_canAttack)
        {
            _playerAnimator.PlayAltAttack();
            StartCoroutine(AttackCooldown());
        }
    }
    public IEnumerator AttackCooldown()
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
