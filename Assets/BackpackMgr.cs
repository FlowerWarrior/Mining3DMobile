using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackMgr : MonoBehaviour
{
    public static System.Action<int> SoldItems;
    int bpCapacity;

    // Listeners
    void Awake()
    {
        SellButton.Clicked += SellBackpackItems;
        MiningMgr.BlockDestroyed += AddItemToBackpack;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DataMgr.instance.IsIniniteBackpackEquipped() == false)
        {
            bpCapacity = DataMgr.instance.GetCurrentBackpack().capacity;
        }
        

        UIMGR.instance.UpdateBackpackUI();
    }

    void AddItemToBackpack(int layer, int index)
    {
        block blockToAdd = DataMgr.instance.GetBlocksInfo()[layer].blocks[index];
        DataMgr.instance.AddBlockToBackpack(blockToAdd);

        UIMGR.instance.UpdateBackpackUI();
    }

    void SellBackpackItems()
    {
        if (DataMgr.instance.GetBackpackItemsCount() > 0)
        {
            int value = DataMgr.instance.GetBackpackValue();
            SoldItems?.Invoke(value);
            DataMgr.instance.AddToMoney(value);

            AudioMgr.instance.PlayAudioSell();

            DataMgr.instance.ResetBackpack();
            UIMGR.instance.UpdateBackpackUI();
        }
    }
}
