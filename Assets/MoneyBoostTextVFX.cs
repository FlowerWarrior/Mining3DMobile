using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyBoostTextVFX : MonoBehaviour
{
    TextMeshProUGUI _text;
    bool hasRunStart = false;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(UpdateCashBoostLook());
        hasRunStart = true;
    }

    void OnEnable()
    {
        if (hasRunStart)
            StartCoroutine(UpdateCashBoostLook());
    }

    void OnDisable()
    {
        StopCoroutine(UpdateCashBoostLook());
    }

    IEnumerator UpdateCashBoostLook()
    {
        int boost = DataMgr.instance.getCurrentCashBoost();

        _text.text = "x" + boost.ToString();

        if (boost == 1)
        {
            _text.text = "";
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateCashBoostLook());
    }
}
