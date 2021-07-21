using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] 
    private Text startTxt = null;
    [SerializeField]
    private Transform chickenTranform = null;
    [SerializeField]
    private Transform cloud = null;
    [SerializeField]
    private Transform logo = null;

    [SerializeField]
    private GameObject exit = null;
    [SerializeField]
    private GameObject clearPanel = null;
    
    

    private void Awake()
    {
        
        Time.timeScale = 1;



    }

    void Start()
    {
        startTxt.DOColor(new Color(0f, 0f, 0f, 10f), 0.8f).SetLoops(-1, LoopType.Yoyo);
        startTxt.GetComponent<Outline>().DOColor(new Color(1f, 1f, 1f, 10f), 0.8f).SetLoops(-1, LoopType.Yoyo);

        /*        chickenTranform.DOScaleY(0.8f, 1.75f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                chickenTranform.DOScaleX(0.7f, 1.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);*/

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit.SetActive(true);
        }
    }

    public void ExitCancleBtn()
    {
        exit.SetActive(false);
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