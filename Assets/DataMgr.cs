using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

[ExecuteInEditMode]
public class DataMgr : MonoBehaviour
{
    public static DataMgr instance;
    public static System.Action<int, int> UpdateMoneyUI;
    public static System.Action RefreshShopUI;
    public static System.Action NotEnoughMoney;
    public static System.Action Rebirthed;

    [SerializeField] private BlocksData blocksData;
    [SerializeField] private BackpacksData backpacksData;
    [SerializeField] private TasksData tasksData;
    [SerializeField] private PickaxesData pickaxesData;

    private void Awake()
    {
        instance = this;
    }

    public void IncrementSecondsPlayed()
    {
        PlayerPrefs.SetInt("totalSecondsPlayed", 1 + GetSecondsPlayed());
    }

    public int GetSecondsPlayed()
    {
        return PlayerPrefs.GetInt("totalSecondsPlayed", 0);
    }

    public layer[] GetBlocksInfo()
    {
        return blocksData.blockInfo;
    }

    public block GetCurrentBlock()
    {
        int currentLayerID = PlayerPrefs.GetInt("savedLayer", 0);
        int currentBlockID = PlayerPrefs.GetInt("savedBlock", 0);

        if (PlayerPrefs.GetInt("blocksMined", 0) == 0)
        {
            return blocksData.startBlock;
        }

        block myBlock = GetBlocksInfo()[currentLayerID].blocks[currentBlockID];
        return myBlock;
    }

    public backpack[] GetBackpacksInfo()
    {
        return backpacksData.backpacksInfo;
    }

    public backpack GetCurrentBackpack()
    {
        int index = PlayerPrefs.GetInt("selectedBackpack", 0);
        return GetBackpacksInfo()[index];
    }

    public void SetCurrentBackpackIndex(int newIndex)
    {
        PlayerPrefs.SetInt("selectedBackpack", newIndex);
    }

    public int GetCurrentBackpackIndex()
    {
        int index = PlayerPrefs.GetInt("selectedBackpack", 0);
        return index;
    }
    public int GetCurrentPickaxeIndex()
    {
        int index = PlayerPrefs.GetInt("selectedPickaxe", 0);
        return index;
    }
    public pickaxe GetCurrentPickaxe()
    {
        int index = PlayerPrefs.GetInt("selectedPickaxe", 0);
        return GetPickaxes()[index];
    }

    public bool IsBackpackBought(int backpackIndex)
    {
        if (backpackIndex == 0)
            return true;

        int state = PlayerPrefs.GetInt($"backpackBought{backpackIndex}", 0);
        if (state == 0)
            return false;
        else
            return true;
    }

    public bool TryBuyBackpack(int backpackIndex)
    {
        int cost = GetBackpacksInfo()[backpackIndex].cost;

        if (GetMoney() >= cost)
        {
            AddToMoney(-cost);
            PlayerPrefs.SetInt($"backpackBought{backpackIndex}", 1);
            AudioMgr.instance.PlayAudioBuy();
            return true;
        }
        else
        {
            NotEnoughMoney?.Invoke();
            return false;
        }
    }

    public void EquipBackpack(int newIndex)
    {
        if (newIndex == 31214350) // infinite backpack
        {
            PlayerPrefs.SetInt("selectedBackpack", newIndex);
            RefreshShopUI?.Invoke();
            return;
        }

        if (IsIniniteBackpackEquipped())
        {
            if (GetBackpackItemsCount() > GetBackpacksInfo()[newIndex].capacity)
            {
                SetBackpackFull(GetBackpacksInfo()[newIndex]);
            }

            PlayerPrefs.SetInt("selectedBackpack", newIndex);
            RefreshShopUI?.Invoke();
            return;
        }

        if (GetCurrentBackpack().capacity > GetBackpacksInfo()[newIndex].capacity)
        {
            if (GetBackpackItemsCount() > GetBackpacksInfo()[newIndex].capacity)
            {
                SetBackpackFull(GetBackpacksInfo()[newIndex]);
            }
        }

        PlayerPrefs.SetInt("selectedBackpack", newIndex);
        //ResetBackpack();
        RefreshShopUI?.Invoke();
    }

