using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private Rigidbody2D rigidbody2D = null;
    private Vector3 moveDirection = new Vector3();

    private bool canMove = false;
    #endregion

    #region INIT
    public void Init()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        canMove = true;

        PauseManager.instance.AddCallbackOnChangeState(SetCanMove);
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

        return new Vector3(moveX, moveY).normalized;
    }

    private void SetCanMove(bool state)
    {
        canMove = !state;

        if(!canMove)
        {
            moveDirection = new Vector2(0f, 0f);
        }
    }
    #endregion
}
