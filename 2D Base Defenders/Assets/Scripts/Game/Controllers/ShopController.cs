using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GunShopItem[] shopItems = null;

    public void Setup()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].OnSelected += SelectGun;
        }

        shopItems[0].IsUnlocked = true;
    }

    private void SelectGun(GunShopItem selectedGun)
    {
        if (selectedGun.IsUnlocked)
        {
            WeaponController.Instance.SelectGunByReference(selectedGun.Gun);
            return;
        }

        if (EconomyController.Instance.TotalCoins < selectedGun.ValueAmount)
        {
            Debug.Log("Not enough coins!");
            return;
        }

        EconomyController.Instance.TotalCoins -= selectedGun.ValueAmount;

        selectedGun.IsUnlocked = true;

        WeaponController.Instance.SelectGunByReference(selectedGun.Gun);
    }
}
