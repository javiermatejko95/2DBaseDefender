using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunShopItem : MonoBehaviour
{
    [SerializeField] private int valueAmount = 10;
    [SerializeField] private Gun gun = null;
    [SerializeField] private bool isStartingWeapon = false;
    [SerializeField] private Button buyBtn = null;
    [SerializeField] private TextMeshProUGUI priceTxt = null;
    [SerializeField] private Image lockImg = null;
    [SerializeField] private Image selectedImg = null;

    public event Action<GunShopItem> OnSelected = null;

    public bool IsStartingWeapon { get => isStartingWeapon; }
    public bool IsUnlocked { get => gun.IsUnlocked; }
    public int ValueAmount { get => valueAmount; }
    public Gun Gun { get => gun; }

    private void Awake()
    {
        buyBtn.onClick.AddListener(OnSelect);
    }

    public void Setup()
    {
        priceTxt.text = "$ " + valueAmount;
    }

    public void OnSelect()
    {
        OnSelected.Invoke(this);
    }

    public void Select()
    {
        selectedImg.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        selectedImg.gameObject.SetActive(false);
    }

    public void Lock()
    {
        priceTxt.gameObject.SetActive(true);
        lockImg.gameObject.SetActive(true);

        gun.IsUnlocked = false;
    }

    public void Unlock()
    {
        priceTxt.gameObject.SetActive(false);
        lockImg.gameObject.SetActive(false);

        gun.IsUnlocked = true;
    }
}
