using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Gun : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float reloadRate = 1f;
    [SerializeField] private LayerMask layersToIgnore;
    [SerializeField] private AudioClip shootClip = null;
    [SerializeField] private AudioClip reloadClip = null;
    [SerializeField] private Transform shootingPos = null;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private bool isReloading = false;

    [Space, Header("Animation")]
    [SerializeField] private Animator animator = null;
    [SerializeField] private string triggerShootName = "shoot";
    [SerializeField] private string triggerReloadName = "reload";

    public float FireRate { get => fireRate; }

    private int currentAmmo = 10;


    private Action<Transform, Vector3> onSpawnTrail = null;
    private Action<EFFECT_TYPE, Vector3> onSpawnParticlesEffect = null;

    public void Init(Action<Transform, Vector3> onSpawnTrail, Action<EFFECT_TYPE, Vector3> onSpawnParticlesEffect)
    {
        this.onSpawnTrail = onSpawnTrail;
        this.onSpawnParticlesEffect = onSpawnParticlesEffect;

        currentAmmo = maxAmmo;
    }

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

        Vector3 direction = shootingPos.right;

        AudioManager.Instance.PlaySound(shootClip);

        float distance = Vector2.Distance(shootPos, mousePos);

        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPos, direction, distance, ~layersToIgnore);

        Vector3 targetPos = shootingPos.position + direction * distance;

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
            onSpawnTrail?.Invoke(shootingPos, targetPos);
            onSpawnParticlesEffect?.Invoke(EFFECT_TYPE.GROUND, targetPos);
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
    }

    private void Reload()
    {
        currentAmmo = maxAmmo;

        UIManager.Instance.UpdateAmmoText(currentAmmo, maxAmmo);

        isReloading = false;
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

    private void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        Debug.DrawLine(startPos, endPos, Color.white, 0.1f);
    }
}
