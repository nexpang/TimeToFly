using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class IntroCutScene : MonoBehaviour
{
    public Image blockPanelAll;
    public Button skipBtn;

    [Header("대화창")]
    public CanvasGroup msgBox;
    public Text subtitleTxt;
    public CanvasGroup subtitleConfirm;

    [Header("컷씬그림")]
    public Image[] cutScenes;

    [Header("카메라 이동 포지션")]
    public Transform[] cameraPositions;

    private string currentText;

    private bool isText = false;

    private bool isTextEnd = false;
    private bool isFinished = false;
    private int cutSceneIndex = 0;

    private Tweener textTween = null;

    private void Awake()
    {
        for(int i = 0; i < cutScenes.Length;i++)
        {
            cutScenes[i].color = new Color(1, 1, 1, 0);
        }
    }

    void Start()
    {
        blockPanelAll.color = Color.black;
        blockPanelAll.DOFade(0, 1.5f);
        StartCoroutine(Tutorial());

        if (!SecurityPlayerPrefs.GetBool("newbie", true))
        {
            skipBtn.gameObject.SetActive(true);
            skipBtn.onClick.AddListener(Skip);
        }
    }

    private void Update()
    {
        SkipText();
    }

    private IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(7);
        HidePanel(false, 2f);
        yield return new WaitForSeconds(2);

        ShowText("옛날옛적에 날고싶은 닭들이 있었답니다.", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("정말 간절히도 날고 싶었던 닭들이었지요.", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("그때 사악한 악마가 찾아와 닭들에게 날 수 있는 방법이 있다고 꼬드겼어요.", 3.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("악마는 신의 날개 깃털을 가져가면 날 수 있다며 시간의 힘을 줄테니 신들에게서 훔쳐오라고 말했죠.", 4f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("너무나 날고 싶었던 닭들은 악마인 줄도 모르고 그 모험을 받아들였답니다.", 3.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("이건 바로 그 모험에서 일어난 일들이에요!", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 2f);
        yield return new WaitForSeconds(5);

        EndScene();
    }

    private void ShowText(string text, float dur = 1f)
    {
        subtitleConfirm.gameObject.SetActive(false);
        subtitleConfirm.DOKill();
        subtitleConfirm.alpha = 0;

        cutScenes[cutSceneIndex].gameObject.SetActive(true);
        cutScenes[cutSceneIndex].DOFade(1, 1);

        isText = true;
        isTextEnd = false;

        currentText = text;

        subtitleTxt.text = "";
        textTween = subtitleTxt.DOText(text, dur)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        isTextEnd = true;
                        subtitleConfirm.gameObject.SetActive(true);
                        subtitleConfirm.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
                    });
    }

    private void HidePanel(bool isHide, float dur = 1f)
    {
        if (isHide)
        {
            msgBox.DOFade(0f, dur);
        }
        else
        {
            msgBox.DOFade(1f, dur);
        }
    }

    private void SkipText()
    {
        if (!isText) return;

        if (!isTextEnd && Input.GetMouseButtonDown(0))
        {
            isTextEnd = true;
            subtitleConfirm.gameObject.SetActive(true);

            textTween.Kill();
            subtitleTxt.text = currentText;
            subtitleConfirm.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
        }
        else if (isTextEnd && Input.GetMouseButtonDown(0))
        {
            isText = false;

            Camera.main.transform.DOComplete();
            Camera.main.transform.DOMove(cameraPositions[cutSceneIndex + 1].transform.position, 2).SetEase(Ease.Linear);

            cutScenes[cutSceneIndex].DOFade(0, 1).OnComplete(() =>
             {
                 isFinished = true;

                 cutScenes[cutSceneIndex].gameObject.SetActive(false);
                 cutSceneIndex++;
             });
        }
    }

    public void Skip()
    {
        CancelInvoke();
        blockPanelAll.DOFade(1, 1.5f).OnComplete(() =>
        {
            LoadScene();
        });
        skipBtn.interactable = false;
    }

    void EndScene()
    {
        SecurityPlayerPrefs.SetBool("newbie", false);
        LoadScene();
    }

    void LoadScene()
    { 
        if (SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1) == -1 && SceneController.targetMapId != 0)
        {
            SceneController.LoadScene("ChickenSelectScene");
        }
        else
        {
            SceneController.LoadScene("InGame");
        }
    }

}
