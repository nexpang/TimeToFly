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
        life = SaveManager.Instance.gameData.TempLife;
        lifeCount.text = isInfinityLife ? "∞" : life.ToString();

        DOTween.To(() => gameStartScreen.alpha, value => gameStartScreen.alpha = value, 0f, 2f).OnComplete(() => {
            gameStartScreen.gameObject.SetActive(false);
            Time.timeScale = 1;
        }).SetUpdate(true).SetDelay(2);
    }

    private void Start()
    {
        
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
        SaveManager.Instance.gameData.CurrentStage = currentStage; // 디버그용으로 있는 줄
        SaveManager.Instance.gameData.TempLife = SaveManager.Instance.gameData.GetStage()[SaveManager.Instance.gameData.CurrentStage].stageLife;
        timer = SaveManager.Instance.gameData.GetStage()[SaveManager.Instance.gameData.CurrentStage].stageTimer;

        if (SaveManager.Instance.gameData.TempLife == -10) IsInfinityLife = true;

        isNeedTimer = (timer != -10);

        if (timer == -10)
        {
            timer = 999;
        }
    }
}
