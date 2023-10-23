using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barricade : MonoBehaviour
{
    public static Barricade instance;

    #region EXPOSED_FIELDS
    [SerializeField] private int healthAmount = 100;
    [SerializeField] private FillBar healthBar = null;
    #endregion

    #region PRIVATE_FIELDS
    private Action onLose = null;
    #endregion

    #region INIT
    public void Initialize(Action onLose)
    {
        this.onLose = onLose;

        instance = this;
        healthBar.Initialize(healthAmount);
    }
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(1);
        }
    }

    #region PUBLIC_METHODS
    public void TakeDamage(int amount)
    {
        healthAmount -= amount;
        healthBar.UpdateValue(-amount);

        if(healthAmount <= 0)
        {
            onLose?.Invoke();
        }
    }
    #endregion
}
