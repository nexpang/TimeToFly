using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentStage = 0;

    [SerializeField] bool isNeedTimer = true;
    [SerializeField] int life = 5;
    [SerializeField] bool isInfinityLife = false;
    [SerializeField] bool isDebug = false;
    public bool IsInfinityLife { get { return isInfinityLife; } set { isInfinityLife = value; } }
    [SerializeField] CanvasGroup gameStartScreen = null;
    [SerializeField] Image gameStartScreenChicken = null;
    [SerializeField] Text lifeCount = null;

    [Header("플레이어")]
    public PlayerController player;

    [Header("배경음악")]
    [SerializeField] AudioSource bgAudioSource = null;
    public AudioClip defaultBGM = null;
    public float defaultBGMvolume = 1;

    SpriteRenderer playerSpr = null;

    [Header("스테이지")]
    public ChapterInfo[] chapters = null;
    [HideInInspector] public StageInfo stage;

    [HideInInspector] public readonly string tempLifekey = "inGame.tempLife";

    public int tempLife = 0;
    public int timer;
    public float timerScale;

    private void Awake()
    {
        Instance = this; // 싱글톤
    }

    private void Start()
    {
        Time.timeScale = 0;

        playerSpr = FindObjectOfType<PlayerAnimation>().GetComponent<SpriteRenderer>();

        // 목숨 받아오고
        tempLife = SecurityPlayerPrefs.GetInt(tempLifekey, life);
        life = tempLife;
        lifeCount.text = isInfinityLife ? "∞" : life.ToString();

        for (int i = 0; i < chapters.Length; i++)
        {
            if (chapters[i] != null)
            {
                for (int j = 0; j < chapters[i].stageInfos.Count; j++)
                {
                    if (chapters[i].stageInfos[j].stage != null)
                    {
                        if (currentStage == chapters[i].stageInfos[j].stageId)
                        {
                            chapters[i].stageInfos[j].stage.SetActive(true);
                            chapters[i].stageInfos[j].virtualCamera.Follow = player.transform;
                            StartCoroutine(CameraReset(chapters[i].stageInfos[j].virtualCamera));
                            timer = chapters[i].stageInfos[j].stageTimer;
                            stage = chapters[i].stageInfos[j];
                        }
                        else
                        {
                            chapters[i].stageInfos[j].stage.SetActive(false);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("빈 스테이지 감지");
                    }
                }
            }
        }

        // 위에 애들 Awake말고 다른 스크립에서 바꾸기로 하든지 말든지 이제 귀찮다 이제

        // 디버그용 코드 : 스타트 스크린 스킵 디버그
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
        targetTime = currentTime + 1 * timerScale;
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

    IEnumerator CameraReset(CinemachineVirtualCamera virtualCamera)
    {
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
        virtualCamera.GetComponent<CinemachineConfiner>().m_Damping = 0;
        yield return null;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 1;
        virtualCamera.GetComponent<CinemachineConfiner>().m_Damping = 1;
    }
}
