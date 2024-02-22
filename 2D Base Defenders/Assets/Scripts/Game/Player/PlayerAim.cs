using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Transform aimParent = null;
    [SerializeField] private float clampAngle = 75f;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        float clampedAngle = Mathf.Clamp(angle, -clampAngle, clampAngle);  
        aimParent.eulerAngles = new Vector3(0f, 0f, clampedAngle);
    }
    #endregion

    #region PRIVATE_METHODS
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    private Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    #endregion
}
