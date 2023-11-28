using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTrail : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float speed = 1f;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
    #endregion
}
