using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ShopPickaxeMgr : MonoBehaviour
{
    [SerializeField] GameObject pickaxeItemPrefab;
    public bool buttonGenerate; //"run" or "generate" for example

    void Update()
    {
        if (Application.isPlaying)
            this.enabled = false;

        if (buttonGenerate)
            GenerateButtons();

        buttonGenerate = false;
    }

    void GenerateButtons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int count = DataMgr.instance.GetPickaxes().Length;

        for (int i = 0; i < count; i++)
        {
            GameObject newItem = Instantiate(pickaxeItemPrefab, Vector3.zero, Quaternion.identity, transform);
            newItem.GetComponent<ShopItemPickaxe>().myIndex = i;
        }

        GameObject infBackpack = Instantiate(pickaxeItemPrefab, Vector3.zero, Quaternion.identity, transform);
        infBackpack.GetComponent<ShopItemPickaxe>().isPremium = true;
    }
}
