using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    Image progressBar;

    static string nextScene;

    public static bool isLoaded = false;
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
        yield return null;
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        isLoaded = false;
        if (async == null) yield break;
        async.allowSceneActivation = false;

        float timer = 0f;
        while (!async.isDone)
        {
            yield return null;

            if (async.progress < 0.9f)
            {
                yield return null;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    isLoaded = true;
                    async.allowSceneActivation = true;
                    yield break;
                }
            }
            yield return null;
        }
    }
}
