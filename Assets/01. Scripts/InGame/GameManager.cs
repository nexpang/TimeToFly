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

    [Header("�÷��̾�")]
    public PlayerController player;
    public Transform playerAnimObj = null;

    [Header("�������")]
    [SerializeField] AudioSource bgAudioSource = null;
    public float defaultBGMvolume = 1;

    SpriteRenderer playerSpr = null;

    [Header("��������")]
    public ChapterInfo[] chapters = null;
    public List<int> remainChickenIndex = new List<int>() { 0, 1, 2, 3, 4 };

    [HideInInspector] public ChapterInfo curChapterInfo;
    [HideInInspector] public StageInfo curStageInfo;

    [HideInInspector] public readonly string tempLifekey = "inGame.tempLife";

    public int tempLife = 0;
    public int timer;
    public float timerScale;

    [Header("��� ������ �����̳�")]
    public Transform prefabContainer;

    [Header("����� �ҽ���")]
    private List<AudioSource> AllSources = new List<AudioSource>();
    public List<AudioSource> BGMSources = new List<AudioSource>();
    public List<AudioSource> SFXSources = new List<AudioSource>();

    [Header("ī�޶�")]
    public CinemachineImpulseSource cinemachineCamera;

    [Header("UI")]
    public Color timerDefaultColor;
    public CanvasGroup bossBar;
    public RectTransform bossBarChicken;
    public RectTransform bossBarFill;
    public Image fadeScreen;

    private void Awake()
    {
        Instance = this; // �̱���
        Time.timeScale = 0;
    }

    private void Start()
    {
        targetTime = Time.time + 1 * timerScale;

        playerSpr = FindObjectOfType<PlayerAnimation>().GetComponent<SpriteRenderer>();

        // ��� �޾ƿ���
        tempLife = SecurityPlayerPrefs.GetInt(tempLifekey, life);
        life = tempLife;
        lifeCount.text = isInfinityLife ? "��" : life.ToString();

        // �Ҹ��� �� �����´�.
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

                            stageName.text = $"STAGE - {curStageInfo.stageName}";

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
                        Debug.LogWarning("�� �������� ����");
                    }
                }
            }
        }

        // ���� �ֵ� Awake���� �ٸ� ��ũ������ �ٲٱ�� �ϵ��� ������ ���� ������ ����

        // ����׿� �ڵ� : ��ŸƮ ��ũ�� ��ŵ �����
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

    public void CameraImpulse(float attack, float sustainTime, float decay, float force = 1)
    {
        cinemachineCamera.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = attack;
        cinemachineCamera.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = sustainTime;
        cinemachineCamera.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = decay;
        cinemachineCamera.GenerateImpulse(force);
    }

    public void SceneReset()
    {
        PoolManager.ResetPool();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    bool isFinished = false;
    public void FadeInOut(float inSec, float outSec, float waitTime = 0f, UnityAction whileFunc = null)
    {
        StartCoroutine(Fade(inSec, outSec, waitTime, whileFunc));
    }

    IEnumerator Fade(float inSec, float outSec, float waitTime = 0f, UnityAction whileFunc = null)
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.color = new Color(0, 0, 0, 0);
        fadeScreen.DOFade(1, inSec).OnComplete(() => isFinished = true);

        yield return new WaitUntil(() => isFinished);

        if (whileFunc != null)
        {
            whileFunc();
        }

        yield return new WaitForSeconds(waitTime);

        isFinished = false;

        fadeScreen.DOFade(0, outSec).OnComplete(() => fadeScreen.gameObject.SetActive(false));
    }
}
