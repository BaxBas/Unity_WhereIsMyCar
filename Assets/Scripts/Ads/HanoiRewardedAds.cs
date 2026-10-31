using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class HanoiRewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    string _adUnitId;

    [SerializeField] Button _rewardedAdButton;


    void Awake()
    {
        _adUnitId = _androidAdUnitId;
    }

    public void LoadAd()
    {
        if (!Advertisement.isInitialized)
        {
            Debug.Log("Tried to load rewarded ad before Unity Ads was initialized.");
            return;
        }

        Debug.Log("Loading Rewarded ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded ad loaded: " + placementId);

        if (placementId.Equals(_adUnitId))
            _rewardedAdButton.interactable = true;
    }

    public void ShowAd()
    {
        //Vçlâk jâpaslçpj banner ad

        _rewardedAdButton.interactable = false;
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading rewarded ad {placementId} : {error.ToString()} - {message}");
        StartCoroutine(WaitAndLoadAd(10f));
    }

    private IEnumerator WaitAndLoadAd(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing rewarded ad: {placementId} : {error.ToString()} - {message}");
        StartCoroutine(WaitAndLoadAd(10f));
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Time.timeScale = 0f;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("User clicked on rewarded ad: " + placementId);
    }

    public void OnUnityAdsShowComplete(string placementId,
        UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_adUnitId) &&
             showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Rewarded ad completed!");
            _rewardedAdButton.interactable = false;
            StartCoroutine(WaitAndLoadAd(10f));
        }
        Time.timeScale = 1f;
    }

    public void SetButton(Button button)
    {
        if (button == null)
            return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowAd);
        _rewardedAdButton = button;
        _rewardedAdButton.interactable = false;
    }
}
