using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    public bool isTestMode;

    void Start()
    {
        LoadBannerAd();
        ToggleBannerAd(true);
    }

    void Update() { }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }



    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "";
    BannerView bannerAd;


    void LoadBannerAd()
    {
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        if (this.bannerAd != null)
        {
            this.bannerAd.Destroy();
        }
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
            adaptiveSize, AdPosition.Top);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion


}