    public void EquipPickaxe(int newIndex)
    {
        PlayerPrefs.SetInt("selectedPickaxe", newIndex);
        RefreshShopUI?.Invoke();
    }

    public void SetBackpackFull(backpack myBackpack)
    {
        PlayerPrefs.SetInt("bpItemsCount", myBackpack.capacity);
    }

    public void AddToMoney(int amount)
    {
        int oldCoins = GetMoney();

        if (amount > 0)
        {
            amount *= getCurrentCashBoost();
        }

        int newCoins = oldCoins + amount;

        PlayerPrefs.SetInt("coins", newCoins);
        UpdateMoneyUI?.Invoke(oldCoins, newCoins);

        print(newCoins);
    }

    public int GetMoney()
    {
        return PlayerPrefs.GetInt("coins", 0);
    }

    public int GetRandomBlockIDAtLayer(int layer)
    {
        block[] myBlocks = GetBlocksInfo()[layer].blocks;
        List<int> tempBlockChances = new List<int>();
        int luck = 1;

        // double chance with luck ad boost x2
        if (IsOfferBonusActive() && GetCurrentBoostName() == "luck")
        {
            luck *= 2;
        }
        if (isPremiumLuckBoostBought())
        {
            luck *= 3;
        }

        int chancePool = 0;
        for (int i = 0; i < myBlocks.Length; i++)
        {
            tempBlockChances.Add(0);

            if (myBlocks[i].rarity == "RARE" || myBlocks[i].rarity == "EPIC")
            {
                tempBlockChances[i] += myBlocks[i].chance * luck;
            }
            else
            {
                tempBlockChances[i] += myBlocks[i].chance;
            }

            chancePool += tempBlockChances[i];
        }

        int r = UnityEngine.Random.Range(0, chancePool);

        int blockIndex = 0;
        int chancePoolCounter = 0;
        while (blockIndex < myBlocks.Length)
        {
            if (r >= chancePoolCounter && r < chancePoolCounter + tempBlockChances[blockIndex])
            {
                break;
            }

            chancePoolCounter += tempBlockChances[blockIndex];
            blockIndex++;
        }

        if (GetBlocksInfo()[layer].blocks[blockIndex].rarity == "RARE")
            IncrementRareOresFoundCounter();

        if (GetBlocksInfo()[layer].blocks[blockIndex].rarity == "EPIC")
            IncrementEpicOresFoundCounter();

        return blockIndex;
    }

    public void IncrementRareOresFoundCounter()
    {
        PlayerPrefs.SetInt("rareOresFound", GetRareOresFound() + 1);
    }

    public int GetRareOresFound()
    {
        return PlayerPrefs.GetInt("rareOresFound", 0);
    }

    public void IncrementEpicOresFoundCounter()
    {
        PlayerPrefs.SetInt("epicOresFound", GetEpicOresFound() + 1);
    }

    public int GetEpicOresFound()
    {
        return PlayerPrefs.GetInt("epicOresFound", 0);
    }

    public int GetBlocksMined()
    {
        return PlayerPrefs.GetInt("blocksMined", 0);
    }
    public int GetTotalBlocksMined()
    {
        return PlayerPrefs.GetInt("totalBlocksMined", 0);
    }

    public void AddToBlocksMined()
    {
        int newDepth = 1 + PlayerPrefs.GetInt("blocksMined", 0);
        PlayerPrefs.SetInt("blocksMined", newDepth);

        int newTotal = 1 + PlayerPrefs.GetInt("totalBlocksMined", 0);
        PlayerPrefs.SetInt("totalBlocksMined", newDepth);

        UIMGR.instance.UpdateDepthText();
    }

