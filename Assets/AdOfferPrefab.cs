using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdOfferPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTime;
    [SerializeField] GameObject glow;
    [SerializeField] GameObject adIndicator;
    [SerializeField] GameObject activeBoostIndicator;
    [SerializeField] string offerName;
    [SerializeField] int durationMin;

    public static System.Action<string> ClickedAdOffer;

    // Start is called before the first frame update
    private void Start()
    {
        OnEnable();
    }

    void OnEnable()
    {
        AdOfferManager.receivedAdBoost += AdBoostReceived;

        textTime.text = durationMin.ToString() + " MIN";
        StartCoroutine(CheckIfActiveOffer());

        if (DataMgr.instance.IsOfferBonusActive())
        {
            SetLookToActiveBoost();
        }
        else if (!DataMgr.instance.IsOfferActive())
        {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        AdOfferManager.receivedAdBoost -= AdBoostReceived;
    }

    void AdBoostReceived()
    {
        SetLookToActiveBoost();
    }

    void SetLookToActiveBoost()
    {
        //glow.SetActive(false);
        adIndicator.SetActive(false);
        activeBoostIndicator.SetActive(true);   
        StartCoroutine(TimeLeftUpdater());
    }

    IEnumerator TimeLeftUpdater()
    {
        while (true)
        {
            int min = Mathf.Abs(DataMgr.instance.GetCurrentAdBonusTimeLeft().Minutes);
            int sec = Mathf.Abs(DataMgr.instance.GetCurrentAdBonusTimeLeft().Seconds);

            textTime.text = min.ToString() + ":" + sec.ToString("00");
            yield return new WaitForSeconds(1);

            if (DataMgr.instance.IsOfferBonusActive() == false)
            {
                DataMgr.instance.SetOfferBonusToDisabled();
                gameObject.SetActive(false);
            }
        }
    }

    public void Clicked()
    {
        if (!DataMgr.instance.IsOfferBonusActive())
            ClickedAdOffer?.Invoke(offerName);
    }

    IEnumerator CheckIfActiveOffer()
    {
        while (true)
        {
            if (DataMgr.instance.IsOfferActive() == false)
            {
                gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(2);
        }
    }
}
