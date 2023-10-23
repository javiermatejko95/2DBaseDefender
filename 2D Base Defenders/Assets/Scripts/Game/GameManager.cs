using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private WavesManager wavesManager = null;
    [SerializeField] private Barricade barricade = null;
    [SerializeField] private WeaponController weaponController = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private PauseManager pauseManager = null;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        barricade.Initialize(LoseGame);
        pauseManager.Init();
        wavesManager.Init();
        playerController.Init();
        weaponController.Init();

        StartGame();
    }
    #endregion

    #region PRIVATE_METHODS
    private void StartGame()
    {
        wavesManager.StartSpawning();
    }

    private void NextRound()
    {

    }

    private void EndGame()
    {
        PauseManager.instance.CallChangeState(false);

        //wavesManager.StopSpawning();
        //wavesManager.StopEnemies();
    }

    private void LoseGame()
    {
        EndGame();        
        Debug.Log("PERDISTES WEI");
    }
    #endregion
}
