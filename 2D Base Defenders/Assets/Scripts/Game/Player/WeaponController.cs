using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Gun gun = null;
    #endregion

    #region PRIVATE_FIELDS
    private bool canShoot = false;
    #endregion

    #region INIT
    public void Init()
    {
        gun.Init();
        canShoot = true;

        PauseManager.instance.AddCallbackOnPause(ChangeState);
    }
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if(!canShoot)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0f;

            Vector3 direction = (targetPos - transform.position).normalized;

            gun.Shoot(transform.position, direction);
            DrawLine(transform.position, targetPos);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            gun.StartReloading();
        }
    }
    #endregion

    #region PUBLIC_METHODS

    #endregion

    #region PRIVATE_METHODS
    private float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    private void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        Debug.DrawLine(startPos, endPos, Color.white, 0.1f);
    }

    private void ChangeState(bool state)
    {
        canShoot = !state;
    }
    #endregion
}
