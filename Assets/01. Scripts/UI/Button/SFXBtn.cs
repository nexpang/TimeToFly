using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXBtn : MonoBehaviour
{
    [SerializeField] private OptionUIManager optionUIManager;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            optionUIManager.OnSoundBtn(SoundSetting.SFX);
        });
    }
}
