using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class IntroCutScene : MonoBehaviour
{
    public enum CutSceneType
    {
        INTRO,
        ENDING
    }

    public CutSceneType type;

    [Header("Sounds")]
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    public SoundDatas soundData;

    [Space(20)]
    public AudioClip IntroBGM;
    public AudioClip EndingBGM;

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
    public CanvasGroup ending_clockBG;
    public GameObject ending_clockBG_warpEnter;
    public CanvasGroup ending_farmBG;

    [Space(20)]
    public string godnessColor;
    public string[] chickenColors;

    [System.Serializable]
    public struct CutScenes
    {
        public Sprite[] chickenSprites;
    }
    public CutScenes[] ending_cutScenes_sprites;
    public Image[] ending_chickenImgs;


    private string currentText;

    private bool isText = false;

    private bool isTextEnd = false;
    private bool isFinished = false;

    private bool isImageKeep = false;
    private UnityAction action = null;

    private int cutSceneIndex = 0;
    private int chickenIndex = 0;

    private Tweener textTween = null;

    private void Awake()
    {
        SFXSource.mute = !SecurityPlayerPrefs.GetBool("inGame.SFX", true);
        BGMSource.mute = !SecurityPlayerPrefs.GetBool("inGame.BGM", true);

        // TO DO : 엔딩 끝나고 타이틀 갈때 ending false 해줘야됨 + saveMapid도 0으로
        if (SecurityPlayerPrefs.GetBool("inGame.ending", false))
        {
            type = CutSceneType.ENDING;
        }
    }

    void Start()
    {
        chickenIndex = SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1);
        if (chickenIndex == -1) chickenIndex = 3;

        if (type == CutSceneType.INTRO)
        {
            intro.SetActive(true);
            BGMSource.clip = IntroBGM;
            BGMSource.Play();
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
            BGMSource.clip = EndingBGM;
            BGMSource.Play();
            for (int i = 0; i < ending_cutScenes.Length; i++)
            {
                ending_cutScenes[i].alpha = 0;
            }

            for(int i = 0; i< ending_chickenImgs.Length;i++)
            {
                ending_chickenImgs[i].sprite = ending_cutScenes_sprites[i].chickenSprites[chickenIndex];
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
        string chickenColor = chickenColors[chickenIndex];
        string[] chickenNames = new string[5] { "백숙이", "토닭이", "퍼렁이", "딸기", "태일이" };
        string[] chickenCallNames = new string[5] { "백숙아", "토닭아", "퍼렁아", "딸기야", "태일아" };

        yield return new WaitForSeconds(2);
        endingMoveBG.DOAnchorPos(Vector2.zero, 3);
        yield return new WaitForSeconds(4);
        ending_halfBlack.DOFade(138 / 255f, 1);
        yield return new WaitForSeconds(2);

        HidePanel(false, 2f);
        ending_cutScene_bg.gameObject.SetActive(true);
        ending_cutScene_bg.DOFade(0.65f, 2);
        yield return new WaitForSeconds(2);

        ShowText($"<color=\"{godnessColor}\">어머, 너는 누구니?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">저는 {chickenNames[chickenIndex]}에요..</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">여긴 어떻게 온거야?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">어떤 여자가 시간의 힘을 주면서 알려줬어요!</color>", 2f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">뭐? 그녀석은 악마야! 내 힘을 훔쳐간 녀석이라구!\n너를 왜 여기로 보내..</color>", 2.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">..오는 동안 친구들이 모두 죽었어요...\n친구들이 보고 싶어요...</color>", 2.25f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">어머, 친구들이 있었나 보구나.</color>", 1.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">친구들이 보고싶니?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">네...</color>", 0.5f, false, () =>
        {
            ending_clockBG.DOFade(1f, 1);
        });
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">알았어. 지금부터 시간을 되돌릴꺼야.</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">...!</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">너희들이 모두 있었던 시간으로 되돌릴꺼야.\n정신 똑바로 차리고 기억해 내야 해!</color>", 2.5f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">으아아아아!</color>", 0.5f, false, () =>
        {
            ending_clockBG_warpEnter.SetActive(true);
            ending_clockBG.DOFade(0f, 1);
            ending_farmBG.DOFade(1f, 1);
        });
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">(꿈뻑꿈뻑)</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">으으음..</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"어서 일어나 {chickenCallNames[chickenIndex]}! 어서 모험을 떠나야지!", 2f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">맞다! 어서 갈 준비를 해야지!</color>", 1.25f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"자! 날기 위해 모험을 떠나자!!", 1f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 2f);
        ending_cutScene_bg.DOFade(0, 2);
    }

    private IEnumerator EndingSec()
    {
        string chickenColor = chickenColors[chickenIndex];
        string[] chickenNames = new string[5] { "백숙이", "토닭이", "퍼렁이", "딸기", "태일이" };
        string[] chickenCallNames = new string[5] { "백숙아", "토닭아", "퍼렁아", "딸기야", "태일아" };

        yield return new WaitForSeconds(2);
        endingMoveBG.DOAnchorPos(Vector2.zero, 3);
        yield return new WaitForSeconds(4);
        ending_halfBlack.DOFade(138 / 255f, 1);
        yield return new WaitForSeconds(2);

        HidePanel(false, 2f);
        ending_cutScene_bg.gameObject.SetActive(true);
        ending_cutScene_bg.DOFade(0.65f, 2);
        yield return new WaitForSeconds(2);

        ShowText($"<color=\"{godnessColor}\">어머, 또 왔네?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">저를 아세요...?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">당연히 알지!\n돌려보낸 시간이 잘못 됐나 보네..</color>", 1f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">..오는 동안 친구들이 모두 죽었어요...\n친구들이 보고 싶어요...</color>", 2.25f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">어머, 친구들이 있었나 보구나.</color>", 1.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">친구들이 보고싶니?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">네...</color>", 0.5f, false, () =>
        {
            ending_clockBG.DOFade(1f, 1);
        });
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">알았어. 지금부터 시간을 되돌릴꺼야.</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">...!</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">너희들이 모두 있었던 시간으로 되돌릴꺼야.\n정신 똑바로 차리고 기억해 내야 해!</color>", 2.5f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">으아아아아!</color>", 0.5f, false, () =>
        {
            ending_clockBG_warpEnter.SetActive(true);
            ending_clockBG.DOFade(0f, 1);
            ending_farmBG.DOFade(1f, 1);
        });
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">(꿈뻑꿈뻑)</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">으으음..</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"어서 일어나 {chickenCallNames[chickenIndex]}! 어서 모험을 떠나야지!", 2f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">맞다! 어서 갈 준비를 해야지!</color>", 1.25f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"자! 날기 위해 모험을 떠나자!!", 1f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 2f);
        ending_cutScene_bg.DOFade(0, 2);
    }

    private void ShowText(string text, float dur = 1f, bool keepImg = false, UnityAction afterAction = null)
    {
        isText = true;
        isTextEnd = false;
        isImageKeep = keepImg;
        action = afterAction;

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

                if (action != null)
                {
                    action();
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

                if (action != null)
                {
                    action();
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
