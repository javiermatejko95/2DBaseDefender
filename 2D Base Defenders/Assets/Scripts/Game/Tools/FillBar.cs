using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Image barImg = null;
    #endregion

    #region PRIVATE_FIELDS
    private int maxValue = 1;
    private int currentValue = 1;
    #endregion

    #region INIT
    public void Initialize(int maxValue)
    {
        this.maxValue = maxValue;
        currentValue = maxValue;

        barImg.fillAmount = 1f;
    }
    #endregion

    #region PUBLIC_METHODS
    public void UpdateValue(int amount)
    {
        currentValue += amount;

        if(currentValue <= 0)
        {
            currentValue = 0;
        }

        barImg.fillAmount = ((float)currentValue / (float)maxValue);
    }
    #endregion
}
