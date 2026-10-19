using UnityEngine;

public class AdManager : MonoBehaviour
{
    public AdsInitializer adsInitializer;
    public InterstisialAd interstitialAd;
    [SerializeField] bool turnOffInterstitialAds = false;
    private bool firstAdShown = false;

    public static AdManager Instance { get; private set; }
    
    
    // Turpināsim
    public void Awake()
    {
        
    }
}
