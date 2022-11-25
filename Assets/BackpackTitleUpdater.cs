using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BackpackTitleUpdater : MonoBehaviour
{
    [SerializeField] bool isBackpack = true; // else is pickaxe
    TextMeshProUGUI textTitle;

    void OnEnable()
    {
        textTitle = GetComponent<TextMeshProUGUI>();
        UpdateTitle();
        DataMgr.RefreshShopUI += UpdateTitle;
    }

    void OnDisable()
    {
        DataMgr.RefreshShopUI -= UpdateTitle;
    }

    void UpdateTitle()
    {
        if (isBackpack)
        {
            if (DataMgr.instance.IsIniniteBackpackEquipped())
            {
                textTitle.text = "BACKPACK: " + "∞" + " CAP.";
            }
            else
            {
                textTitle.text = "BACKPACK: " + DataMgr.instance.GetCurrentBackpack().capacity + " CAP.";
            }
        }
        else
        {
            if (DataMgr.instance.isUltimatePickaxeEquipped())
            {
                textTitle.text = "PICKAXE: " + DataMgr.instance.GetPremiumPickaxe().power + " DMG.";
            }
            else
            {
                textTitle.text = "PICKAXE: " + DataMgr.instance.GetCurrentPickaxe().power + " DMG.";
            }
            
        }
    }
}
