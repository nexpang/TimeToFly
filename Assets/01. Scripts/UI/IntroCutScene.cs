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

    [Header("��ȭâ")]
    public CanvasGroup msgBox;
    public Text subtitleTxt;
    public CanvasGroup subtitleConfirm;

    [Header("�ƾ��׸�")]
    public Image[] cutScenes;

    [Header("ī�޶� �̵� ������")]
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

        ShowText("���������� ������� �ߵ��� �־���ϴ�.", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("���� �������� ���� �;��� �ߵ��̾�����.", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�׶� ����� �Ǹ��� ã�ƿ� �ߵ鿡�� �� �� �ִ� ����� �ִٰ� �������.", 3.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�Ǹ��� ���� ���� ������ �������� �� �� �ִٸ� �ð��� ���� ���״� �ŵ鿡�Լ� ���Ŀ���� ������.", 4f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ʹ��� ���� �;��� �ߵ��� �Ǹ��� �ٵ� �𸣰� �� ������ �޾Ƶ鿴��ϴ�.", 3.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�̰� �ٷ� �� ���迡�� �Ͼ �ϵ��̿���!", 2f);
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
