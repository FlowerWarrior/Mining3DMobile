using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] GameObject fullBackpackPanel;
    public static System.Action Tap;
    bool startedOnce = false;

    private void Start()
    {
        StartCoroutine(ClickLoop());
        startedOnce = true;
    }

    void OnEnable()
    {
        if (startedOnce)
            StartCoroutine(ClickLoop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator ClickLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / 8f);
            if (PurchasesMgr.instance.IsAutoTapEnabled() && fullBackpackPanel.activeSelf == false)
            {
                Tap?.Invoke();
            }
        }
    }
}
