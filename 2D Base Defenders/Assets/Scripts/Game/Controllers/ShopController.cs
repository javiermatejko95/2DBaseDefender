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
            shopItems[i].Setup();

            if(shopItems[i].IsStartingWeapon)
            {
                shopItems[i].Unlock();
                shopItems[i].Select();
            }
            else
            {
                shopItems[i].Lock();
                shopItems[i].Deselect();
            }
        }
    }

    private void SelectGun(GunShopItem selectedGun)
    {
        if (selectedGun.IsUnlocked)
        {
            WeaponController.Instance.SelectGunByReference(selectedGun.Gun);

            for(int i = 0; i < shopItems.Length; i++)
            {
                if(shopItems[i] == selectedGun)
                {
                    shopItems[i].Select();
                    continue;                    
                }

                shopItems[i].Deselect();
            }

            return;
        }

        if (EconomyController.Instance.TotalCoins < selectedGun.ValueAmount)
        {
            Debug.Log("Not enough coins!");
            return;
        }

        EconomyController.Instance.TotalCoins -= selectedGun.ValueAmount;

        selectedGun.Unlock();
        selectedGun.Select();

        WeaponController.Instance.SelectGunByReference(selectedGun.Gun);
    }
}
