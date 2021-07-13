using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
    TIMERTEXT
}

public class GameUI : MonoBehaviour
{
    [SerializeField] UIType type;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        switch(type)
        {
            case UIType.TIMERTEXT:
                TimerUpdate();
                break;
        }
    }

    void TimerUpdate()
    {
        text.color = (GameManager.Instance.timer <= 99 || GameManager.Instance.player.abilitys[4].GetComponent<Ability_Teleport>().IsUsing) ? Color.red : Color.black;
        text.text = string.Format("TIME\n{0:D3}", GameManager.Instance.timer);
    }
}
