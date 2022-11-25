using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeMeshMgr : MonoBehaviour
{
    Animator _animatorPickaxe;

    // Start is called before the first frame update
    void Start()
    {
        _animatorPickaxe = GetComponent<Animator>();

        UpdatePickaxePrefab();
    }

    void OnEnable()
    {
        DataMgr.RefreshShopUI += UpdatePickaxePrefab;
        MiningMgr.PickaxeHit += PlayHitAnim;
        DataMgr.Rebirthed += UpdatePickaxePrefab;
    }

    void OnDisable()
    {
        DataMgr.RefreshShopUI -= UpdatePickaxePrefab;
        MiningMgr.PickaxeHit -= PlayHitAnim;
        DataMgr.Rebirthed -= UpdatePickaxePrefab;
    }

    void UpdatePickaxePrefab()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GameObject pickPrefab;
        if (DataMgr.instance.isUltimatePickaxeEquipped())
        {
            pickPrefab = DataMgr.instance.GetPremiumPickaxe().prefab;
        }
        else
        {
            pickPrefab = DataMgr.instance.GetCurrentPickaxe().prefab;
        }
        
        Instantiate(pickPrefab, transform.position, transform.rotation, transform);
    }

    void PlayHitAnim()
    {
        // Pickaxe Animation
        _animatorPickaxe.Play("A_PickMine", 0, 0);
    }
}
