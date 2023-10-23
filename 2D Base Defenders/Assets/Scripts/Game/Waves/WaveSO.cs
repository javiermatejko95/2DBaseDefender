
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "ScriptableObjects/Waves", order = 1)]
public class WaveSO : ScriptableObject
{
    #region EXPOSED_FIELDS
    [SerializeField] private string id = string.Empty;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private int amountToSpawn = 10;
    #endregion

    #region PROPERTIES
    public string Id { get => id; }
    public GameObject EnemyPrefab { get => enemyPrefab; }
    public int AmountToSpawn { get => amountToSpawn; }
    #endregion
}
