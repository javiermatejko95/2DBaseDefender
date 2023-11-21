using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float distanceToTarget = 10f;
    [Header("Config")]
    [SerializeField] private Timer attackTimer = null;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int attackDamage = 10;
    #endregion

    #region PRIVATE_FIELDS
    private Rigidbody2D rigidbody2D = null;
    private Animator anim = null;

    private Vector3 moveDirection = new Vector3();
    private Transform targetPos = null;
    private Barricade barricade = null;

    private bool canMove = false;
    private bool canAttack = false;

    private Action onDeath = null;
    #endregion

    #region PROPERTIES

    #endregion

    #region UNITY_CALLS
    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region INIT
    public void Initialize(Action onDeath, Barricade barricade)
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        this.onDeath = onDeath;

        this.barricade = barricade;

        targetPos = barricade.transform;

        moveDirection = new Vector3(-1f, 0f).normalized;

        canMove = true;

        attackTimer.Init(attackRate, Attack);

        PauseManager.instance.AddCallbackOnPause(ChangeState);
    }
    #endregion

    #region PUBLIC_METHODS
    public void Toggle(bool status)
    {
        gameObject.SetActive(status);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void TakeDamage()
    {
        onDeath?.Invoke();
        ForceDestroy();
    }

    public void ForceDestroy()
    {
        PauseManager.instance.RemoveCallbackOnPause(ChangeState);
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
    #endregion

    #region PRIVATE_METHODS
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

        barricade.TakeDamage(attackDamage);
    }    
    #endregion
}
