using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;

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
    [SerializeField] Text stageName = null;
    [SerializeField] Text lifeCount = null;
    [HideInInspector] public bool isBossStage = false;
    [HideInInspector] public bool isBossStart = false;
    [HideInInspector] public bool isCleared = false;

    [Header("플레이어")]
    public PlayerController player;
    public Transform playerAnimObj = null;

    [Header("배경음악")]
    public AudioSource bgAudioSource = null;
    public float defaultBGMvolume = 1;

    SpriteRenderer playerSpr = null;

    [Header("스테이지")]
    public ChapterInfo[] chapters = null;
    public List<int> remainChickenIndex = new List<int>() { 0, 1, 2, 3, 4 };

    [HideInInspector] public ChapterInfo curChapterInfo;
    [HideInInspector] public StageInfo curStageInfo;

    [HideInInspector] public readonly string tempLifekey = "inGame.tempLife";
    [HideInInspector] public readonly string remainChickenKey = "inGame.remainChicken";

    public int tempLife = 0;
    public int timer;
    public float timerScale;

    [Header("블록 아이템 컨테이너")]
    public Transform prefabContainer;

    [Header("오디오 소스들")]
    private List<AudioSource> AllSources = new List<AudioSource>();
    public List<AudioSource> BGMSources = new List<AudioSource>();
    public List<AudioSource> SFXSources = new List<AudioSource>();

    [Header("카메라")]
    public CinemachineImpulseSource cinemachineCamera;

    [Header("UI")]
    public Color timerDefaultColor;
    public CanvasGroup bossBar;
    public RectTransform bossBarChicken;
    public RectTransform bossBarFill;
    public CanvasGroup todakBossAbilityTip;
    public Button todakBossAbilityTipExit;
    public Image fadeScreen;
    public GameObject cameraLimitWall;
    public RectTransform timeOverUI;
    public RectTransform gameClearUI;

    [Header("데스 스크린")]
    [SerializeField] CanvasGroup lifeOverScreen = null;

    private void Awake()
    {
        Instance = this; // 싱글톤
        Time.timeScale = 0;

        if (currentStage == -1)
        {
            currentStage = SceneController.targetMapId;
        }

        if (currentStage == 0)
        {
            isNeedTimer = false;
            isInfinityLife = true;
        }

        // 소리를 다 가져온다.
        AllSources = FindObjectsOfType<AudioSource>().ToList();
        for (int i = 0; i < AllSources.Count; i++)
        {
            if (AllSources[i].outputAudioMixerGroup.name == "BGM")
            {
                BGMSources.Add(AllSources[i]);
            }
            else if (AllSources[i].outputAudioMixerGroup.name == "SFX")
            {
                SFXSources.Add(AllSources[i]);
            }
        }
    }

    private void Start()
    {
        targetTime = Time.time + 1 * timerScale;

        playerSpr = FindObjectOfType<PlayerAnimation>().GetComponent<SpriteRenderer>();

        // 목숨 받아오고
        tempLife = SecurityPlayerPrefs.GetInt(tempLifekey, life);
        life = tempLife;
        lifeCount.text = isInfinityLife ? "∞" : life.ToString();

        LoadRemainChicken();

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

                            stageName.text = $"STAGE - {curStageInfo.stageName}";

                            StartCoroutine(CameraSetting(curStageInfo.virtualCamera));
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

                if(player.abilityNumber == 1 && isBossStage)
                {
                    DOTween.To(() => todakBossAbilityTip.alpha, value => todakBossAbilityTip.alpha = value, 1, 0.5f);
                    todakBossAbilityTip.blocksRaycasts = true;
                    todakBossAbilityTip.interactable = true;
                }

            }).SetUpdate(true).SetDelay(2);
        }
        else
        {
            Time.timeScale = 1;

            bgAudioSource.clip = curChapterInfo.chapterBGM;
            bgAudioSource.volume = defaultBGMvolume;
            bgAudioSource.Play();

            if (player.abilityNumber == 1 && isBossStage)
            {
                DOTween.To(() => todakBossAbilityTip.alpha, value => todakBossAbilityTip.alpha = value, 1, 0.5f);
                todakBossAbilityTip.blocksRaycasts = true;
                todakBossAbilityTip.interactable = true;
            }
        }

        todakBossAbilityTipExit.onClick.AddListener(() =>
        {
            DOTween.To(() => todakBossAbilityTip.alpha, value => todakBossAbilityTip.alpha = value, 0, 0.5f);
            todakBossAbilityTip.blocksRaycasts = false;
            todakBossAbilityTip.interactable = false;
        });
    }

    IEnumerator CameraSetting(CinemachineVirtualCamera virtualCamera)
    {
        virtualCamera.Follow = player.transform;
        Time.timeScale = 1;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
        virtualCamera.GetComponent<CinemachineConfiner>().m_Damping = 0;
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = 0;
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

        if(timer<=0)
        {
            player.TimeOver();
        }
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

    public void CameraImpulse(float attack, float sustainTime, float decay, float force = 1)
    {
        cinemachineCamera.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = attack;
        cinemachineCamera.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = sustainTime;
        cinemachineCamera.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = decay;
        cinemachineCamera.GenerateImpulse(force);
    }

    public void LifeOver()
    {
        PoolManager.ResetPool();
        player.deathScreen.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        DOTween.To(() => lifeOverScreen.alpha, value => lifeOverScreen.alpha = value, 1, 0.5f).OnComplete(() =>
        {
            lifeOverScreen.interactable = true;
            lifeOverScreen.blocksRaycasts = true;
        });
    }

    public void SceneReset()
    {
        PoolManager.ResetPool();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ChapterAgain()
    {
        StageReset();
        SceneReset();
    }

    public void GoToTitle()
    {
        StageReset();

        PoolManager.ResetPool();
        SceneManager.LoadScene("Title");
    }

    public void StageReset()
    {
        if (!IsInfinityLife) tempLife = 9;
        SecurityPlayerPrefs.SetInt(tempLifekey, tempLife);
        SecurityPlayerPrefs.SetInt("inGame.saveMapid", curChapterInfo.stageInfos[0].stageId);
    }

    bool isFinished = false;
    public void FadeInOut(float inSec, float outSec, float waitTime = 0f, UnityAction whileFunc = null)
    {
        StartCoroutine(Fade(inSec, outSec, waitTime, whileFunc));
    }

    IEnumerator Fade(float inSec, float outSec, float waitTime = 0f, UnityAction whileFunc = null)
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.DOFade(1, inSec).OnComplete(() => isFinished = true).SetUpdate(UpdateType.Normal, true);

        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        yield return new WaitForSecondsRealtime(waitTime / 2);

        if (whileFunc != null)
        {
            whileFunc();
        }

        yield return new WaitForSecondsRealtime(waitTime / 2);

        fadeScreen.DOFade(0, outSec).OnComplete(() => fadeScreen.gameObject.SetActive(false)).SetUpdate(true);
    }

    public void LoadRemainChicken()
    {
        string chicken = SecurityPlayerPrefs.GetString(remainChickenKey, "0 1 2 3 4");

        string[] chickens = chicken.Split(' ');
        remainChickenIndex.Clear();

        for (int i = 0; i < chickens.Length; i++)
        {
            remainChickenIndex.Add(int.Parse(chickens[i]));
        }
    }
     
    public void SaveRemainChicken()
    {
        string chicken = "";

        for(int i = 0; i < remainChickenIndex.Count;i++)
        {
            chicken += remainChickenIndex[i].ToString();

            if(i != remainChickenIndex.Count - 1)
            {
                chicken += " ";
            }
        }

        SecurityPlayerPrefs.SetString(remainChickenKey, chicken);
    }

    public void RemoveRemainChicken(int index)
    {
        remainChickenIndex.Remove(index);
        SceneController.targetDieChicken = index;
        SaveRemainChicken();
    }


    [ContextMenu("디버그 / 플레이어 프랩스 표시")]
    public void ShowPlayerPrefs()
    {
        Debug.Log($"inGame.tempLife : {SecurityPlayerPrefs.GetInt("inGame.tempLife", 9)} \n" +
            $"inGame.remainChicken : {SecurityPlayerPrefs.GetString("inGame.remainChicken", "0 1 2 3 4")} \n" +
            $"inGame.saveMapid : {SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0)} \n" +
            $"newbie : {SecurityPlayerPrefs.GetBool("newbie", true)} \n" +
            $"inGame.saveCurrentChickenIndex : {SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)}");
    }
}
