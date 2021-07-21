using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleIntro : MonoBehaviour
{
    public bool isNight = true;
    private bool isStarting = false;

    public GameObject dayBG;
    public GameObject nightBG;

    [SerializeField] 
    private Text startTxt = null;
    [SerializeField]
    private Transform chickenTranform = null;
    [SerializeField]
    private Transform cloud = null;
    [SerializeField]
    private Transform logo = null;

    public Transform blackPanelLeft;
    public Transform blackPanelRight;
    public Image blackPanelAll;

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip dayBGM;
    public AudioClip nightBGM;
    public AudioClip gameStartSFX;
    
    private void Awake()
    { 
        Time.timeScale = 1;
        blackPanelLeft.localScale = new Vector3(0,1,1);
        blackPanelRight.localScale = new Vector3(0,1,1);
        dayBG.SetActive(!isNight);
        nightBG.SetActive(isNight);
        bgmSource.clip = isNight ? nightBGM : dayBGM;
        bgmSource.Play();
    }

    void Start()
    {
        startTxt.DOColor(new Color(0f, 0f, 0f), 0.8f).SetLoops(-1, LoopType.Yoyo);
        startTxt.GetComponent<Outline>().DOColor(new Color(1f, 1f, 1f), 0.8f).SetLoops(-1, LoopType.Yoyo);

        chickenTranform.DOMoveY(0.8f, 1.75f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative();
        cloud.DOMoveY(0.1f,2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetRelative();

        LogoRotate(true);
    }

    void LogoRotate(bool right)
    {
        if (right)
        {
            logo.DORotate(new Vector3(0, 0, 10), 2f).SetEase(Ease.InOutSine).OnComplete(() => LogoRotate(!right));
        }
        else
        {
            logo.DORotate(new Vector3(0, 0, -10), 2f).SetEase(Ease.InOutSine).OnComplete(() => LogoRotate(!right));
        }
    }

    private void Update()
    {
        if (isNight)
        {
            if (Input.GetMouseButtonDown(0) && !isStarting)
            {
                isStarting = true;

                sfxSource.PlayOneShot(gameStartSFX, 1);
                DontDestroyOnLoad(bgmSource.gameObject);
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
    }

    public void NextScene()
    {
        SceneManager.LoadScene("IntroAnime");
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

}