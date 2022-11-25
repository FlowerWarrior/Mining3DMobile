using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PurchasesMgr : MonoBehaviour
{
    [SerializeField] GameObject panelPurchaseFailed;
    [SerializeField] GameObject panelPurchaseSuccessful;

    public static PurchasesMgr instance;

    void Awake()
    {
        instance = this;
    }

    public void PurchaseFailed()
    {
        panelPurchaseFailed.SetActive(true);
    }

    public void UserBoughtInfiniteBackpack(Button button)
    {
        button.onClick.RemoveAllListeners();
        panelPurchaseSuccessful.SetActive(true);
        DataMgr.instance.SetIniniteBackpackToBought();
        DataMgr.instance.EquipBackpack(31214350);

        DataMgr.RefreshShopUI?.Invoke();
    }

    public void UserBoughtUltimatePickaxe(Button button)
    {
        button.onClick.RemoveAllListeners();
        panelPurchaseSuccessful.SetActive(true);
        DataMgr.instance.SetUltimatePickaxeToBought();
        DataMgr.instance.EquipPickaxe(877787);

        DataMgr.RefreshShopUI?.Invoke();
    }

    public void UserBoughtLuckBoost(Button button)
    {
        button.onClick.RemoveAllListeners();
        panelPurchaseSuccessful.SetActive(true);
        PlayerPrefs.SetInt("boughtLuckBoost", 1);

        DataMgr.RefreshShopUI?.Invoke();
    }

    public void UserBoughCashBoost(Button button)
    {
        button.onClick.RemoveAllListeners();
        panelPurchaseSuccessful.SetActive(true);
        PlayerPrefs.SetInt("boughtCashBoost", 1);

        DataMgr.RefreshShopUI?.Invoke();
    }

    public void UserBoughtNoAds(Button button)
    {
        button.onClick.RemoveAllListeners();
        panelPurchaseSuccessful.SetActive(true);
        PlayerPrefs.SetInt("boughtNoAds", 1);

        DataMgr.RefreshShopUI?.Invoke();
    }

    public bool isNoAdsBought()
    {
        return (PlayerPrefs.GetInt("boughtNoAds", 0) == 1); 
    }

    public void UserBoughtAutoTap(Button button)
    {
        button.onClick.RemoveAllListeners();
        panelPurchaseSuccessful.SetActive(true);
        PlayerPrefs.SetInt("boughtAutoTap", 1);

        DataMgr.RefreshShopUI?.Invoke();
    }

    public bool IsAutoTapEnabled()
    {
        if (PlayerPrefs.GetInt("activeAutoTap", 1) == 1 && PlayerPrefs.GetInt("boughtAutoTap", 0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void ClickedAutoTapButton()
    {
        if (PlayerPrefs.GetInt("boughtAutoTap", 0) == 1)
        {
            if (PlayerPrefs.GetInt("activeAutoTap", 1) == 1)
            {
                PlayerPrefs.SetInt("activeAutoTap", 0);
            }
            else
            {
                PlayerPrefs.SetInt("activeAutoTap", 1);
            }
        }

        DataMgr.RefreshShopUI?.Invoke();
    }
}
