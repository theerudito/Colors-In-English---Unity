using GoogleMobileAds.Api;
using UnityEngine;

public class AdsBanner : MonoBehaviour
{
    public static AdsBanner Instance { get; private set; }

    private BannerView _bannerView;

#if UNITY_ANDROID
    private string _adsBanner = "ca-app-pub-3940256099942544/6300978111";
#else
    private string _adUnitId = "unused";
#endif


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAdsBanner()
    {
        if (_bannerView == null)
        {
            CreateBannerView();
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        _bannerView.LoadAd(adRequest);
    }

    private void CreateBannerView()
    {

        if (_bannerView != null)
        {
            DestroyAdsBanner();
        }
        _bannerView = new BannerView(_adsBanner, AdSize.Banner, AdPosition.Bottom);
    }
    private void DestroyAdsBanner()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
}
