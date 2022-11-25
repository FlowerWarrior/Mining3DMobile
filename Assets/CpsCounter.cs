using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CpsCounter : MonoBehaviour
{
    TextMeshProUGUI _text;
    float cps = 0;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        MiningMgr.PickaxeHit += Clicked;
        StartCoroutine(UpdateLoop());
    }

    void OnDisable()
    {
        MiningMgr.PickaxeHit -= Clicked;
        StopCoroutine(UpdateLoop());
        cps = 0;
    }

    void Clicked()
    {
        cps++;
        //_text.text = cps.ToString("0.0");
        StartCoroutine(SubstractAfterSecond());
    }

    IEnumerator SubstractAfterSecond()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1f);
            cps -= 1f;
        }
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _text.text = cps.ToString("0.0");

            if (cps >= 10)
            {
                DataMgr.instance.SetAchieved10CPS();
            }

            if (cps >= 20)
            {
                DataMgr.instance.SetAchieved20CPS();
            }
        }
    }
}
