using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyController : MonoBehaviour
{
    public static EconomyController Instance = null;

    [SerializeField] private TextMeshProUGUI totalCoinsText = null;
    [SerializeField] private int totalCoins = 0;

    public int TotalCoins { 
        get { return totalCoins; }  
        set
        {
            totalCoins = value;
            totalCoinsText.text = "$ " + totalCoins.ToString();
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        TotalCoins = 0;
    }
}
