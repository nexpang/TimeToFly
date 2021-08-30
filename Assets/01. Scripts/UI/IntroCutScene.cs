using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class IntroCutScene : MonoBehaviour
{
    public Image blockPanelAll;

    void Start()
    {
        blockPanelAll.DOFade(0, 1.5f);
        Invoke("EndScene", 68);
    }

    void EndScene()
    {
        SecurityPlayerPrefs.SetBool("newbie", false);
        SceneController.LoadScene("InGame");
    }
}
