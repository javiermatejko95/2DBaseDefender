using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class FeedbackManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject feedbackMenu = null;
    [SerializeField] private Button buttonPlayAgain = null;
    [SerializeField] private Button buttonMenu = null;
    [SerializeField] private TextMeshProUGUI textFeedback = null;
    #endregion

    #region INIT
    public void Init(Action onPlayAgain)
    {
        buttonPlayAgain.onClick.AddListener(() =>
        {
            Toggle(false);
            onPlayAgain?.Invoke();
        });
        buttonMenu.onClick.AddListener(() => Menu());

        Toggle(false);
    }
    #endregion

    #region PUBLIC_METHODS
    public void Toggle(bool status)
    {
        feedbackMenu.SetActive(status);
    }

    public void SetHasWon(bool hasWon)
    {
        textFeedback.text = hasWon ? "You won!" : "You lost";
    }
    #endregion

    #region PRIVATE_METHODS
    private void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    #endregion
}
