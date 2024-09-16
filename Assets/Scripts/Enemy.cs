using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IAttack
{
    [SerializeField]
    private EnemyController _enemyController;
    [SerializeField]
    private EnemyAnimator _enemyAnimator;

    public Entity Target;

    [SerializeField, Tooltip("Attacks per second")]
    private float _attackSpeed = 1f;
    [SerializeField]
    private float _attackDelay = 1f;
    private bool _canAttack = true;

    private void Awake()
    {
        _enemyController.OnAttackEvent += OnAttack;
        _enemyController.OnAltAttackEvent += OnAltAttack;
    }

    private void OnDestroy()
    {
        _enemyController.OnAttackEvent -= OnAttack;
        _enemyController.OnAltAttackEvent -= OnAltAttack;
    }

    private void Update()
    {
        LookAtTarget();
        CheckAnimSpeed();
    }

    private void LookAtTarget()
    {
        if(Target != null)
        {
            var targetDirection = Target.transform.position - gameObject.transform.position;
            var rot = transform.eulerAngles;
            if(targetDirection.x < 0f)
            {
                rot.y = -180;
            }
            else if (targetDirection.x > 0f)
            {
                rot.y = 0;
            }
            transform.eulerAngles = rot;
        }
    }

    private void CheckAnimSpeed()
    {
        if (_attackSpeed != _enemyAnimator.AttackSpeed) _enemyAnimator.AttackSpeed = _attackSpeed;
    }


    private void OnAttack()
    {
        Attack();
    }

    private void OnAltAttack()
    {
        AltAttack();
    }

    public void Attack()
    {
        if (_canAttack)
        {
            _enemyAnimator.PlayAttack();
            StartCoroutine(AttackDelay());
        }
    }

    public void AltAttack()
    {
        if (_canAttack)
        {
            _enemyAnimator.PlayAltAttack();
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
