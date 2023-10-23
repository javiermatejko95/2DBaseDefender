using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("Menus")]
    [SerializeField] private GameObject menuMain = null;
    [SerializeField] private GameObject menuConfig = null;

    [Space, Header("Menu Main UI")]
    [SerializeField] private Button buttonPlay = null;
    [SerializeField] private Button buttonOptions = null;
    [SerializeField] private Button buttonExit = null;

    [Space, Header("Menu Options UI")]
    [SerializeField] private Button buttonOptionsGameplay = null;
    [SerializeField] private Button buttonOptionsControls = null;
    [SerializeField] private Button buttonOptionsGraphics = null;
    [SerializeField] private Button buttonOptionsAudio = null;
    [SerializeField] private Button buttonOptionsLanguage = null;
    [SerializeField] private Button buttonOptionsBack = null;

    [Space, Header("Scenes")]
    [SerializeField] private string nextScene = "Game";
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        Init();
    }
    #endregion

    #region PRIVATE_METHODS
    private void Init()
    {
        buttonPlay.onClick.AddListener(() => Play());

        buttonOptions.onClick.AddListener(() => OpenOptions());

        buttonExit.onClick.AddListener(() => ExitApplication());


        buttonOptionsBack.onClick.AddListener(() => CloseOptions());
    }

    private void Play()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void OpenOptions()
    {
        menuMain.gameObject.SetActive(false);
        menuConfig.gameObject.SetActive(true);
    }

    private void CloseOptions()
    {
        menuMain.gameObject.SetActive(true);
        menuConfig.gameObject.SetActive(false);
    }

    private void ExitApplication()
    {
        Application.Quit();
    }
    #endregion
}
