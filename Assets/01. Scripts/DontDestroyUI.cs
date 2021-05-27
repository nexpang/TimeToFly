using UnityEngine;
using UnityEngine.SceneManagement;

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
}
