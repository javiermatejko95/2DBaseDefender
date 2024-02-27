using System;
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
    public static WeaponController Instance = null;

    [SerializeField] private Gun[] gunsInventory = null;
    [SerializeField] private Gun selectedGun = null;
    [SerializeField] private GunTrail trail = null;
    [SerializeField] private ParticlesEffect bloodEffect = null;
    [SerializeField] private ParticlesEffect groundEffect = null;

    private bool canShoot = false;

    private int currentWeaponIndex = 0;

    private float nextFireTime = 0f;
    private float fireRate = 1f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        currentWeaponIndex = -1;

        SetupWeapons();

        SelectGunByIndex(0);

        canShoot = true;

        PauseController.Instance.AddCallbackOnPause(ChangeState);
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

    public void SelectGunByReference(Gun selectedGun)
    {
        if(gunsInventory[currentWeaponIndex] == selectedGun)
        {
            return;
        }

        if(!selectedGun.IsUnlocked)
        {
            return;
        }

        int index = Array.IndexOf(gunsInventory, selectedGun);

        SelectGun(index);
    }

    private void SelectGunByIndex(int index)
    {
        if (currentWeaponIndex == index)
        {
            return;
        }

        if (!gunsInventory[index].IsUnlocked)
        {
            return;
        }

        SelectGun(index);
    }

    private void SelectGun(int index)
    {
        if(currentWeaponIndex >= 0)
        {
            gunsInventory[currentWeaponIndex].Hide();
        }        

        selectedGun = gunsInventory[index];

        fireRate = selectedGun.FireRate;

        gunsInventory[index].Show();

        currentWeaponIndex = index;
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

    private void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectGunByIndex(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectGunByIndex(1);
        }
    }
}
