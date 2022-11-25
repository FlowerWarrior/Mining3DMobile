using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;

public class ShopItemPickaxe : MonoBehaviour
{
    public int myIndex;
    public bool isPremium = false;

    [SerializeField] Button _button;
    [SerializeField] Image imgBackpack;
    [SerializeField] Image imgDarker;
    [SerializeField] Image imgButtonBg;
    [SerializeField] TextMeshProUGUI txtPower;
    [SerializeField] TextMeshProUGUI txtButton;
    [SerializeField] TextMeshProUGUI txtCost;
    [SerializeField] IAPButton _IAPButton;

    void OnEnable()
    {
        Refresh();
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
        if (isPremium)
        {
            premiumPickaxe premiumPick = DataMgr.instance.GetPremiumPickaxe();

            txtCost.gameObject.SetActive(false);
            txtButton.text = premiumPick.costUSD + "USD";
            txtButton.fontSize = 51;

            imgBackpack.sprite = premiumPick.sprite;
            txtPower.text = "DMG " + premiumPick.power.ToString();
            myIndex = 877787;

            if (DataMgr.instance.IsPickaxeBought(myIndex) == false)
                _IAPButton.enabled = true;
        }
        else
        {
            pickaxe myPickaxe = DataMgr.instance.GetPickaxes()[myIndex];

            txtCost.text = DataMgr.instance.ConvertToCurrency(myPickaxe.cost);
            imgBackpack.sprite = myPickaxe.sprite;
            txtPower.text = "DMG " + myPickaxe.power.ToString();
        }

        if (DataMgr.instance.IsPickaxeBought(myIndex))
        {
            // Set looks to owned
            txtButton.text = "EQUIP";
            imgDarker.gameObject.SetActive(false);
            txtCost.gameObject.SetActive(false);
            imgButtonBg.color = new Color32(102, 176, 255, 255);
            imgBackpack.color = new Color32(255, 255, 255, 255);
            SetElementsToOpacity(255);
        }

        if (DataMgr.instance.GetCurrentPickaxeIndex() == myIndex)
        {
            // Set looks to equipped
            txtButton.text = "IN USE";
            imgButtonBg.color = new Color32(29, 130, 238, 255);

            imgBackpack.color = new Color32(255, 255, 255, 160);
            txtCost.gameObject.SetActive(false);
            SetElementsToOpacity(255);
        }
    }

    void SetElementsToOpacity(byte alpha)
    {
        Color32 fadedColor = new Color32(255, 255, 255, alpha);
        //txtPower.color = fadedColor;
        txtButton.color = fadedColor;
        //imgBackpack.color = fadedColor;
    }

    public void OnClicked()
    {
        if (DataMgr.instance.IsPickaxeBought(myIndex) && DataMgr.instance.GetCurrentPickaxeIndex() != myIndex)
        {
            DataMgr.instance.EquipPickaxe(myIndex);
            AudioMgr.instance.PlayAudioUI();
        }

        if (!DataMgr.instance.IsPickaxeBought(myIndex) && !isPremium)
        {
            if (DataMgr.instance.TryBuyPickaxe(myIndex) == true)
            {
                DataMgr.instance.EquipPickaxe(myIndex);
                Refresh();
            }
        }

        // TODO - Try buy premium pickaxe
    }

    // Ultimate Pickaxe IAP
    public void OnPurchaseSuccesful()
    {
        PurchasesMgr.instance.UserBoughtUltimatePickaxe(_button);
    }
    public void OnPurchaseFailed()
    {
        PurchasesMgr.instance.PurchaseFailed();
    }
}
