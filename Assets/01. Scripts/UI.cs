using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] 
    private Text startTxt = null;
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
        startTxt.DOColor(new Color(1f, 1f, 1f, 10f), 0.8f).SetLoops(-1, LoopType.Yoyo);
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
        SceneManager.LoadScene(1);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

}