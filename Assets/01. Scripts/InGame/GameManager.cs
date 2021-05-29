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

    [Header("배경음악")]
    [SerializeField] AudioSource bgAudioSource = null;
    public AudioClip defaultBGM = null;
    public float defaultBGMvolume = 1;

    SpriteRenderer playerSpr = null;

    [Header("스테이지")]
    [SerializeField] GameObject[] stages = null;


    [HideInInspector] public Tween tween; // 시계 없어지는거 방지용 트윈


    private int _timer;
    public int timer
    {
        get { return _timer; }
        set { _timer = value; }
    }

    private float timerScale = 1;
    public float TimerScale
    {
        get { return timerScale; }
        set { timerScale = value; }
    }

    private void Awake()
    {
        stages[0].SetActive(true);

        Debug();

        Time.timeScale = 0;
        Instance = this; // 싱글톤

        playerSpr = FindObjectOfType<PlayerAnimation>().GetComponent<SpriteRenderer>();

        // 목숨 받아오고
        life = Temp.Instance.TempLife;
        lifeCount.text = isInfinityLife ? "∞" : life.ToString();

        // 디버그용 코드
        if (!isDebug)
        {
            gameStartScreen.gameObject.SetActive(true);
            DOTween.To(() => gameStartScreen.alpha, value => gameStartScreen.alpha = value, 0f, 2f).OnComplete(() =>
            {
                gameStartScreen.gameObject.SetActive(false);
                Time.timeScale = 1;

                bgAudioSource.clip = defaultBGM;
                bgAudioSource.volume = defaultBGMvolume;
                bgAudioSource.Play();

            }).SetUpdate(true).SetDelay(2);
        }
        else
        {
            bgAudioSource.clip = defaultBGM;
            bgAudioSource.volume = defaultBGMvolume;
            bgAudioSource.Play();
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
        else timer = 999;
    }

    public void Timer()
    {
        timer--;
        targetTime = currentTime + 1 * TimerScale;
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

    public void SetAudio(AudioSource aS, AudioClip clip, float volume, bool Looping = false)
    {
        aS.clip = clip;
        aS.loop = Looping;
        aS.volume = volume;
        if (!aS.isPlaying) aS.Play();
    }

    public void SetAudioImmediate(AudioSource aS, AudioClip clip, float volume, bool Looping = false)
    {
        aS.loop = Looping;
        aS.volume = volume;
        aS.PlayOneShot(clip);
    }
}
