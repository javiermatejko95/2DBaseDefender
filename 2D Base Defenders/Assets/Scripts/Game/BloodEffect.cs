using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    #region UNITY_CALLS
    private void Awake()
    {
        Destroy(gameObject, 1f);
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetPosition(Vector3 spawnPos)
    {
        transform.position = spawnPos;
    }
    #endregion
}
