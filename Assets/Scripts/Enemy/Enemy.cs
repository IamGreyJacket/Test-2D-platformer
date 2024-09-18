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

    [SerializeField, Min(0f), Tooltip("Attacks per second")]
    private float _attackSpeed = 1f;
    [SerializeField, Min(0f)]
    private float _attackDelay = 1f; //seconds
    private bool _canAttack = true;

    private void Awake()
    {
        _enemyController.AttackEvent += Attack;
        _enemyController.AltAttackEvent += AltAttack;
    }

    private void OnDestroy()
    {
        _enemyController.AttackEvent -= Attack;
        _enemyController.AltAttackEvent -= AltAttack;
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

    //synchronizes animation speed with set attack speed;
    private void CheckAnimSpeed()
    {
        if (_attackSpeed != _enemyAnimator.AttackSpeed) _enemyAnimator.AttackSpeed = _attackSpeed;
    }

    //Plays the animation of attack. Collider of a weapon is turned on and off with Unity Animation Events
    public void Attack()
    {
        if (_canAttack)
        {
            _enemyAnimator.PlayAttack();
            StartCoroutine(AttackCooldown());
        }
    }
    public void AltAttack()
    {
        if (_canAttack)
        {
            _enemyAnimator.PlayAltAttack();
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
