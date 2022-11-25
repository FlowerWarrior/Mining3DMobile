using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;

public class ShopItemBackpack : MonoBehaviour
{
    public int myIndex;
    public bool isInfinite = false;

    [SerializeField] Button _button;
    [SerializeField] Image imgBackpack;
    [SerializeField] Image imgDarker;
    [SerializeField] Image imgButtonBg;
    [SerializeField] TextMeshProUGUI txtCapacity;
    [SerializeField] TextMeshProUGUI txtButton;
    [SerializeField] TextMeshProUGUI txtCost;
    [SerializeField] IAPButton _IAPButton;

    void OnEnable()
    {
        DataMgr.RefreshShopUI += Refresh;
    }

    void OnDisable()
    {
        DataMgr.RefreshShopUI -= Refresh;
    }

    void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        if (isInfinite)
        {
            infBackpack myBackpack = DataMgr.instance.GetInfiniteBackpack();

            txtCost.gameObject.SetActive(false);
            txtButton.text = myBackpack.costUSD + "USD";
            txtButton.fontSize = 51;

            imgBackpack.sprite = myBackpack.sprite;
            txtCapacity.text = "0/" + myBackpack.capacity;
            myIndex = 31214350;

            if (DataMgr.instance.IsBackpackBought(myIndex) == false)
                _IAPButton.enabled = true;
        }
        else
        {
            backpack myBackpack = DataMgr.instance.GetBackpacksInfo()[myIndex];

            txtCost.text = DataMgr.instance.ConvertToCurrency(myBackpack.cost);
            imgBackpack.sprite = myBackpack.sprite;
            txtCapacity.text = "0/" + myBackpack.capacity.ToString();
        }

        if (DataMgr.instance.IsBackpackBought(myIndex))
        {
            // Set looks to owned
            txtButton.text = "EQUIP";
            imgDarker.gameObject.SetActive(false);
            txtCost.gameObject.SetActive(false);
            imgButtonBg.color = new Color32(102, 176, 255, 255);
            imgBackpack.color = new Color32(255, 255, 255, 255);
            SetElementsToOpacity(255);
        }

        if (DataMgr.instance.GetCurrentBackpackIndex() == myIndex)
        {
            // Set looks to equipped
            txtButton.text = "IN USE";
            imgButtonBg.color = new Color32(29, 130, 238, 255);
            
            imgBackpack.color = new Color32(150, 150, 150, 255);
            txtCost.gameObject.SetActive(false);
            SetElementsToOpacity(255);
        }
    }

    void SetElementsToOpacity(byte alpha)
    {
        Color32 fadedColor = new Color32(255, 255, 255, alpha);
        //txtCapacity.color = fadedColor;
        txtButton.color = fadedColor;
        //imgBackpack.color = fadedColor;
    }

    public void OnClicked()
    {
        if (DataMgr.instance.IsBackpackBought(myIndex) && DataMgr.instance.GetCurrentBackpackIndex() != myIndex)
        {
            DataMgr.instance.EquipBackpack(myIndex);
            AudioMgr.instance.PlayAudioUI();
        }
        
        if (!DataMgr.instance.IsBackpackBought(myIndex) && !isInfinite)
        {
            if (DataMgr.instance.TryBuyBackpack(myIndex) == true)
            {
                DataMgr.instance.EquipBackpack(myIndex);
                Refresh();
            }
        }

        // TODO - Try buy infinite backpack
    }

    // Infinite backpack iap

    public void OnPurchaseSuccesful()
    {
        PurchasesMgr.instance.UserBoughtInfiniteBackpack(_button);
    }
    public void OnPurchaseFailed()
    {
        PurchasesMgr.instance.PurchaseFailed();
    }
}
