using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBtn : MonoBehaviour
{
    [SerializeField] private OptionUIManager optionUIManager;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            optionUIManager.OnBtn(OptionBtnIdx.SOUND);
        });
    }
}
