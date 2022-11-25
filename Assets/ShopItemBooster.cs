using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.Events;

public class ShopItemBooster : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _buttonImage;
    [SerializeField] TextMeshProUGUI _buttonText;
    [SerializeField] string PlayerPrefsName;
    [SerializeField] IAPButton _IAPButton;

    private void Start()
    {
        Refresh();
    }

    void OnEnable()
    {
        DataMgr.RefreshShopUI += Refresh;
    }

    void OnDisable()
    {
        DataMgr.RefreshShopUI -= Refresh;
    }

    void Refresh()
    {

        if (PlayerPrefs.GetInt(PlayerPrefsName, 0) == 1)
        {
            _buttonText.text = "ACTIVE";
            _buttonImage.color = new Color32(238, 150, 28, 255);
            _button.onClick.RemoveAllListeners();

            // Enable disable auto tap
            if (PlayerPrefsName == "boughtAutoTap")
            {
                if (PurchasesMgr.instance.IsAutoTapEnabled())
                {
                    _buttonText.text = "ON";
                    _buttonImage.color = new Color32(28, 216, 238, 255);
                }
                else
                {
                    _buttonText.text = "OFF";
                    _buttonImage.color = new Color32(147, 147, 147, 255);
                }
            }
        }
        else
        {
            _IAPButton.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
