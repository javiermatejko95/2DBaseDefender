using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance = null;

    [SerializeField] private GameObject menuPause = null;

    [SerializeField] private Button buttonResume = null;
    [SerializeField] private Button buttonSettings = null;
    [SerializeField] private Button buttonMenuBack = null;

    [Space, Header("Scenes")]
    [SerializeField] private string backScene = "Menu";

    private bool isOnPause = false;
    private bool canPause = false;

    private Action<bool> onPause = null;

    public void Init()
    {
        Instance = this;

        isOnPause = false;
        canPause = true;

        ChangeState(isOnPause);

        buttonResume.onClick.AddListener(OnResume);
        buttonMenuBack.onClick.AddListener(OnMenuBack);
    }

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
}
