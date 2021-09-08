using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ADs : MonoBehaviour
{
    private readonly string unitID = "ca-app-pub-5031676656577007/8177359685";
    private readonly string testUnitID = "ca-app-pub-3940256099942544/1033173712";

    private InterstitialAd screenAd;

    private void Start()
    {
        InitAD();
        StartCoroutine(ShowAd());
    }

    private void InitAD()
    {
        string id = Debug.isDebugBuild ? testUnitID : unitID;

        screenAd = new InterstitialAd(id);

        AdRequest request = new AdRequest.Builder().Build();

        screenAd.LoadAd(request);
        screenAd.OnAdClosed += (sender, e) => Debug.Log("±¤°í°¡ ´ÝÈû");
        screenAd.OnAdLoaded += (sender, e) => Debug.Log("±¤°í°¡ ·ÎµåµÊ");
    }

    private IEnumerator ShowAd()
    {
        while(!screenAd.IsLoaded())
        {
            yield return null;
        }
        screenAd.Show();
    }
}
