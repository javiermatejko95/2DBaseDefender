using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunShopItem : MonoBehaviour
{
    [SerializeField] private int valueAmount = 10;
    [SerializeField] private bool isLocked = true;
    [SerializeField] private Image selectedImg = null;
    [SerializeField] private Gun gun = null;
    [SerializeField] private bool isStartingWeapon = false;

    public void Select()
    {

    }

    public void Deselect()
    {

    }

    public void Buy()
    {

    }
}
