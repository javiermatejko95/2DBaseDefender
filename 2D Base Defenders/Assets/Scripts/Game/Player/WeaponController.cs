using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFFECT_TYPE
{
    BLOOD,
    GROUND
}

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Gun[] gunsInventory = null;
    [SerializeField] private Gun selectedGun = null;
    [SerializeField] private GunTrail trail = null;
    [SerializeField] private ParticlesEffect bloodEffect = null;
    [SerializeField] private ParticlesEffect groundEffect = null;

    private bool canShoot = false;

    private int currentWeaponIndex = 0;

    private float nextFireTime = 0f;
    private float fireRate = 1f;

    public void Init()
    {
        currentWeaponIndex = 0;

        SetupWeapons();

        SelectGun(currentWeaponIndex);

        canShoot = true;

        PauseManager.instance.AddCallbackOnPause(ChangeState);
    }

    private void Update()
    {
        if(!canShoot)
        {
            return;
        }

        ChangeWeapon();

        Shoot();

        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedGun.StartReloading();
        }
    }

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

    private void SpawnTrail(Transform shootPos, Vector3 targetPos)
    {
        GunTrail trailGO = Instantiate(trail.gameObject, shootPos.position, shootPos.rotation).GetComponent<GunTrail>();

        trailGO.Initialize(targetPos);
    }

    private void SpawnParticlesEffect(EFFECT_TYPE type, Vector3 spawnPos)
    {
        ParticlesEffect effect = null;

        switch(type)
        {
            case EFFECT_TYPE.BLOOD:
                effect = bloodEffect; 
                break;
            case EFFECT_TYPE.GROUND:
                effect = groundEffect;                
                break;
        }

        Instantiate(effect.gameObject, spawnPos, effect.gameObject.transform.rotation);
    }

    private void Shoot()
    {
        if(fireRate == 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f;

                selectedGun.Shoot(mousePos);
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time > nextFireTime)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f;

                selectedGun.Shoot(mousePos);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }     
    }

    private void SetupWeapons()
    {
        for(int i = 0; i < gunsInventory.Length; i++)
        {
            gunsInventory[i].Init(SpawnTrail, SpawnParticlesEffect);
            gunsInventory[i].Hide();
        }
    }

    private void SelectGun(int index)
    {
        gunsInventory[currentWeaponIndex].Hide();

        selectedGun = gunsInventory[index];

        fireRate = selectedGun.FireRate;

        gunsInventory[index].Show();

        currentWeaponIndex = index;
    }

    private void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectGun(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectGun(1);
        }
    }
}
