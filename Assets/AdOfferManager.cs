using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdOfferManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    string _adUnitId = "Rewarded_Android";
    string _popupAdUnityId = "Interstitial_Android";

    bool isAdAvailable = false;
    bool popupAvailable = false;
    bool isTimeForPopup = false;

    [SerializeField] Transform whereToSpawn;
    [SerializeField] GameObject luckOffer;
    [SerializeField] GameObject moneyOffer;

    public static System.Action receivedAdBoost;

    void OnEnable()
    {
        AdOfferPrefab.ClickedAdOffer += ShowRewardedAd;
        BackpackMgr.SoldItems += OnSoldItems;
    }
    void OnDisable()
    {
        AdOfferPrefab.ClickedAdOffer -= ShowRewardedAd;
        BackpackMgr.SoldItems -= OnSoldItems;
    }

    void OnSoldItems(int val)
    {
        SometimesPopupAd();
    }

    public void SometimesPopupAd()
    {
        if (popupAvailable && isTimeForPopup && !PurchasesMgr.instance.isNoAdsBought())
        {
            ShowPopupAd();
            isTimeForPopup = false;
            PlanNextPopup(3, 5);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("nextAdOfferTime", "none") == "none")
        {
            PlanNextOfferTime(1,2);
        }

        PlanNextPopup(3, 5);

        StartCoroutine(LoopCheckIfTimeForOffer());

        GameObject offerObject;
        if (DataMgr.instance.GetCurrentOfferBonusName() == "luck")
        {
            offerObject = luckOffer;
        }
        else
        {
            offerObject = moneyOffer;
        }
        if (DataMgr.instance.IsOfferActive())
        {
            offerObject.SetActive(true);
        }
    }

    void PlanNextOfferTime(int minMinutes, int maxMinutes)
    {

        int minutesDelay = UnityEngine.Random.Range(minMinutes, maxMinutes + 1);
        DateTime nextOfferTime = DateTime.Now.AddMinutes(minutesDelay);

        string s = nextOfferTime.ToString(CultureInfo.GetCultureInfo("en-US"));
        PlayerPrefs.SetString("nextAdOfferTime", s);
    }

    void PlanNextPopup(int minMinutes, int maxMinutes)
    {
        int minutesDelay = UnityEngine.Random.Range(minMinutes, maxMinutes + 1);
        DateTime nextOfferTime = DateTime.Now.AddMinutes(minutesDelay);

        string s = nextOfferTime.ToString(CultureInfo.GetCultureInfo("en-US"));
        PlayerPrefs.SetString("nextPopupTime", s);
    }

    bool isReadyForAdOffer(string name)
    {
        string s = PlayerPrefs.GetString(name, "none");
        if (s == "none")
            return false;

        DateTime offerTime = DateTime.Parse(s, CultureInfo.GetCultureInfo("en-US"));
        DateTime now = DateTime.Now;

        print(offerTime);
        print(isAdAvailable);

        if (DateTime.Compare(offerTime, now) <= 0 && isAdAvailable)
        {
            return true;
        }

        return false;
    }


    IEnumerator LoopCheckIfTimeForOffer()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (isReadyForAdOffer("nextAdOfferTime"))
            {
                SpawnOffer();
            }
            if (isReadyForAdOffer("nextPopupTime"))
            {
                isTimeForPopup = true;
            }
        }
    }

    void SpawnOffer()
    {
        GameObject offerObject;

        int lifespan = 3;
        DateTime newTime = DateTime.Now.AddMinutes(lifespan);
        DataMgr.instance.SaveTimeToPlayerPrefs("offerEndTime", newTime);

        int r = UnityEngine.Random.Range(0, 2);
        if (r == 0)
        {
            offerObject = luckOffer;
            PlayerPrefs.SetString("currentOffer", "luck");
        }
        else
        {
            PlayerPrefs.SetString("currentOffer", "money");
            offerObject = moneyOffer;
        }

        offerObject.SetActive(true);

        PlanNextOfferTime(6,9);
    }

    //
    //
    //
    /// <summary>
    /// ADS STUFF
    /// </summary>
    //
    //


    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Advertisement.Load(_adUnitId, this);
        Advertisement.Load(_popupAdUnityId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adUnitId))
        {
            isAdAvailable = true;
        }

        if (adUnitId.Equals(_popupAdUnityId))
        {
            popupAvailable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    void ShowRewardedAd(string offerName)
    {
        if (PurchasesMgr.instance.isNoAdsBought())
        {
            OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
        }
        else
        {
            Advertisement.Show(_adUnitId, this);
        }
    }

    void ShowPopupAd()
    {
        Advertisement.Show(_popupAdUnityId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Grant a reward.
            DataMgr.instance.SetOfferBonusToEnabled();
            DataMgr.instance.SaveTimeToPlayerPrefs("offerEndTime", DateTime.Now.AddMinutes(3));
            receivedAdBoost?.Invoke();
            PlayerPrefs.SetString("watchingAdOffer", "none");

            // Load another ad:
            Advertisement.Load(_adUnitId, this);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}
