using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class IntroCutScene : MonoBehaviour
{
    public Image blockPanelAll;
    public Button skipBtn;

    void Start()
    {
        blockPanelAll.color = Color.black;
        blockPanelAll.DOFade(0, 1.5f);
        Invoke("EndScene", 68);

        if (!SecurityPlayerPrefs.GetBool("newbie", true))
        {
            skipBtn.gameObject.SetActive(true);
            skipBtn.onClick.AddListener(Skip);
        }
    }

    public void Skip()
    {
        CancelInvoke();
        blockPanelAll.DOFade(1, 1.5f).OnComplete(() =>
        {
            SceneController.LoadScene("InGame");
        });
        skipBtn.interactable = false;
    }

    void EndScene()
    {
        SecurityPlayerPrefs.SetBool("newbie", false);
        SceneController.LoadScene("InGame");
    }
}
