using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private float startingTime = 1f;
    private float currentTime = 1f;
    private bool isOn = false;
    #endregion

    #region ACTIONS
    private Action onReached = null;
    private Action onStart = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if(isOn)
        {
            Countdown();
        }        
    }
    #endregion

    #region INIT
    public void Init(float startingTime, Action onReached = null, Action onStart = null)
    {
        this.startingTime = startingTime;
        currentTime = startingTime;
        this.onReached = onReached;
        this.onStart = onStart;
    }
    #endregion

    #region PUBLIC_METHODS
    public void ToggleTimer(bool status)
    {
        isOn = status;

        if(isOn)
        {
            onStart?.Invoke();
        }
    }

    public void RestartTimer()
    {
        currentTime = startingTime;
    }
    #endregion

    #region PRIVATE_METHODS
    private void Countdown()
    {
        currentTime -= Time.unscaledDeltaTime;

        if(currentTime <= 0f)
        {
            ToggleTimer(false);
            RestartTimer();
            onReached?.Invoke();
        }
    }
    #endregion
}
