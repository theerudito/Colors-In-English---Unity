using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsRewarded : MonoBehaviour
{
     public static AdsRewarded Instance { get; private set; }
     private RewardedAd _rewardedAd;


#if UNITY_ANDROID
     private string _adsRewarded = "ca-app-pub-7633493507240683/5880348147";
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

     public void LoadAdsRewarded()
     {
          if (_rewardedAd != null)
          {
               _rewardedAd.Destroy();
               _rewardedAd = null;
          }

          Debug.Log("Loading the rewarded ad.");

          // create our request used to load the ad.
          var adRequest = new AdRequest();

          // send the request to load the ad.
          RewardedAd.Load(_adsRewarded, adRequest,
              (RewardedAd ad, LoadAdError error) =>
              {
                   // if error is not null, the load request failed.
                   if (error != null || ad == null)
                   {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                        return;
                   }

                   Debug.Log("Rewarded ad loaded with response : "
                         + ad.GetResponseInfo());

                   _rewardedAd = ad;
                   ShowRewardedAd();
                   RegisterEventHandlers(_rewardedAd);
                   //RegisterReloadHandler(_rewardedAd);
              });

     }

     public void ShowRewardedAd()
     {
          const string rewardMsg =
              "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

          if (_rewardedAd != null && _rewardedAd.CanShowAd())
          {
               _rewardedAd.Show((Reward reward) =>
               {
                    // TODO: Reward the user.
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
               });
          }
     }

     private void RegisterEventHandlers(RewardedAd ad)
     {
          // Raised when the ad is estimated to have earned money.
          ad.OnAdPaid += (AdValue adValue) =>
          {
               Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
             adValue.Value,
             adValue.CurrencyCode));
          };
          // Raised when an impression is recorded for an ad.
          ad.OnAdImpressionRecorded += () =>
          {
               Debug.Log("Rewarded ad recorded an impression.");
          };
          // Raised when a click is recorded for an ad.
          ad.OnAdClicked += () =>
          {
               Debug.Log("Rewarded ad was clicked.");
          };
          // Raised when an ad opened full screen content.
          ad.OnAdFullScreenContentOpened += () =>
          {
               Debug.Log("Rewarded ad full screen content opened.");
          };
          // Raised when the ad closed full screen content.
          ad.OnAdFullScreenContentClosed += () =>
          {
               Debug.Log("Rewarded ad full screen content closed.");
          };
          // Raised when the ad failed to open full screen content.
          ad.OnAdFullScreenContentFailed += (AdError error) =>
          {
               Debug.LogError("Rewarded ad failed to open full screen content " +
                        "with error : " + error);
          };
     }

     private void RegisterReloadHandler(RewardedAd ad)
     {
          // Raised when the ad closed full screen content.
          ad.OnAdFullScreenContentClosed += () =>
          {
               Debug.Log("Rewarded Ad full screen content closed.");

               // Reload the ad so that we can show another as soon as possible.
               LoadAdsRewarded();
          };
          // Raised when the ad failed to open full screen content.
          ad.OnAdFullScreenContentFailed += (AdError error) =>
          {
               Debug.LogError("Rewarded ad failed to open full screen content " +
                        "with error : " + error);

               // Reload the ad so that we can show another as soon as possible.
               LoadAdsRewarded();
          };
     }



}
