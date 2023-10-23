using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance = null;

    #region EXPOSED_FIELDS
    [SerializeField] private GameObject menuPause = null;

    [SerializeField] private Button buttonResume = null;
    [SerializeField] private Button buttonOptions = null;
    [SerializeField] private Button buttonMenu = null;
    #endregion

    #region PRIVATE_FIELDS
    private bool isOnPause = false;

    private Action<bool> onChangeState = null;
    #endregion

    #region INIT
    public void Init()
    {
        instance = this;

        isOnPause = false;

        ChangeState(isOnPause);
    }
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isOnPause = !isOnPause;
            ChangeState(isOnPause);
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void AddCallbackOnChangeState(Action<bool> onChangeState)
    {
        this.onChangeState += onChangeState;
    }

    public void RemoveCallbackOnChangeState(Action<bool> onChangeState)
    {
        this.onChangeState -= onChangeState;
    }

    public void CallChangeState(bool state)
    {
        onChangeState?.Invoke(state);
    }
    #endregion

    #region PRIVATE_METHODS
    private void ChangeState(bool state)
    {
        menuPause.SetActive(state);
        onChangeState?.Invoke(state);
    }
    #endregion
}
