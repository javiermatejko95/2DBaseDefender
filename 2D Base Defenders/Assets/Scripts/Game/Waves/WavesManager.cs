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
    private Action onWin = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {

    }
    #endregion

    #region INIT
    public void Init(Action onWin)
    {
        this.onWin = onWin;

        timer.Init(spawnRate, SpawnEnemy);

        barricade = Barricade.instance;

        PauseController.Instance.AddCallbackOnPause(ChangeSpawningState);
    }
    #endregion

    #region PUBLIC_METHODS
    public void StartSpawning()
    {        
        Spawn();
    }

    public void RestartSpawning()
    {
        currentWave = 0;

        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] != null)
            {
                enemies[i].Toggle(false);
                enemies[i].ForceDestroy();
            }
        }

        //List<EnemyController> aux = new List<EnemyController>(enemies);

        //for(int i = 0; i < aux.Count; i++)
        //{
        //    if(aux[i] != null)
        //    {
        //        aux[i].ForceDestroy();
        //    }
        //}

        enemies.Clear();
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
                onWin?.Invoke();
            }
            else
            {
                enemiesKilled = 0;
                currentWave++;
                enemies.Clear();
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
