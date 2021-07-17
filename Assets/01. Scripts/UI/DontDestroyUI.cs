using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroyUI : MonoBehaviour
{
    public static DontDestroyUI Instance;
    [SerializeField]
    private CanvasGroup clearCG;
    [SerializeField]
    private Canvas clearCanvas;
    [SerializeField]
    private GameObject clear;
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Camera main;
    [SerializeField]
    private Button soundBtn;

    private bool isMute;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
       
    }
    private void Start()
    {
        clearCG = GetComponent<CanvasGroup>();
        main = GameObject.Find("Main Camera").GetComponent<Camera>();
        soundBtn = GameObject.Find("MuteButton").GetComponent<Button>();
        if (!main)
        {
            clearCanvas.worldCamera = main;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (clearCG.alpha == 1)
            {
                Time.timeScale = 1f;
                clearCG.alpha = 0f;
                clear.SetActive(false);
                menu.SetActive(false);
                clearCG.blocksRaycasts = false;
            }
            else
            {
                Time.timeScale = 0f;
                clearCG.alpha = 1f;
                clear.SetActive(true);
                clearCG.blocksRaycasts = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (clearCG.alpha == 1)
            {
                Time.timeScale = 1f;
                clearCG.alpha = 0f;
                menu.SetActive(false);
                clear.SetActive(false);
                clearCG.blocksRaycasts = false;
            }
            else
            {
                Time.timeScale = 0f;
                clearCG.alpha = 1f;
                menu.SetActive(true);
                clearCG.blocksRaycasts = true;
            }
        }

    }

    public void OnClickExitBtn()
    {
        Application.Quit();
    }
    public void OnClickContinueBtn() 
    {
        if (clearCG.alpha == 1)
        {
            Time.timeScale = 1f;
            clearCG.alpha = 0f;
            menu.SetActive(false);
            clear.SetActive(false);
            clearCG.blocksRaycasts = false;
        }
    }
    public void OnClickTitleBtn() 
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Title");

    }
    public void SoundOnOffBtn()
    {
        if (isMute)
        {
            isMute = false;
            PlayerPrefs.SetInt("Mute", 0);
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            AudioListener.volume = 1;
        }
        else
        {
            isMute = true;
            PlayerPrefs.SetInt("Mute", 1);
            soundBtn.GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            AudioListener.volume = 0;
        }
    }
}
