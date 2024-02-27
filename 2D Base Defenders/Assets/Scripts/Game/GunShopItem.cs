using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunShopItem : MonoBehaviour
{
    [SerializeField] private int valueAmount = 10;
    [SerializeField] private Gun gun = null;
    [SerializeField] private bool isStartingWeapon = false;
    [SerializeField] private Button buyBtn = null;

    public event Action<GunShopItem> OnSelected = null;

    public bool IsUnlocked { get => gun.IsUnlocked; set => gun.IsUnlocked = value; }
    public int ValueAmount { get => valueAmount; }
    public Gun Gun { get => gun; }

    private void Awake()
    {
        buyBtn.onClick.AddListener(Select);
    }

    public void Select()
    {
        OnSelected.Invoke(this);
    }

    public void Deselect()
    {

    }
}
