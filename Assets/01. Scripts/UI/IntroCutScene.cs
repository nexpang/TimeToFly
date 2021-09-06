using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class IntroCutScene : MonoBehaviour
{
    public enum CutSceneType
    {
        INTRO,
        ENDING
    }

    public CutSceneType type;

    [Header("Objects")]
    public GameObject intro;
    public GameObject ending;

    [Header("Intro")]
    public Image intro_blockPanelAll;
    public Button intro_skipBtn;

    [Space(20)]
    public CanvasGroup intro_msgBox;
    public Text intro_subtitleTxt;
    public CanvasGroup intro_subtitleConfirm;

    [Space(20)]
    public Image[] intro_cutScenes;
    public CanvasGroup intro_godnessBless;
    public Image intro_logoImg;

    [Space(20)]
    public Transform[] intro_cameraPositions;

    [Header("Ending")]
    public Image ending_blockPanelAll;

    [Space(20)]
    public RectTransform endingMoveBG;
    public CanvasGroup ending_msgBox;
    public Text ending_subtitleTxt;
    public CanvasGroup ending_subtitleConfirm;

    [Space(20)]
    public CanvasGroup[] ending_cutScenes;
    public Image ending_halfBlack;
    public Image ending_cutScene_bg;

    private string currentText;

    private bool isText = false;

    private bool isTextEnd = false;
    private bool isFinished = false;
    private bool isImageKeep = false;
    private int cutSceneIndex = 0;

    private Tweener textTween = null;

    private void Awake()
    {
        if(SecurityPlayerPrefs.GetBool("inGame.ending", false))
        {
            type = CutSceneType.ENDING;
        }
    }

    void Start()
    {
        if (type == CutSceneType.INTRO)
        {
            intro.SetActive(true);
            for (int i = 0; i < intro_cutScenes.Length; i++)
            {
                intro_cutScenes[i].color = new Color(1, 1, 1, 0);
            }

            intro_blockPanelAll.color = Color.black;
            intro_blockPanelAll.DOFade(0, 1.5f);
            StartCoroutine(Tutorial());

            if (!SecurityPlayerPrefs.GetBool("newbie", true))
            {
                intro_skipBtn.gameObject.SetActive(true);
                intro_skipBtn.onClick.AddListener(Skip);
            }
        }
        else
        {
            ending.SetActive(true);
            for (int i = 0; i < ending_cutScenes.Length; i++)
            {
                ending_cutScenes[i].alpha = 0;
            }

            ending_blockPanelAll.color = Color.black;
            ending_blockPanelAll.DOFade(0, 1.5f);
            StartCoroutine(Ending());
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
        yield return new WaitForSeconds(3);
        intro_cutScenes[cutSceneIndex].gameObject.SetActive(true);
        intro_cutScenes[cutSceneIndex].DOFade(1, 1);
        yield return new WaitForSeconds(5);
        intro_cutScenes[cutSceneIndex].DOFade(0, 1);

        yield return new WaitForSeconds(2.5f);
        intro_logoImg.gameObject.SetActive(true);
        Camera.main.transform.DOMove(intro_cameraPositions[intro_cameraPositions.Length - 1].transform.position, 2).SetEase(Ease.Linear);
        intro_logoImg.DOFade(1, 1);
        yield return new WaitForSeconds(4);
        intro_logoImg.DOFade(0, 1);
        yield return new WaitForSeconds(2);
        intro_blockPanelAll.DOFade(1, 1.5f);
        yield return new WaitForSeconds(4);

        EndScene();
    }

    private IEnumerator Ending()
    {
        yield return new WaitForSeconds(2);
        endingMoveBG.DOAnchorPos(Vector2.zero, 3);
        yield return new WaitForSeconds(4);
        ending_halfBlack.DOFade(138 / 255f, 1);
        yield return new WaitForSeconds(2);

        HidePanel(false, 2f);
        ending_cutScene_bg.gameObject.SetActive(true);
        ending_cutScene_bg.DOFade(1, 2);
        yield return new WaitForSeconds(2);

        ShowText("<color=\"#ff0000\">�ڸ�1</color>", 2f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("<color=\"#ffff00\">�ڸ�2</color>", 2f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ڸ�2", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ڸ�3", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ڸ�4", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ڸ�5", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ڸ�6", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 2f);
    }

    private void ShowText(string text, float dur = 1f, bool keepImg = false)
    {
        isText = true;
        isTextEnd = false;
        isImageKeep = keepImg;

        currentText = text;

        if (type == CutSceneType.INTRO)
        {
            intro_subtitleConfirm.gameObject.SetActive(false);
            intro_subtitleConfirm.DOKill();
            intro_subtitleConfirm.alpha = 0;
            intro_subtitleTxt.text = "";

            intro_cutScenes[cutSceneIndex].gameObject.SetActive(true);
            intro_cutScenes[cutSceneIndex].DOFade(1, 1);
            intro_godnessBless.DOFade(1, 1);

            textTween = intro_subtitleTxt.DOText(text, dur)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            isTextEnd = true;
                            intro_subtitleConfirm.gameObject.SetActive(true);
                            intro_subtitleConfirm.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
                        });
        }
        else
        {
            ending_subtitleConfirm.gameObject.SetActive(false);
            ending_subtitleConfirm.DOKill();
            ending_subtitleConfirm.alpha = 0;
            ending_subtitleTxt.text = "";

            ending_cutScenes[cutSceneIndex].gameObject.SetActive(true);
            ending_cutScenes[cutSceneIndex].DOFade(1, 1);

            textTween = ending_subtitleTxt.DOText(text, dur)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            isTextEnd = true;
                            ending_subtitleConfirm.gameObject.SetActive(true);
                            ending_subtitleConfirm.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
                        });
        }
    }

    private void HidePanel(bool isHide, float dur = 1f)
    {
        if (type == CutSceneType.INTRO)
        {
            if (isHide)
            {
                intro_msgBox.DOFade(0f, dur);
            }
            else
            {
                intro_msgBox.DOFade(1f, dur);
            }
        }
        else
        {
            if (isHide)
            {
                ending_msgBox.DOFade(0f, dur);
            }
            else
            {
                ending_msgBox.DOFade(1f, dur);
            }
        }
    }

    private void SkipText()
    {
        if (!isText) return;

        if (!isTextEnd && Input.GetMouseButtonDown(0))
        {
            isTextEnd = true;
            textTween.Kill();

            if (type == CutSceneType.INTRO)
            {
                intro_subtitleConfirm.gameObject.SetActive(true);
                intro_subtitleTxt.text = currentText;
                intro_subtitleConfirm.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                ending_subtitleConfirm.gameObject.SetActive(true);
                ending_subtitleTxt.text = currentText;
                ending_subtitleConfirm.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
            }
        }
        else if (isTextEnd && Input.GetMouseButtonDown(0))
        {
            isText = false;

            if (type == CutSceneType.INTRO)
            {
                Camera.main.transform.DOComplete();
                Camera.main.transform.DOMove(intro_cameraPositions[cutSceneIndex + 1].transform.position, 2).SetEase(Ease.Linear);

                if (!isImageKeep)
                {
                    intro_cutScenes[cutSceneIndex].DOFade(0, 1).OnComplete(() =>
                    {
                        isFinished = true;
                        intro_cutScenes[cutSceneIndex].gameObject.SetActive(false);
                        cutSceneIndex++;
                    });
                }
                else
                {
                    isFinished = true;
                }
                intro_godnessBless.DOFade(0, 1);
            }
            else
            {
                if (!isImageKeep)
                {
                    ending_cutScenes[cutSceneIndex].DOFade(0, 1).OnComplete(() =>
                    {
                        isFinished = true;
                        ending_cutScenes[cutSceneIndex].gameObject.SetActive(false);
                        cutSceneIndex++;
                    });
                }
                else
                {
                    isFinished = true;
                }
            }
        }
    }

    public void Skip()
    {
        CancelInvoke();
        intro_blockPanelAll.DOFade(1, 1.5f).OnComplete(() =>
        {
            LoadScene();
        });
        intro_skipBtn.interactable = false;
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
