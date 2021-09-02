using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleIntro : MonoBehaviour
{
    public bool isNight = true;
    private bool isStarting = false;

    [Header("스프라이트")]
    public Image dayNightBG;
    public Sprite dayChicken;
    public Sprite nightChicken;
    public Sprite dayBG;
    public Sprite nightBG;
    public Sprite dayThemeChange;
    public Sprite nightThemeChange;

    [Header("트랜스폼")]
    [SerializeField]
    private Text startTxt = null;
    [SerializeField]
    private Transform chickenTranform = null;
    [SerializeField]
    private Transform logo = null;
    public Transform blackPanelLeft;
    public Transform blackPanelRight;
    public Image blackPanelAll;

    [Header("버튼")]
    public Button themeChangeButton;
    public Button screenButton;

    [Header("사운드")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip dayBGM;
    public AudioClip nightBGM;
    public AudioClip gameStartSFX;

    private void Awake()
    {
        Time.timeScale = 1;
        blackPanelLeft.localScale = new Vector3(0, 1, 1);
        blackPanelRight.localScale = new Vector3(0, 1, 1);
        isNight = SecurityPlayerPrefs.GetBool("newbie", true);

        SecurityPlayerPrefs.SetInt("inGame.tempLife", 9);

        if (SecurityPlayerPrefs.GetBool("newbie", true))
        {
            themeChangeButton.gameObject.SetActive(false);
        }
        themeChangeButton.onClick.AddListener(() =>
        {
            isNight = !isNight;

            if (isNight)
            {
                themeChangeButton.GetComponent<Image>().sprite = nightThemeChange;
            }
            else
            {
                themeChangeButton.GetComponent<Image>().sprite = dayThemeChange;
            }

            ThemeChange(isNight);
        });

        ThemeChange(isNight);

        bgmSource.clip = isNight ? nightBGM : dayBGM;
        bgmSource.Play();
    }

    void Start()
    {
        startTxt.DOColor(new Color(0f, 0f, 0f), 0.8f).SetLoops(-1, LoopType.Yoyo);
        startTxt.GetComponent<Outline>().DOColor(new Color(1f, 1f, 1f), 0.8f).SetLoops(-1, LoopType.Yoyo);

        chickenTranform.DOMoveY(0.3f, 1.75f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative();
        chickenTranform.DOScaleX(0.95f, 1.75f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        chickenTranform.DOScaleY(0.95f, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        LogoRotate(true);

        Invoke("EnableScreen", 3);
    }

    void EnableScreen()
    {
        screenButton.onClick.AddListener(ScreenStart);
    }

    void LogoRotate(bool right)
    {
        if (right)
        {
            logo.DORotate(new Vector3(0, 0, 6), 2f).SetEase(Ease.InOutSine).OnComplete(() => LogoRotate(!right));
        }
        else
        {
            logo.DORotate(new Vector3(0, 0, -6), 2f).SetEase(Ease.InOutSine).OnComplete(() => LogoRotate(!right));
        }
    }

    private void ScreenStart()
    {
        if (!isStarting)
        {
            isStarting = true;

            sfxSource.PlayOneShot(gameStartSFX, 1);
            //DOTween.To(() => bgmSource.volume, value => bgmSource.volume = value, 0, 1);
            blackPanelLeft.DOScaleX(1, 1.5f);
            blackPanelRight.DOScaleX(1, 1.5f);
            logo.GetComponent<Image>().DOFade(0, 2.5f);
            startTxt.DOKill();
            startTxt.GetComponent<Text>().DOFade(0, 2.5f).OnComplete(() =>
            {
                blackPanelAll.DOFade(1, 0.75f).OnComplete(() =>
                {
                    NextScene();
                });
            });
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("IntroAnime");
    }

    private void ThemeChange(bool isNight)
    {
        if (isNight)
        {
            dayNightBG.sprite = nightBG;
            chickenTranform.GetComponent<Image>().sprite = nightChicken;
        }
        else
        {
            dayNightBG.sprite = dayBG;
            chickenTranform.GetComponent<Image>().sprite = dayChicken;
        }
    }



    #region DEBUG_EVENTS

    [ContextMenu("뉴비 온")]
    public void NewbieOn()
    {
        SecurityPlayerPrefs.SetBool("newbie", true);
        SecurityPlayerPrefs.SetInt("inGame.saveMapid", 0);
        SecurityPlayerPrefs.SetString("inGame.remainChicken", "0 1 2 3 4");
        SecurityPlayerPrefs.SetBool("newbie", true);
    }

    [ContextMenu("뉴비 오프")]
    public void NewbieOff()
    {
        SecurityPlayerPrefs.SetBool("newbie", false);
    }
    #endregion
}