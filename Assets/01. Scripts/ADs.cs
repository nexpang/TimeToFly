using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class ADs : MonoBehaviour
{
    private readonly string nomalFullunitID = "ca-app-pub-3131514107827460/4413745622";
    private readonly string nomalFulltestUnitID = "ca-app-pub-3940256099942544/1033173712";

    private readonly string rewardUnitID = "ca-app-pub-3131514107827460/5816179400";
    private readonly string rewardTestUnitID = "ca-app-pub-3940256099942544/5224354917";

    public RewardedAd rewardedAd;
    public InterstitialAd screenAd;

    private bool isFullSizeAdloaded = false;

    public PlayerController PlayerController;
    public void CallFUllSizeAD()
    {
        NomalInitAD();
        StartCoroutine(ScreenAdShow());
    }

    public void CallRewardAD()
    {
        RewardInitAD();
        StartCoroutine(RewardADShow());
    }

    private void NomalInitAD()
    {
        string id = Debug.isDebugBuild ? nomalFulltestUnitID : nomalFullunitID;

        screenAd = new InterstitialAd(id);

        AdRequest request = new AdRequest.Builder().Build();

        screenAd.LoadAd(request);
        screenAd.OnAdClosed += (sender, e) => Debug.Log("±¤°í°¡ ´ÝÈû");
        screenAd.OnAdLoaded += (sender, e) => Debug.Log("±¤°í°¡ ·ÎµåµÊ");
    }

    private void RewardInitAD()
    {
        string id = Debug.isDebugBuild ? rewardTestUnitID : rewardUnitID;

        rewardedAd = new RewardedAd(id);

        AdRequest request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request);

        rewardedAd.OnAdFailedToLoad += (sender, e) => Debug.LogWarning("failed to load AD");
        rewardedAd.OnAdFailedToShow += (sender, e) => Debug.LogWarning("failed to show AD");
        rewardedAd.OnAdDidRecordImpression += (sender, e) => Debug.LogWarning("disconnect AD , no reward");
        rewardedAd.OnUserEarnedReward += (sender, e) =>
        {
            GameManager.Instance.UserEarnedRewardFromaAD();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        };
    }

    private IEnumerator RewardADShow()
    {
        while(!rewardedAd.IsLoaded())
        {
            yield return null;
        }
        rewardedAd.Show();
    }

    private IEnumerator ScreenAdShow()
    {
        while(!screenAd.IsLoaded())
        {
            yield return null;
        }
        screenAd.Show();
    }
}
