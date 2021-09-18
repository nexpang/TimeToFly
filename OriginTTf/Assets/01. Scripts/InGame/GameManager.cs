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
    public bool isGameStart { get; private set; } = false;
    [HideInInspector] public bool isBossStage = false;
    [HideInInspector] public bool isBossStart = false;
    [HideInInspector] public bool isCleared = false;

    [Header("�÷��̾�")]
    public PlayerController player;
    public Transform playerAnimObj = null;

    [Header("�������")]
    public AudioSource bgAudioSource = null;
    public float defaultBGMvolume = 1;

    SpriteRenderer playerSpr = null;

    [Header("��������")]
    public ChapterInfo[] chapters = null;
    public List<int> remainChickenIndex = new List<int>() { 0, 1, 2, 3, 4 };

    [HideInInspector] public ChapterInfo curChapterInfo;
    [HideInInspector] public StageInfo curStageInfo;

    [HideInInspector] public readonly string tempLifekey = "inGame.tempLife";
    [HideInInspector] public readonly string remainChickenKey = "inGame.remainChicken";

    public int tempLife = 0;
    public int timer;
    public float timerScale;

    [Header("���� ������ �����̳�")]
    public Transform prefabContainer;

    [Header("ī�޶� �� �ƿ� ����Ʈ")]
    public List<CameraZoomOut> zoomOuts = new List<CameraZoomOut>();

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
    public GameObject bossBarTuto;
    public RectTransform bossBarFill;
    public CanvasGroup todakBossAbilityTip;
    public Button todakBossAbilityTipExit;
    public Image fadeScreen;
    public GameObject cameraLimitWall;
    public RectTransform timeOverUI;
    public RectTransform gameClearUI;
    public GameObject savePointEffect;
    public GameObject savePointEffectTxt;
    public Transform UITutorialPanel;
    private int UITutorialIdx;

    [Header("���� ��ũ��")]
    [SerializeField] CanvasGroup lifeOverScreen = null;

    [Header("����")]
    [SerializeField] ADs ADs = null;

    private void Awake()
    {
        Instance = this; // �̱���
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

        // �Ҹ��� �� �����´�.
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

        // ��� �޾ƿ���
        tempLife = SecurityPlayerPrefs.GetInt(tempLifekey, life);
        life = tempLife;
        lifeCount.text = isInfinityLife ? "��" : life.ToString();

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
                isGameStart = true;

                if (player.abilityNumber == 1 && isBossStage)
                {
                    DOTween.To(() => todakBossAbilityTip.alpha, value => todakBossAbilityTip.alpha = value, 1, 0.5f);
                    todakBossAbilityTip.blocksRaycasts = true;
                    todakBossAbilityTip.interactable = true;
                }
                if (currentStage == 0)
                {
                    UITutorialIdx = 0;
                    PassUITutoPaenl();
                }
            }).SetUpdate(true).SetDelay(2);
        }
        else
        {
            Time.timeScale = 1;

            bgAudioSource.clip = curChapterInfo.chapterBGM;
            bgAudioSource.volume = defaultBGMvolume;
            bgAudioSource.Play();
            isGameStart = true;

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

    public void PassUITutoPaenl()
    {
        if(UITutorialIdx == 0)
        {
            UITutorialPanel.GetChild(0).GetComponent<CanvasGroup>().DOFade(1f, 0.8f);
            UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;
            UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().interactable = true;
        }
        else if(UITutorialIdx == 1)
        {
            UITutorialPanel.GetChild(0).GetComponent<CanvasGroup>().DOKill();
            UITutorialPanel.GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = false;
            UITutorialPanel.GetChild(0).GetComponent<CanvasGroup>().interactable = false;
            UITutorialPanel.GetChild(0).GetComponent<CanvasGroup>().DOFade(0f, 0.8f).OnComplete(() =>
            {
                UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().DOFade(1f, 0.8f);
                UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;
                UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().interactable = true;
            });
        }
        else
        {
            UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().DOKill();
            UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = false;
            UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().interactable = false;
            UITutorialPanel.GetChild(1).GetComponent<CanvasGroup>().DOFade(0f, 0.8f);
        }
        UITutorialIdx++;
    }

    IEnumerator CameraSetting(CinemachineVirtualCamera virtualCamera)
    {
        virtualCamera.Follow = player.transform;
        Time.timeScale = 1;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
        virtualCamera.GetComponent<CinemachineConfiner>().m_Damping = 0;

        yield return new WaitForSecondsRealtime(0.5f);

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

    public void CameraZoomSet()
    {
        float maxValue;
        if (zoomOuts.Count > 0)
            maxValue = zoomOuts[0].zoomValue;
        else
            maxValue = 5;

        for (int i = 0; i< zoomOuts.Count;i++)
        {
            if(maxValue < zoomOuts[i].zoomValue)
            {
                maxValue = zoomOuts[i].zoomValue;
            }
        }

        DOTween.To(() => curStageInfo.virtualCamera.m_Lens.OrthographicSize,
            value => curStageInfo.virtualCamera.m_Lens.OrthographicSize = value, maxValue, 2);
    }

    public void LifeOver()
    {
        PoolManager.ResetPool();
        SceneController.isSavePointChecked = false;
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
        SceneController.targetMapId = SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0);

        SceneReset();
    }

    public void GoToTitle()
    {
        StageReset();

        PoolManager.ResetPool();
        SceneManager.LoadScene("ChickenSelectScene");
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

    public void UserEarnedRewardFromaAD()
    {
        SecurityPlayerPrefs.SetInt("inGame.tempLife", 0);
    }


    [ContextMenu("����� / �÷��̾� ������ ǥ��")]
    public void ShowPlayerPrefs()
    {
        Debug.Log($"inGame.tempLife : {SecurityPlayerPrefs.GetInt("inGame.tempLife", 9)} \n" +
            $"inGame.remainChicken : {SecurityPlayerPrefs.GetString("inGame.remainChicken", "0 1 2 3 4")} \n" +
            $"inGame.saveMapid : {SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0)} \n" +
            $"newbie : {SecurityPlayerPrefs.GetBool("newbie", true)} \n" +
            $"inGame.saveCurrentChickenIndex : {SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)}");
    }
}