using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public AdsInitializer adsInitializer;
    public InterstitialAd interstitialAd;
    [SerializeField] bool turnOffInterstitialAds = false;
    private bool showInterstitialWhenReady = false;

    public RewardedAds rewardedAds; // city scene
    public HanoiRewardedAds hanoiRewardedAds; // hanoi scene
    [SerializeField] bool turnOffRewardedAds = false;

    public BannerAd bannerAd;
    [SerializeField] bool turnOffBannerAds = false;

    public static AdManager Instance { get; private set; }

    private bool firstSceneLoaded = false;
    string sceneName;

    private void Awake()
    {
        if(adsInitializer == null)
        {
            adsInitializer = FindFirstObjectByType<AdsInitializer>();
        }

        if (bannerAd == null)
            bannerAd = FindFirstObjectByType<BannerAd>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
      
        Instance = this;
        DontDestroyOnLoad(gameObject);
        adsInitializer.OnAdsInitialized += HandleAdsInitialized;
    }

    private bool isInitializedSubscribed = false;
    private void HandleAdsInitialized()
    {
        if (interstitialAd != null && !turnOffInterstitialAds)
        {
            // Unsubscribe first to prevent duplicate stacking across scene reloads
            interstitialAd.OnInterstitialAdReady -= HandleInterstitialReady;
            interstitialAd.OnInterstitialAdReady += HandleInterstitialReady;
            interstitialAd.LoadAd();
        }

        if (!turnOffRewardedAds)
        {
            if (rewardedAds != null) rewardedAds.LoadAd();
            if (hanoiRewardedAds != null) hanoiRewardedAds.LoadAd();
        }

        if (bannerAd != null && !turnOffBannerAds)
        {
            bannerAd.LoadBanner();
        }
    }

    private void HandleInterstitialReady()
    {
        if (!showInterstitialWhenReady)
        {
            Debug.Log("Interstitial ad is ready to be shown again!");
            return;
        }

        showInterstitialWhenReady = false;   // clear first so it shows only once per scene load
        Debug.Log("Showing interstitial ad for the new scene!");
        interstitialAd.ShowAd();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (interstitialAd == null)
            interstitialAd = FindFirstObjectByType<InterstitialAd>();

        showInterstitialWhenReady = !turnOffInterstitialAds;

        GameObject interButtonObj = GameObject.FindWithTag("InterstitialAdButton");
        if (interButtonObj != null && interstitialAd != null)
        {
            Button interstitialButton = interButtonObj.GetComponent<Button>();
            if (interstitialButton != null)
                interstitialAd.SetButton(interstitialButton);
        }

        if (rewardedAds == null)
            rewardedAds = FindFirstObjectByType<RewardedAds>();

        if (hanoiRewardedAds == null)
            hanoiRewardedAds = FindFirstObjectByType<HanoiRewardedAds>();

        sceneName = scene.name;

        string rewardTag = (sceneName == "CityScene") ? "RewardedAdButton" : "HanoiRewardedAdButton";

        GameObject rewardButtonObj = GameObject.FindWithTag(rewardTag);
        if (rewardButtonObj != null)
        {
            Button rewardedAdButton = rewardButtonObj.GetComponent<Button>();
            if (rewardedAdButton != null)
            {
                if (rewardTag == "RewardedAdButton") rewardedAds.SetButton(rewardedAdButton);
                if (rewardTag == "HanoiRewardedAdButton") hanoiRewardedAds.SetButton(rewardedAdButton);
            }
        }

        GameObject bannerButtonObj = GameObject.FindWithTag("BannerAdButton");
        if (bannerButtonObj != null && bannerAd != null)
        {
            Button bannerAdButton = bannerButtonObj.GetComponent<Button>();
            if (bannerAdButton != null)
                bannerAd.SetButton(bannerAdButton);
        }

        if (!firstSceneLoaded)
        {
            firstSceneLoaded = true;
            Debug.Log("First scene loaded!");
            return;
        }

        Debug.Log("Scene relodaded!");
        HandleAdsInitialized();
    }

}
