using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationMgr : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    //[SerializeField] float notifTime = 1.5f;

    void OnEnable()
    {
        UIMGR.NewLayerDiscovered += ShowNotification;
    }

    void OnDisable()
    {
        UIMGR.NewLayerDiscovered -= ShowNotification;
    }

    void ShowNotification()
    {
        _panel.SetActive(true);
        AudioMgr.instance.PlayAudioNewLayer();
        //StartCoroutine(HideNotificationAfter(notifTime));
    }

    /*
    IEnumerator HideNotificationAfter(float t)
    {
        yield return new WaitForSeconds(notifTime);
        _text.enabled = false;
    } */
}
