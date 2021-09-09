using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ADs : MonoBehaviour
{
    private readonly string unitID = "ca-app-pub-3131514107827460/4413745622";
    private readonly string testUnitID = "ca-app-pub-3940256099942544/1033173712";

    public InterstitialAd screenAd;

    private bool isFullSizeAdloaded = false;

    public PlayerController PlayerController;
    public void CallFUllSizeAD()
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
        //screenAd.OnAdLoaded += (sender, e) => { isFullSizeAdloaded = true; };
        screenAd.OnAdClosed += (sender, e) =>
        {
            Time.timeScale = 1;
            PlayerController.ClearFuncOnCloseAd();
        };
    }

/*    public bool GetAdState()
    {
        if (isFullSizeAdloaded)
            return true;
        else
            return false;
    }
*/
    
        

    private IEnumerator ShowAd()
    {
        while(!screenAd.IsLoaded())
        {
            yield return null;
        }
        screenAd.Show();
    }
}
