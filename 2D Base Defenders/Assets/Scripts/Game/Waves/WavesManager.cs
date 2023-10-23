using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private WaveSO[] wavesData = null;
    [SerializeField] private Timer timer = null;
    [SerializeField] private Transform[] spawnPoints = null;    
    #endregion

    #region PRIVATE_FIELDS
    [SerializeField] private List<EnemyController> enemies = new List<EnemyController>();

    private int currentWave = 0;
    private int currentIndex = 0;

    private int enemiesKilled = 0;

    private bool canSpawn = false;

    private Barricade barricade = null;
    #endregion

    #region ACTIONS
    private Action onEndGame = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {

    }
    #endregion

    #region INIT
    public void Init()
    {
        timer.Init(spawnRate, SpawnEnemy);

        barricade = Barricade.instance;

        //PauseManager.instance.AddCallbackOnChangeState(ChangeEnemiesState);
        PauseManager.instance.AddCallbackOnChangeState(ChangeSpawningState);
    }
    #endregion

    #region PUBLIC_METHODS
    public void StartSpawning()
    {        
        Spawn();
    }
    #endregion

    #region PRIVATE_METHODS
    private void Spawn()
    {       
        Debug.Log("Spawneando los enemigos apagados");
        canSpawn = true;
        enemiesKilled = 0;

        for (int i = 0; i < wavesData[currentWave].AmountToSpawn; i++)
        {
            int randomPoint = UnityEngine.Random.Range(0, spawnPoints.Length);
            int index = i;

            EnemyController enemyController = Instantiate(wavesData[currentWave].EnemyPrefab).GetComponent<EnemyController>();

            enemyController.Initialize(() =>
            {
                enemiesKilled++;
                CheckNextWave();
            }, barricade);
            enemyController.SetPosition(spawnPoints[randomPoint].position);
            enemyController.Toggle(false);

            enemies.Add(enemyController);
        }

        timer.RestartTimer();
        timer.ToggleTimer(true);

        Debug.Log("Se prende el spawner");
    }

    private void SpawnEnemy()
    {
        enemies[currentIndex].Toggle(true);
        Debug.Log("Se spawnea el enemigo numero" + currentIndex);
        currentIndex++;

        if(currentIndex >= enemies.Count)
        {
            currentIndex = 0;
            timer.ToggleTimer(false);
            canSpawn = false;
            Debug.Log("Se apaga el spawner");
        }
        else
        {
            timer.RestartTimer();
            timer.ToggleTimer(true);
            Debug.Log("Se reinicia el spawner");
        }
    }

    private void CheckNextWave()
    {
        if(enemiesKilled >= enemies.Count)
        {
            if(currentWave >= wavesData.Length - 1)
            {
                Debug.Log("GANASTE!");
            }
            else
            {
                enemiesKilled = 0;
                enemies.Clear();
                currentWave++;
                Spawn();
            }
        }
    }

    private void ChangeSpawningState(bool state)
    {
        if(canSpawn)
        {
            timer.ToggleTimer(!state);
        }        
    }
    #endregion
}
