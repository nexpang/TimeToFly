using System.Linq;
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
    public float defaultBGMvolume = 1;

    SpriteRenderer playerSpr = null;

    [Header("스테이지")]
    public ChapterInfo[] chapters = null;
    public List<int> remainChickenIndex = new List<int>() { 0, 1, 2, 3, 4 };

    [HideInInspector] public ChapterInfo curChapterInfo;
    [HideInInspector] public StageInfo curStageInfo;

    [HideInInspector] public readonly string tempLifekey = "inGame.tempLife";

    public int tempLife = 0;
    public int timer;
    public float timerScale;

    [Header("블록 아이템 컨테이너")]
    public Transform prefabContainer;

    [Header("오디오 소스들")]
    private List<AudioSource> AllSources = new List<AudioSource>();
    public List<AudioSource> BGMSources = new List<AudioSource>();
    public List<AudioSource> SFXSources = new List<AudioSource>();


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

        // 소리를 다 가져온다.
        AllSources = FindObjectsOfType<AudioSource>().ToList();
        for(int i = 0; i< AllSources.Count;i++)
        {
            if(AllSources[i].outputAudioMixerGroup.name == "BGM")
            {
                BGMSources.Add(AllSources[i]);
            }
            else if (AllSources[i].outputAudioMixerGroup.name == "SFX")
            {
                SFXSources.Add(AllSources[i]);
            }
        }

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
                            curStageInfo = chapters[i].stageInfos[j];
                            curChapterInfo = chapters[i];

                            curStageInfo.stage.SetActive(true);
                            curStageInfo.background.SetActive(true);

                            Camera.main.transform.position = curStageInfo.cameraStartPos;
                            curStageInfo.virtualCamera.transform.position = curStageInfo.virtualCameraStartPos;
                            timer = curStageInfo.stageTimer;
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

                bgAudioSource.clip = curChapterInfo.chapterBGM;
                bgAudioSource.volume = defaultBGMvolume;
                bgAudioSource.Play();
                StartCoroutine(CameraSetting(curStageInfo.virtualCamera));
            }).SetUpdate(true).SetDelay(2);
        }
        else
        {
            bgAudioSource.clip = curChapterInfo.chapterBGM;
            bgAudioSource.volume = defaultBGMvolume;
            bgAudioSource.Play();
            StartCoroutine(CameraSetting(curStageInfo.virtualCamera));
            Time.timeScale = 1;
        }
    }

    IEnumerator CameraSetting(CinemachineVirtualCamera virtualCamera)
    {
        virtualCamera.Follow = player.transform;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
        virtualCamera.GetComponent<CinemachineConfiner>().m_Damping = 0;
        yield return new WaitForSeconds(0.1f);
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 1;
        virtualCamera.GetComponent<CinemachineConfiner>().m_Damping = 1;
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
        if (aS.volume == 0) return;

        aS.clip = clip;
        aS.loop = Looping;
        aS.volume = volume;
        if (!aS.isPlaying) aS.Play();
    }

    public void SetAudioImmediate(AudioSource aS, AudioClip clip, float volume, bool Looping = false)
    {
        if (aS.volume == 0) return;

        aS.loop = Looping;
        aS.volume = volume;
        aS.PlayOneShot(clip);
    }
}
