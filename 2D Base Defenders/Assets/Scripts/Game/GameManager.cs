using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WavesManager wavesManager = null;
    [SerializeField] private Barricade barricade = null;
    [SerializeField] private WeaponController weaponController = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private PauseController pauseManager = null;
    [SerializeField] private FeedbackManager feedbackManager = null;
    [SerializeField] private ShopController shopController = null;

    [Space, Header("UI")]
    [SerializeField] private Button buttonStart = null;
    [SerializeField] private Countdown countdown = null;

    private void Awake()
    {
        barricade.Initialize(LoseGame);
        pauseManager.Init();
        wavesManager.Init(WinGame);
        playerController.Init();
        shopController.Setup();
        weaponController.Init();
        feedbackManager.Init(RestartGame);

        //buttonStart.onClick.AddListener(() =>
        //{
        //    buttonStart.gameObject.SetActive(false);
        //    countdown.Initialize(FinishCountdown);
        //    countdown.Show();
        //    countdown.StartCountdown();
        //});

        pauseManager.SetCanPause(false);
        pauseManager.SetPause(true);        

        countdown.Initialize(FinishCountdown);
        countdown.Show();
        countdown.StartCountdown();
    }

    private void StartGame()
    {
        pauseManager.SetCanPause(true);
        wavesManager.StartSpawning();
    }

    private void RestartGame()
    {
        countdown.Initialize(() =>
        {
            wavesManager.RestartSpawning();
            FinishCountdown();
        });
        countdown.Show();
        countdown.StartCountdown();        
    }

    private void NextRound()
    {

    }

    private void EndGame(bool status)
    {
        pauseManager.SetCanPause(false);
        pauseManager.SetPause(true);

        feedbackManager.SetHasWon(status);
        feedbackManager.Toggle(true);
    }

    private void LoseGame()
    {
        EndGame(false);
        Debug.Log("PERDISTES WEI");
    }

    private void WinGame()
    {
        EndGame(true);
        Debug.Log("GANASTES WEI");
    }

    private void FinishCountdown()
    {
        countdown.Hide();
        pauseManager.SetCanPause(true);
        pauseManager.SetPause(false);
        StartGame();
    }
}
