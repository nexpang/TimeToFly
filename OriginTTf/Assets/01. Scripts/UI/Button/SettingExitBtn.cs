using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingExitBtn : MonoBehaviour
{
    [SerializeField] private OptionUIManager optionUIManager;
    [SerializeField] private CanvasGroup closePanel;
    [SerializeField] private CanvasGroup openPanel;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            optionUIManager.OnBtn(OptionBtnIdx.EXITPANEL, closePanel, openPanel);
        });
    }
}
