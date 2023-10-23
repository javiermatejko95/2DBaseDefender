using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Rigidbody2D rigidbody2D = null;
    [SerializeField] private float moveSpeed = 100f;
    #endregion

    #region PRIVATE_FIELDS
    private Vector3 direction = new Vector3();
    #endregion

    #region PUBLIC_METHODS
    public void Init(Vector3 direction)
    {
        this.direction = direction;

        rigidbody2D.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
        transform.eulerAngles = new Vector3(0f, 0f, GetAngleFromVectorFloat(direction));
        Destroy(gameObject, 5f);
    }
    #endregion

    #region PRIVATE_METHODS
    private float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
    #endregion

    #region COLLISION
    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyController enemyController = collider.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage();
            Destroy(gameObject);
        }
    }
    #endregion
}
