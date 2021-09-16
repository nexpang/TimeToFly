using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    Image progressBar;
    Vector2 loadingBarDefaultSize;

    [Header("맵 이미지 / 텍스트")]
    public Text mapName;
    public Image mapImg;
    public Image mapIconimg;

    public ChapterInfoDatas mapData;

    static string nextScene;

    public static bool isLoaded = false;
    public static int targetDieChicken = 0;
    public static int targetMapId = 0;

    // 엔딩
    public static bool isTitleToEnding = false; // 엔딩 보다가 끄고 다시 켜서 타이틀로 넘어갔을때 바로 엔딩으로 넘어갔는가?

    // 세이브 포인트
    public static bool isSavePointChecked = false;
    public static Vector3 savePointPos;

    private void Awake()
    {
        Init();
        loadingBarDefaultSize = progressBar.GetComponent<RectTransform>().sizeDelta;
        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, loadingBarDefaultSize.y);

        int saveMapId = SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0);

        mapName.text = mapData.infos[saveMapId].mapName;
        mapImg.sprite = mapData.infos[saveMapId].mapSprite;
        mapIconimg.sprite = mapData.infos[saveMapId].mapIcon;
    }

    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }


    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        PoolManager.ResetPool();
        SceneManager.LoadScene("Loading");
    }

    public static void Init()
    {
        targetMapId = SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0);
    }


    IEnumerator LoadSceneProgress()
    {
        isLoaded = false;
        yield return new WaitForSeconds(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        if (async == null) yield break;
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            progressBar.rectTransform.sizeDelta = new Vector2(loadingBarDefaultSize.x * async.progress, loadingBarDefaultSize.y);
            if (async.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1);
                isLoaded = true;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
