using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyTextAnim : MonoBehaviour
{
    TextMeshProUGUI txtMoney;
    int animSteps = 10;
    float animTime = 0.3f;
    bool disabledOnce = false;

    void Start()
    {
        txtMoney.text = DataMgr.instance.ConvertToCurrency(DataMgr.instance.GetMoney());
    }

    void OnEnable()
    {
        txtMoney = GetComponent<TextMeshProUGUI>();

        if (disabledOnce)
        {
            txtMoney.text = DataMgr.instance.ConvertToCurrency(DataMgr.instance.GetMoney());
        }
            
        DataMgr.UpdateMoneyUI += ReceiveBroadcastUpdateMoney;      
    }

    void OnDisable()
    {
        disabledOnce = true;
        DataMgr.UpdateMoneyUI -= ReceiveBroadcastUpdateMoney;
    }

    // Update is called once per frame
    void ReceiveBroadcastUpdateMoney(int oldAmount, int newAmount)
    {
        StartCoroutine(UpdateMoneyAnim(oldAmount, newAmount));
    }

    IEnumerator UpdateMoneyAnim(int oldAmount, int newAmount)
    {
        float amountDifference = newAmount - oldAmount;
        float stepDifference = amountDifference / animSteps;
        if ((int)stepDifference == 0)
        {
            stepDifference = 1;
            for (int i = 0; i < amountDifference; i++)
            {
                txtMoney.text = DataMgr.instance.ConvertToCurrency(oldAmount + (int)stepDifference * i);
                yield return new WaitForSeconds(animTime / amountDifference);
            }
        }
        else
        {
            for (int i = 0; i < animSteps; i++)
            {
                txtMoney.text = DataMgr.instance.ConvertToCurrency(oldAmount + (int)stepDifference * i);
                yield return new WaitForSeconds(animTime / animSteps);
            }
        }

        txtMoney.text = DataMgr.instance.ConvertToCurrency(newAmount);
    }
}
