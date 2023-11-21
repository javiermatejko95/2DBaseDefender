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
    [SerializeField] private Button buttonSettings = null;
    [SerializeField] private Button buttonMenuBack = null;

    [Space, Header("Scenes")]
    [SerializeField] private string backScene = "Menu";
    #endregion

    #region PRIVATE_FIELDS
    private bool isOnPause = false;
    private bool canPause = false;

    private Action<bool> onPause = null;
    #endregion

    #region INIT
    public void Init()
    {
        instance = this;

        isOnPause = false;
        canPause = true;

        ChangeState(isOnPause);

        buttonResume.onClick.AddListener(OnResume);
        buttonMenuBack.onClick.AddListener(OnMenuBack);
    }
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if(!canPause)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            isOnPause = !isOnPause;
            ChangeState(isOnPause);
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void AddCallbackOnPause(Action<bool> onPause)
    {
        this.onPause += onPause;
    }

    public void RemoveCallbackOnPause(Action<bool> onPause)
    {
        this.onPause -= onPause;
    }

    public void SetPause(bool state)
    {
        onPause?.Invoke(state);
    }

    public void SetCanPause(bool state)
    {
        canPause = state;
    }
    #endregion

    #region PRIVATE_METHODS
    private void ChangeState(bool state)
    {
        menuPause.SetActive(state);
        onPause?.Invoke(state);
    }

    private void OnResume()
    {
        isOnPause = false;
        ChangeState(isOnPause);
    }

    private void OnMenuBack()
    {
        SceneManager.LoadScene(backScene);
    }
    #endregion
}
