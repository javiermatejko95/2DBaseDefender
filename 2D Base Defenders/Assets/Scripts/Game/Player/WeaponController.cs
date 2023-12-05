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
    #region EXPOSED_FIELDS
    [SerializeField] private Gun gun = null;
    [SerializeField] private Transform shootingPosition = null;
    [SerializeField] private GunTrail trail = null;
    [SerializeField] private ParticlesEffect bloodEffect = null;
    [SerializeField] private ParticlesEffect groundEffect = null;
    #endregion

    #region PRIVATE_FIELDS
    private bool canShoot = false;
    #endregion

    #region INIT
    public void Init()
    {
        gun.Init(SpawnTrail, SpawnParticlesEffect);
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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            Vector3 direction = (mousePos - shootingPosition.position).normalized;

            gun.Shoot(shootingPosition.position, direction, mousePos);

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

    private void SpawnTrail(Vector3 startPos, Vector3 targetPos)
    {
        GunTrail trailGO = Instantiate(trail.gameObject, shootingPosition.position, shootingPosition.rotation).GetComponent<GunTrail>();

        trailGO.Initialize(targetPos);
        //trail.CreateTrail(startPos, targetPos);
        //Instantiate(trail.gameObject, shootingPosition.position, shootingPosition.rotation);
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
    #endregion
}
