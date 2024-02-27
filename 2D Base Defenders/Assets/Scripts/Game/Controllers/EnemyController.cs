using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float distanceToTarget = 10f;
    [Header("Config")]
    [SerializeField] private Timer attackTimer = null;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int coinsDropAmount = 10;

    [SerializeField] private AudioClip impactClip = null;

    [Space, Header("Animation")]
    [SerializeField] private string triggerAttackName = "attack";
    [SerializeField] private string triggerRunName = "run";

    private Rigidbody2D rigidbody2D = null;
    private Animator animator = null;

    private Vector3 moveDirection = new Vector3();
    private Transform targetPos = null;
    private Barricade barricade = null;

    private bool canMove = false;
    private bool canAttack = false;

    private int healthAmount = 100;

    private Action onDeath = null;

    private void FixedUpdate()
    {
        Move();
    }

    public void Initialize(Action onDeath, Barricade barricade)
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        this.onDeath = onDeath;

        this.barricade = barricade;

        targetPos = barricade.transform;

        moveDirection = new Vector3(-1f, 0f).normalized;

        canMove = true;

        attackTimer.Init(attackRate, Attack, () => animator.SetTrigger(triggerAttackName));

        healthAmount = maxHealth;

        PauseController.Instance.AddCallbackOnPause(ChangeState);
    }

    public void Toggle(bool status)
    {
        gameObject.SetActive(status);

        if(status)
        {
            animator.SetTrigger(triggerRunName);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void TakeDamage(int damage)
    {
        AudioController.Instance.PlaySound(impactClip);

        healthAmount -= damage;

        if(healthAmount <= 0)
        {
            EconomyController.Instance.TotalCoins += coinsDropAmount;
            onDeath?.Invoke();
            ForceDestroy();
        }        
    }

    public void ForceDestroy()
    {
        PauseController.Instance.RemoveCallbackOnPause(ChangeState);
        Destroy(gameObject);
    }

    public void ChangeState(bool state)
    {
        canMove = !state;

        if(canAttack)
        {
            attackTimer.ToggleTimer(!state);
        }        

        if (!canMove)
        {
            rigidbody2D.velocity = new Vector2(0f, 0f);
        }
    }

    public void AttackBarricade()
    {
        barricade.TakeDamage(attackDamage);
    }

    private void Move()
    {
        if(!canMove)
        {
            return;
        }

        float distance = Mathf.Abs(transform.position.x - targetPos.position.x);

        if(distance <= distanceToTarget)
        {
            canMove = false;
            canAttack = true;
            rigidbody2D.velocity = new Vector2(0f, 0f);
            attackTimer.ToggleTimer(true);
        }
        else
        {
            rigidbody2D.velocity = moveDirection * moveSpeed;
        }
    }

    private void Attack()
    {
        attackTimer.ToggleTimer(canAttack);        
    }
}
