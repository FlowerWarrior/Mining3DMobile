using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBackpackMgr : MonoBehaviour
{
    [SerializeField] GameObject backpackItemPrefab;

    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int count = DataMgr.instance.GetBackpacksInfo().Length;

        for (int i = 0; i < count; i++)
        {
            GameObject newItem = Instantiate(backpackItemPrefab, Vector3.zero, Quaternion.identity, transform);
            newItem.GetComponent<ShopItemBackpack>().myIndex = i;
        }

        GameObject infBackpack = Instantiate(backpackItemPrefab, Vector3.zero, Quaternion.identity, transform);
        infBackpack.GetComponent<ShopItemBackpack>().isInfinite = true;
    }
}
