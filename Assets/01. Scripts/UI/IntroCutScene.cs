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
    public Image ending_logoImg;
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
    public Image[] ending_subImages;


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

        chickenIndex = SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1);
        if (chickenIndex == -1) chickenIndex = 3;

        // TO DO : 엔딩 컷씬보려면 ending true후 씬 변경 + 엔딩 끝나고 타이틀 갈때 ending false 해줘야됨 + saveMapid도 0으로, 닭도 원래상태로
        if (SecurityPlayerPrefs.GetBool("inGame.ending", false))
        {
            type = CutSceneType.ENDING;

            if (!SceneController.isTitleToEnding)
            {
                int bakSukEndingCount = SecurityPlayerPrefs.GetInt("inGame.bakSukEndingCount", 0);
                int otherEndingCount = SecurityPlayerPrefs.GetInt("inGame.otherEndingCount", 0);

                if (chickenIndex == 0)
                {
                    SecurityPlayerPrefs.SetInt("inGame.bakSukEndingCount", bakSukEndingCount + 1);
                }
                else
                {
                    SecurityPlayerPrefs.SetInt("inGame.otherEndingCount", otherEndingCount + 1);
                }
            }
        }

    }

    void Start()
    {
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

            if (chickenIndex == 0 && SecurityPlayerPrefs.GetInt("inGame.bakSukEndingCount", 0) > 1
                || chickenIndex != 0 && SecurityPlayerPrefs.GetInt("inGame.otherEndingCount", 0) > 1)
            {
                StartCoroutine(EndingSec());
            }
            else
            {
                StartCoroutine(Ending());
            }
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
        intro_logoImg.DOFade(1, 5);
        yield return new WaitForSeconds(9);
        intro_logoImg.DOFade(0, 5);
        yield return new WaitForSeconds(7);
        intro_blockPanelAll.DOFade(1, 1.5f);
        DOTween.To(() => BGMSource.volume, value => BGMSource.volume = value, 0, 1.5f);
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

        ShowText($"<color=\"{godnessColor}\">너희들이 모두 있었던 시간으로 되돌릴꺼야.\n기억할 수 있을지 모르겠지만 정신 똑바로 차리고 기억해 내야 해!</color>", 2.5f, false);
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

        if (chickenIndex != 0)
        {
            ShowText($"<color=\"{chickenColor}\">맞다! 어서 갈 준비를 해야지!</color>", 1.25f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
        }
        else
        {
            ShowText($"<color=\"{chickenColor}\">(음? 모두가 죽었던 것 같은데 꿈이었나..?)</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"어서 가자니까?", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">음.. 그래, 어서 가자!</color>", 0.5f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
        }

        ShowText($"자! 날기 위해 모험을 떠나자!!", 1f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 2f);
        ending_cutScene_bg.DOFade(0, 2);
        yield return new WaitForSeconds(2.5f);
        ending_logoImg.gameObject.SetActive(true);
        ending_logoImg.DOFade(1, 5);
        yield return new WaitForSeconds(9);
        ending_logoImg.DOFade(0, 5);
        yield return new WaitForSeconds(7);
        ending_blockPanelAll.DOFade(1, 1.5f);
        DOTween.To(() => BGMSource.volume, value => BGMSource.volume = value, 0, 4f);
        yield return new WaitForSeconds(4);

        EndingEnd();
    }

    private IEnumerator EndingSec() // 닭들이 각각 두번쨰 엔딩일때
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

        if (chickenIndex == 0)
        {
            // 1번 씬
            ShowText($"<color=\"{godnessColor}\">어머, 또 왔네?</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">네... 다시 왔어요.. 어라, 이번이 한번이 아니군요?</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">음.. 역시나 니 능력은 모든 시간을 기억하는건가 보구나?</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">제 능력이 보이지 않던게 그것 때문이었나요?\n그것마저 여기에 와서 알게 되다니 얼마나 힘들었다고요...\n그리고 그런 일들을 기억을 못할리 없잖아요...</color>", 5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">음... 미안, 많이 힘들었지?</color>", 1.5f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 2번 씬
            ShowText($"<color=\"{chickenColor}\">다시 시간을 돌려주세요! 이번에는 친구들을 죽게 두지 않을꺼에요..</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">음.. 아마도 힘들꺼야. 지금 니가 여기에 와서 기억을 떠올린것도 그렇고,\n어차피 너희가 모험을 떠나는 이상 친구들은 모두 죽을꺼야.</color>", 4f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">그렇다면 모험을 떠나지 않으면 되잖아요!</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">너희 꿈이 나는거 아니였어? 너희 친구들이 다시 모험을 안 떠나게 말릴 수 있어?</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">네...?</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">니가 모험을 떠나지 않아도, 친구들은 여행을 떠날꺼야.</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">그러면 어떻게 해요...</color>", 1f, false, () =>
            {
                ending_clockBG.DOFade(1f, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 3번 씬
            ShowText($"<color=\"{godnessColor}\">좋아, 시간을 되돌려 줄게.</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">하지만 미래를 바꿀수 없다고 하셨잖아요!</color>", 1.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">정신 똑바로 차려! 슬픈 일이 일어나지 않는 방법을 알고 있어.\n..대신 기억 해내야 해.</color>", 2f, true, () => {
                ending_subImages[0].gameObject.SetActive(true);
                ending_subImages[0].DOFade(1, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">..이건 선물이야.</color>", 1f, false, () => {
                ending_subImages[0].DOFade(0, 1).OnComplete(() =>
                {
                    ending_subImages[0].gameObject.SetActive(false);
                });
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 4번 씬
            ShowText($"<color=\"{chickenColor}\">으아아아아!</color>", 0.5f, false, () =>
            {
                ending_clockBG_warpEnter.SetActive(true);
                ending_clockBG.DOFade(0f, 1);
                ending_farmBG.DOFade(1f, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 5번 씬
            ShowText($"<color=\"{chickenColor}\">으으음..</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"어서 일어나 {chickenCallNames[chickenIndex]}! 어서 모험을 떠나야지!", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">(음? 모두가 죽었던 것 같은데 꿈이었나..?)</color>", 1.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"어서 가자니까?", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">음.. 그래, 어서.....</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">..잠깐만.</color>", 0.5f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 6번 씬
            ShowText($"무슨 일이야?", 0.5f, true); // 이미지가 다르다고 합니다
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">...나는.. 여행의 끝에서 왔어.</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"무슨 소리야?", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">다들 나는 연습을 하러 가보자!</color>", 1.25f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 7번 씬
            ShowText($"<color=\"{chickenColor}\">우리 모두 날 수 있어!!</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"무슨 말이야?", 0.5f, true); // 나는 애니메이션 놓고 트루 엔딩
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
        }
        else
        {
            ShowText($"<color=\"{godnessColor}\">어머, 닭이 또 왔네?</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">저를 아세요...?</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">음...\n역시 기억을 못하는구나..</color>", 1f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">..오는 동안 친구들이 모두 죽었어요...\n친구들이 보고 싶어요...</color>", 2.25f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">이번에도 모두 죽었나 보구나...</color>", 1.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">이번에도 라니요..?</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">음 아니야.. 친구들이 보고싶니?</color>", 1.5f, true);
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
        }

        HidePanel(true, 2f);
        ending_cutScene_bg.DOFade(0, 2);
        yield return new WaitForSeconds(2.5f);
        ending_logoImg.gameObject.SetActive(true);
        ending_logoImg.DOFade(1, 5);
        yield return new WaitForSeconds(9);
        ending_logoImg.DOFade(0, 5);
        yield return new WaitForSeconds(7);
        ending_blockPanelAll.DOFade(1, 1.5f);
        DOTween.To(() => BGMSource.volume, value => BGMSource.volume = value, 0, 4f);
        yield return new WaitForSeconds(4);

        EndingEnd();
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

    void EndingEnd()
    {
        SecurityPlayerPrefs.SetBool("inGame.ending", false);
        SceneController.targetMapId = 0;
        SceneController.isTitleToEnding = false;
        SecurityPlayerPrefs.SetInt("inGame.saveMapid", 0);
        SecurityPlayerPrefs.SetString("inGame.remainChicken", "0 1 2 3 4");
        SecurityPlayerPrefs.SetInt("inGame.saveCurrentChickenIndex", -1);
        PoolManager.ResetPool();
        SceneManager.LoadScene("Title");
    }

    void LoadScene()
    { 
        if (SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1) == -1 && SceneController.targetMapId != 0)
        {
            PoolManager.ResetPool();
            SceneManager.LoadScene("ChickenSelectScene");
        }
        else
        {
            SceneController.LoadScene("InGame");
        }
    }

}