using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoText = null;

    [Header("Config")]
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float reloadRate = 1f;
    [SerializeField] private LayerMask layersToIgnore;
    [SerializeField] private Timer reloadTimer = null;
    #endregion

    #region PRIVATE_FIELDS
    private int currentAmmo = 10;

    private bool isReloading = false;
    #endregion

    #region INIT
    public void Init()
    {
        reloadTimer.Init(reloadRate, Reload);

        currentAmmo = maxAmmo;

        UpdateAmmoText();

        PauseManager.instance.AddCallbackOnPause(ChangeState);
    }
    #endregion

    public void Shoot(Vector3 shootPosition, Vector3 shootDirection)
    {
        if(isReloading)
        {
            return;
        }

        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection, 100f, ~layersToIgnore);

        if(raycastHit2D.collider != null)
        {
            EnemyController enemyController = raycastHit2D.collider.GetComponent<EnemyController>();

            if(enemyController != null)
            {
                enemyController.TakeDamage();
            }
        }

        currentAmmo--;

        UpdateAmmoText();

        if(currentAmmo <= 0)
        {
            StartReloading();
        }        
    }

    public void StartReloading()
    {
        isReloading = true;
        reloadTimer.ToggleTimer(true);
    }
    #region PRIVATE_METHODS
    private void Reload()
    {
        currentAmmo = maxAmmo;

        UpdateAmmoText();

        isReloading = false;
    }

    private void UpdateAmmoText()
    {
        ammoText.text = currentAmmo + "/" + maxAmmo;
    }

    private void ChangeState(bool state)
    {
        if(isReloading)
        {
            reloadTimer.ToggleTimer(!state);
        }
    }
    #endregion
}
