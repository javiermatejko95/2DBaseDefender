using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTrail : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float speed = 30f;
    #endregion

    #region PRIVATE_FIELDS
    private Vector3 targetPos = new();

    private float travelTime = 0f;

    private bool isInitialized = false;
    #endregion

    #region INIT
    public void Initialize(Vector3 targetPos)
    {
        this.targetPos = targetPos;

        isInitialized = true;
    }
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if(!isInitialized)
        {
            return;
        }
        Move();
    }
    #endregion

    #region PUBLIC_METHODS

    #endregion

    #region PRIVATE_METHODS
    private void Move()
    {
        travelTime += speed * Time.deltaTime;
        travelTime = Mathf.Clamp01(travelTime);

        transform.position = Vector3.Lerp(transform.position, targetPos, travelTime);

        if(travelTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
