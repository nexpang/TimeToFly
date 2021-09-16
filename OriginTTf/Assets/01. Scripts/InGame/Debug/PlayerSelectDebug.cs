using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectDebug : MonoBehaviour
{
    public Button[] chickenUIBtns;

    void Start()
    {
        for(int i = 0;i<chickenUIBtns.Length;i++)
        {
            int temp = i;
            chickenUIBtns[i].onClick.AddListener(() =>
            {
                DebugChickenChange(temp);
            });
        }

        int abilityNumber = SecurityPlayerPrefs.GetInt("debug.startChicken", 0);
        chickenUIBtns[abilityNumber].GetComponent<Image>().color = Color.red;
    }

    void DebugChickenChange(int number)
    {
        for (int i = 0; i < chickenUIBtns.Length; i++)
        {
            chickenUIBtns[i].GetComponent<Image>().color = (i == number) ? Color.red : Color.white;
        }

        SecurityPlayerPrefs.SetInt("debug.startChicken", number);
    }
}