    public void AddBlockToBackpack(block blockToAdd)
    {
        PlayerPrefs.SetInt("bpItemsValue", blockToAdd.value + GetBackpackValue());
        PlayerPrefs.SetInt("bpItemsCount", 1 + GetBackpackItemsCount());
    }

    public int GetBackpackValue()
    {
        return PlayerPrefs.GetInt("bpItemsValue", 0);
    }

    public string GetBapackDisplayItemsCount()
    {
        return ConvertToShorterFormat(GetBackpackItemsCount());
    }

    public infBackpack GetInfiniteBackpack()
    {
        return backpacksData.infiniteBackpack;
    }

    public string GetBapackDisplayCapacity(int backpackIndex)
    {
        if (DataMgr.instance.IsIniniteBackpackEquipped() == false)
        {
            return ConvertToShorterFormat(GetBackpacksInfo()[backpackIndex].capacity);
        }
        else // ininite bp
        {
            return "∞";
        }
    }

    public int GetBackpackItemsCount()
    {
        return PlayerPrefs.GetInt("bpItemsCount", 0);
    }

    public void ResetBackpack()
    {
        PlayerPrefs.SetInt("bpItemsValue", 0);
        PlayerPrefs.SetInt("bpItemsCount", 0);
    }

    public int GetCurrentLayerIndex()
    {
        int blocksMined = PlayerPrefs.GetInt("blocksMined", 0);
        int layerIndex = 0;

        for (int i = 0; i < GetBlocksInfo().Length; i++)
        {
            if (GetBlocksInfo()[i].depth <= blocksMined)
            {
                layerIndex = i;
            }
            else
            {
                break;
            }

        }

        return layerIndex;
    }

    public layer GetCurrentLayer()
    {
        return GetBlocksInfo()[GetCurrentLayerIndex()];
    }

    public void SetTaskClaimed(int taskIndex)
    {
        PlayerPrefs.SetInt($"taskIndex{taskIndex}", 2);
    }

