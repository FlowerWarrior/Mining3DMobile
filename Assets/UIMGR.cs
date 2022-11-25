using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMGR : MonoBehaviour
{
    [SerializeField] GameObject panelBackground;
    [SerializeField] Slider blockHpSlider;
    [SerializeField] TextMeshProUGUI blockHpText;
    [SerializeField] TextMeshProUGUI blockNameText;
    [SerializeField] TextMeshProUGUI blockValueText;
    [SerializeField] TextMeshProUGUI blockRarityText;
    [SerializeField] GameObject taskPanel;

    [SerializeField] TextMeshProUGUI backpackText;
    [SerializeField] Image[] backpackImagesGameplay;

    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] Image moneyImage;

    [SerializeField] TextMeshProUGUI depthText;
    [SerializeField] TextMeshProUGUI layerText;

    [SerializeField] GameObject fullBackpackPanel;
    [SerializeField] GameObject notEnoughPanel;

    public static UIMGR instance;
    public static System.Action NewLayerDiscovered;
    int lastLayerIndex;

    // Listeners
    private void Awake()
    {
        instance = this;
        DataMgr.RefreshShopUI += UpdateBackpackImages;
        DataMgr.NotEnoughMoney += ShowNotEnoughPanel;
        DataMgr.RefreshShopUI += UpdateBackpackUI;
        MiningMgr.NewBlockSpawned += UpdateLayerLook;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("startedAndClosedTasks", 0);

        UpdateBackpackImages();
        UpdateMoney(DataMgr.instance.GetMoney());
        UpdateDepthText();
        
        lastLayerIndex = DataMgr.instance.GetCurrentLayerIndex();
        UpdateLayerLook();
    }

    void ShowNotEnoughPanel()
    {
        notEnoughPanel.SetActive(true);
    }

    public void UpdateBackpackImages()
    {
        Sprite backpackSprite;

        if (DataMgr.instance.IsIniniteBackpackEquipped() == false)
        {
            backpack currentBackpack = DataMgr.instance.GetCurrentBackpack();
            backpackSprite = currentBackpack.sprite;
        }
        else
        {
            backpackSprite = DataMgr.instance.GetInfiniteBackpack().sprite;
        }
        

        for (int i = 0; i < backpackImagesGameplay.Length; i++)
        {
            backpackImagesGameplay[i].sprite = backpackSprite;
        }

        UpdateBackpackUI();
    }

    public void UpdateBlockUI(int currentHp, block myBlock)
    {
        blockHpText.text = $"{currentHp}/{myBlock.hp}";
        blockHpSlider.value = ((float)currentHp / (float)myBlock.hp);
        blockValueText.text = DataMgr.instance.ConvertToCurrency(myBlock.value);
        blockNameText.text = myBlock.name;
        blockRarityText.text = myBlock.rarity;
    }

    public void UpdateBackpackUI()
    {
        int backpackIndex = DataMgr.instance.GetCurrentBackpackIndex();
        string itemsCount = DataMgr.instance.GetBapackDisplayItemsCount();
        string capacity = DataMgr.instance.GetBapackDisplayCapacity(backpackIndex);
        backpackText.text = $"{itemsCount}/{capacity}";
    }

    public void UpdateMoney(int amount)
    {
        moneyText.text = DataMgr.instance.ConvertToCurrency(amount);
    }

    public void UpdateDepthText()
    {
        int amount = PlayerPrefs.GetInt("blocksMined", 0);
        depthText.text = "DEPTH: " + amount.ToString();
    }

    public void UpdateLayerLook()
    {
        layerText.text = "LAYER: " + DataMgr.instance.GetCurrentLayer().name;
        Camera.main.backgroundColor = DataMgr.instance.GetCurrentLayer().bgColor;

        if (lastLayerIndex != DataMgr.instance.GetCurrentLayerIndex())
        {
            lastLayerIndex = DataMgr.instance.GetCurrentLayerIndex();
            NewLayerDiscovered?.Invoke();
        }
    }

    public void ShowFullBackpackPanel()
    {
        fullBackpackPanel.SetActive(true);
        panelBackground.SetActive(true);
    }
}
