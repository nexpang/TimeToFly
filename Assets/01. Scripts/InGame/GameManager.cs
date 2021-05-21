using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int currentStage = 0; // 디버그용으로 있는 변수
    [SerializeField] bool isNeedTimer = true;
    [SerializeField] int life = 5;
    [SerializeField] bool isInfinityLife = false;
    [SerializeField] bool isDebug = false;
    public bool IsInfinityLife { get { return isInfinityLife; } set { isInfinityLife = value; } }
    [SerializeField] CanvasGroup gameStartScreen = null;
    [SerializeField] Image gameStartScreenChicken = null;
    [SerializeField] Text lifeCount = null;
    SpriteRenderer playerSpr = null;

    private int _timer;
    public int timer
    {
        get { return _timer; }
        set { _timer = value; }
    }

    private void Awake()
    {
        Debug();

        Time.timeScale = 0;
        Instance = this; // 싱글톤

        playerSpr = FindObjectOfType<PlayerAnimation>().GetComponent<SpriteRenderer>();
        life = Temp.Instance.TempLife;
        lifeCount.text = isInfinityLife ? "∞" : life.ToString();

        if (!isDebug)
        {
            gameStartScreen.gameObject.SetActive(true);
            DOTween.To(() => gameStartScreen.alpha, value => gameStartScreen.alpha = value, 0f, 2f).OnComplete(() =>
            {
                gameStartScreen.gameObject.SetActive(false);
                Time.timeScale = 1;
            }).SetUpdate(true).SetDelay(2);
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private float targetTime = 1;
    private float currentTime = 0;
    private void Update()
    {
        if(gameStartScreenChicken.sprite != playerSpr.sprite)
            gameStartScreenChicken.sprite = playerSpr.sprite;

        if (isNeedTimer)
        {
            currentTime = Time.time;
            if (currentTime > targetTime)
            {
                Timer();
            }
        }
    }

    private void Timer()
    {
        timer--;
        targetTime = currentTime + 1;
    }

    private void Debug() // 초기화
    {
/*        Temp.Instance.CurrentStage = currentStage; 
        Temp.Instance.TempLife = Temp.Instance.GetStage()[Temp.Instance.CurrentStage].stageLife;*/ // 디버그용으로 있는 줄, 게임 시작전에 해주는게 원래 맞음
        timer = Temp.Instance.GetStage()[Temp.Instance.CurrentStage].stageTimer;

        if (Temp.Instance.TempLife == -10) IsInfinityLife = true;

        isNeedTimer = (timer != -10);

        if (timer == -10)
        {
            timer = 999;
        }
    }
}
