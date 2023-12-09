using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Gun : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("Config")]
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float reloadRate = 1f;
    [SerializeField] private LayerMask layersToIgnore;
    [SerializeField] private Timer reloadTimer = null;
    [SerializeField] private AudioClip shootClip = null;
    [SerializeField] private AudioClip reloadClip = null;
    [SerializeField] private Transform shootingPos = null;
    [SerializeField] private float fireRate = 1f;

    [Space, Header("Animation")]
    [SerializeField] private Animator animator = null;
    [SerializeField] private string triggerShootName = "shoot";
    [SerializeField] private string triggerReloadName = "reload";
    #endregion

    #region PROPERTIES
    public float FireRate { get => fireRate; }
    #endregion

    #region PRIVATE_FIELDS
    private int currentAmmo = 10;

    private bool isReloading = false;

    private Action<Transform, Vector3> onSpawnTrail = null;
    private Action<EFFECT_TYPE, Vector3> onSpawnParticlesEffect = null;
    #endregion

    #region INIT
    public void Init(Action<Transform, Vector3> onSpawnTrail, Action<EFFECT_TYPE, Vector3> onSpawnParticlesEffect)
    {
        this.onSpawnTrail = onSpawnTrail;
        this.onSpawnParticlesEffect = onSpawnParticlesEffect;

        reloadTimer.Init(reloadRate, Reload);

        currentAmmo = maxAmmo;

        PauseManager.instance.AddCallbackOnPause(ChangeState);
    }
    #endregion

    #region PUBLIC_METHODS
    public void Show()
    {
        gameObject.SetActive(true);
        UIManager.Instance.UpdateAmmoText(currentAmmo, maxAmmo);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Shoot(Vector3 mousePos)
    {
        if (isReloading)
        {
            return;
        }

        Vector3 shootPos = shootingPos.position;

        Vector3 direction = (mousePos - shootPos).normalized;

        AudioManager.Instance.PlaySound(shootClip);

        float distance = Vector2.Distance(shootPos, mousePos);

        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPos, direction, distance, ~layersToIgnore);

        if (raycastHit2D.collider != null)
        {
            EnemyController enemyController = raycastHit2D.collider.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                enemyController.TakeDamage();
                onSpawnTrail?.Invoke(shootingPos, raycastHit2D.point);
                onSpawnParticlesEffect?.Invoke(EFFECT_TYPE.BLOOD, raycastHit2D.point);
            }
        }
        else
        {
            onSpawnTrail?.Invoke(shootingPos, mousePos);
            onSpawnParticlesEffect?.Invoke(EFFECT_TYPE.GROUND, mousePos);
        }

        animator.SetTrigger(triggerShootName);

        currentAmmo--;

        UIManager.Instance.UpdateAmmoText(currentAmmo, maxAmmo);

        if (currentAmmo <= 0)
        {
            StartReloading();
        }
    }

    public void StartReloading()
    {
        if (!CanReload())
        {
            return;
        }

        isReloading = true;

        animator.SetTrigger(triggerReloadName);

        AudioManager.Instance.PlaySound(reloadClip);

        reloadTimer.ToggleTimer(true);
    }
    #endregion

    #region PRIVATE_METHODS
    private void Reload()
    {
        currentAmmo = maxAmmo;

        UIManager.Instance.UpdateAmmoText(currentAmmo, maxAmmo);

        isReloading = false;
    }

    private void ChangeState(bool state)
    {
        if(isReloading)
        {
            reloadTimer.ToggleTimer(!state);
        }
    }

    private bool CanReload()
    {
        if(isReloading)
        {
            return false;
        }

        if(currentAmmo >= maxAmmo)
        {
            return false;
        }

        return true;
    }
    #endregion
}
