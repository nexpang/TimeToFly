using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    Image progressBar;

    Vector2 loadingBarDefaultSize;

    static string nextScene;

    public static bool isLoaded = false;

    private void Awake()
    {
        loadingBarDefaultSize = progressBar.GetComponent<RectTransform>().sizeDelta;
        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, loadingBarDefaultSize.y);
    }

    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }


    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
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
