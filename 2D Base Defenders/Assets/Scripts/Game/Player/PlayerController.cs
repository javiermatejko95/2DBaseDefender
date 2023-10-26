using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private Rigidbody2D rigidbody2D = null;
    private Vector3 moveDirection = new Vector3();

    private bool canMove = false;

    private Animator anim = null;
    #endregion

    #region INIT
    public void Init()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;

        PauseManager.instance.AddCallbackOnPause(SetCanMove);
    }
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if(!canMove)
        {
            return;
        }

        moveDirection = GetMovement();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        rigidbody2D.velocity = moveDirection * 20f;
    }
    #endregion

    #region PRIVATE_METHODS
    private Vector3 GetMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if(Input.GetKey(KeyCode.W))
        {
            moveY = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }

        bool isIdle = moveX == 0f && moveY == 0f;

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;

        anim.SetFloat("horizontalMovement", moveDir.x);
        anim.SetFloat("verticalMovement", moveDir.y);

        anim.SetBool("isMoving", !isIdle);

        return moveDir;
    }

    private void SetCanMove(bool state)
    {
        canMove = !state;

        if(!canMove)
        {
            rigidbody2D.velocity = new Vector2(0f, 0f);
            moveDirection = new Vector2(0f, 0f);
        }
    }
    #endregion
}
