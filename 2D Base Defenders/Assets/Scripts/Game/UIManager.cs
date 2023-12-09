using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region STATIC
    public static UIManager Instance = null;
    #endregion

    #region EXPOSED_FIELDS
    [SerializeField] private TextMeshProUGUI ammoTxt = null;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        ammoTxt.text = currentAmmo + "/" + maxAmmo;
    }
    #endregion
}