    public bool IsTaskClaimed(int taskIndex)
    {
        if (PlayerPrefs.GetInt($"taskIndex{taskIndex}", 0) == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public task GetTaskWithIndex(int taskIndex)
    {
        return tasksData.tasks[taskIndex];
    }

    public int GetTasksCount()
    {

        return tasksData.tasks.Length;
    }

    public string ConvertToCurrency(int a)
    {
        return ConvertToShorterFormat(a) + "$";
    }

    public string ConvertToShorterFormat(int a)
    {
        float amount = a;
        string displayText = "";
        if (amount < 1000)
        {
            displayText = amount.ToString();
        }
        else if (amount < 1000000)
        {
            displayText = (amount / 1000).ToString("0.#0") + "k";
        }
        else
        {
            displayText = (amount / 1000000).ToString("0.#0") + "m";
        }

        return displayText;
    }

    public DateTime GetOfferEndTime()
    {
        string savedDate = PlayerPrefs.GetString("offerEndTime", "none");
        return DateTime.Parse(savedDate, CultureInfo.GetCultureInfo("en-US"));
    }

    public TimeSpan GetCurrentAdBonusTimeLeft()
    {
        return DateTime.Now - GetOfferEndTime();
    }

    public bool IsOfferBonusActive()
    {
        if (PlayerPrefs.GetInt("currentAdBonusActive", 0) == 1)
            return true;

        return false;
    }

    public void SetOfferBonusToEnabled()
    {
        PlayerPrefs.SetInt("currentAdBonusActive", 1);
    }

    public void SetOfferBonusToDisabled()
    {
        PlayerPrefs.SetInt("currentAdBonusActive", 0);
    }

    public string GetCurrentOfferBonusName()
    {
        return PlayerPrefs.GetString("currentOffer", "none");
    }

    public DateTime GetSavedTime(string varName)
    {
        string savedDate = PlayerPrefs.GetString(varName, "none");
        return DateTime.Parse(savedDate, CultureInfo.GetCultureInfo("en-US"));
    }

    public void SaveTimeToPlayerPrefs(string varName, DateTime date)
    {
        PlayerPrefs.SetString(varName, date.ToString(CultureInfo.GetCultureInfo("en-US")));
    }

    public bool IsOfferActive()
    {
        try
        {
            if (DateTime.Compare(GetSavedTime("offerEndTime"), DateTime.Now) <= 0)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GetCurrentBoostName()
    {
        return PlayerPrefs.GetString("currentOffer", "none");
    }

    public pickaxe[] GetPickaxes()
    {
        List<pickaxe> pickList = new List<pickaxe>();
        for (int i = 0; i < pickaxesData.pickaxes.Length; i++)
        {
            pickaxe pick = new pickaxe();

            pick.cost = pickaxesData.pickaxes[i].cost;
            pick.prefab = pickaxesData.pickaxes[i].prefab;
            pick.sprite = pickaxesData.pickaxes[i].sprite;
            pick.power = pickaxesData.pickaxes[i].power * GetRebirthPickaxeMultiplier();

            pickList.Add(pick);
        }

        return pickList.ToArray();
    }

    public premiumPickaxe GetPremiumPickaxe()
    {
        premiumPickaxe myPickaxe = pickaxesData.premium;
        myPickaxe.power *= GetRebirthPickaxeMultiplier();

        return myPickaxe;
    }

    public bool IsPickaxeBought(int pickaxeIndex)
    {
        if (pickaxeIndex == 0)
            return true;

        int state = PlayerPrefs.GetInt($"pickaxeBought{pickaxeIndex}", 0);
        if (state == 0)
            return false;
        else
            return true;
    }

    public bool TryBuyPickaxe(int pickaxeIndex)
    {
        int cost = GetPickaxes()[pickaxeIndex].cost;

        if (GetMoney() >= cost)
        {
            AddToMoney(-cost);
            PlayerPrefs.SetInt($"pickaxeBought{pickaxeIndex}", 1);
            AudioMgr.instance.PlayAudioBuy();
            return true;
        }
        else
        {
            NotEnoughMoney?.Invoke();
            return false;
        }
    }

    public bool isTaskActionCompleted(int index)
    {
        switch (index)
        {
            case 0:
                if (DataMgr.instance.GetTotalBlocksMined() >= 5)
                {
                    return true;
                }
                break;
            case 1:
                if (DataMgr.instance.GetTotalBlocksMined() >= 10)
                {
                    return true;
                }
                break;
            case 2:
                if (DataMgr.instance.GetTotalBlocksMined() >= 20)
                {
                    return true;
                }
                break;
            case 3:
                if (DataMgr.instance.GetTotalBlocksMined() >= 30)
                {
                    return true;
                }
                break;
            case 4:
                if (DataMgr.instance.GetTotalBlocksMined() >= 10000)
                {
                    return true;
                }
                break;
            case 5:
                if (DataMgr.instance.GetTotalBlocksMined() >= 100000)
                {
                    return true;
                }
                break;
            case 6:
                return HasUserRatedApp();
            case 7:
                return IsAchieved10CPS();
            case 8:
                return IsAchieved20CPS();
            case 9:
                if (GetRareOresFound() >= 10)
                {
                    return true;
                }
                break;
            case 10:
                if (GetEpicOresFound() >= 15)
                {
                    return true;
                }
                break;
            case 11:
                if (GetEpicOresFound() >= 50)
                {
                    return true;
                }
                break;
            case 12:
                if (GetSecondsPlayed() >= 1800)
                {
                    return true;
                }
                break;
            case 13:
                if (GetSecondsPlayed() >= 7200)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public void SetUserRatedApp()
    {
        PlayerPrefs.SetInt("ratedApp", 1);
    }

    public bool HasUserRatedApp()
    {
        if (PlayerPrefs.GetInt("ratedApp", 0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAchieved10CPS()
    {
        PlayerPrefs.SetInt("achieved10CPS", 1);
    }
    public bool IsAchieved10CPS()
    {
        if (PlayerPrefs.GetInt("achieved10CPS", 0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAchieved20CPS()
    {
        PlayerPrefs.SetInt("achieved20CPS", 1);
    }

    public bool IsAchieved20CPS()
    {
        if (PlayerPrefs.GetInt("achieved20CPS", 0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool areAnyTasksReadyToClaim()
    {
        for (int i = 0; i < tasksData.tasks.Length; i++)
        {
            if (isTaskActionCompleted(i) && DataMgr.instance.IsTaskClaimed(i) == false)
            {
                return true;
            }
        }

        return false;
    }

    public void SetIniniteBackpackToBought()
    {
        PlayerPrefs.SetInt($"backpackBought{31214350}", 1);
        AudioMgr.instance.PlayAudioBuy();
    }

    public bool IsIniniteBackpackEquipped()
    {
        if (GetCurrentBackpackIndex() == 31214350)
        {
            return true;
        }
        return false;
    }

    public void SetUltimatePickaxeToBought()
    {
        PlayerPrefs.SetInt($"pickaxeBought{877787}", 1);
        AudioMgr.instance.PlayAudioBuy();
    }

    public bool isUltimatePickaxeEquipped()
    {
        if (GetCurrentPickaxeIndex() == 877787)
        {
            return true;
        }
        return false;
    }

    public bool isPremiumLuckBoostBought()
    {
        return (PlayerPrefs.GetInt("boughtLuckBoost", 0) == 1);
    }

    public bool isPremiumCashBoostBought()
    {
        return (PlayerPrefs.GetInt("boughtCashBoost", 0) == 1);
    }

    public bool isAdCashBoostActive()
    {
        if (IsOfferBonusActive() && GetCurrentOfferBonusName() == "money")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getCurrentCashBoost()
    {
        int boost = 1;
        print("ispremiumcash: " + isPremiumCashBoostBought().ToString());
        if (DataMgr.instance.isPremiumCashBoostBought())
        {
            boost *= 3;
        }
        print("isadcash: " + isAdCashBoostActive().ToString());
        if (DataMgr.instance.isAdCashBoostActive())
        {
            boost *= 2;
        }
        return boost;
    }

    public void Rebirth()
    {
        PlayerPrefs.SetInt("blocksMined", 0);

        // reset pickaxe
        for (int i = 0; i < GetPickaxes().Length; i++)
        {
            PlayerPrefs.SetInt($"pickaxeBought{i}", 0);
        }

        // reset backpack
        for (int i = 0; i < GetBackpacksInfo().Length; i++)
        {
            PlayerPrefs.SetInt($"backpackBought{i}", 0);
        }

        PlayerPrefs.DeleteKey("savedHp");
        PlayerPrefs.SetInt("bpItemsValue", 0);
        PlayerPrefs.SetInt("bpItemsCount", 0);

        AddToMoney(-GetMoney());

        if (!isUltimatePickaxeEquipped())
        {
            EquipPickaxe(0);
        }

        if (!IsIniniteBackpackEquipped())
        {
            EquipBackpack(0);
        }

        PlayerPrefs.SetInt("rebirthCounter", 1 + PlayerPrefs.GetInt("rebirthCounter", 0));
        Rebirthed?.Invoke();
        UIMGR.instance.UpdateDepthText();
        UIMGR.instance.UpdateLayerLook();
    }

    public int GetRebirthCounter()
    {
        return PlayerPrefs.GetInt("rebirthCounter", 0);
    }

    public int GetRebirthPickaxeMultiplier()
    {
        if (GetRebirthCounter() > 0)
        {
            return (GetRebirthCounter() * 2);
        }
        else
        {
            return 1;
        }
    }
}
