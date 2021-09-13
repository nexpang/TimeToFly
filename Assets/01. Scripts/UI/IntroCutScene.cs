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

        // TO DO : ���� �ƾ������� ending true�� �� ���� + ���� ������ Ÿ��Ʋ ���� ending false ����ߵ� + saveMapid�� 0����, �ߵ� �������·�
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
        string[] chickenNames = new string[5] { "�����", "�����", "�۷���", "����", "������" };
        string[] chickenCallNames = new string[5] { "�����", "��߾�", "�۷���", "�����", "���Ͼ�" };

        yield return new WaitForSeconds(2);
        endingMoveBG.DOAnchorPos(Vector2.zero, 3);
        yield return new WaitForSeconds(4);
        ending_halfBlack.DOFade(138 / 255f, 1);
        yield return new WaitForSeconds(2);

        HidePanel(false, 2f);
        ending_cutScene_bg.gameObject.SetActive(true);
        ending_cutScene_bg.DOFade(0.65f, 2);
        yield return new WaitForSeconds(2);

        ShowText($"<color=\"{godnessColor}\">���, �ʴ� ������?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">���� {chickenNames[chickenIndex]}����..</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">���� ��� �°ž�?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">� ���ڰ� �ð��� ���� �ָ鼭 �˷�����!</color>", 2f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">��? �׳༮�� �Ǹ���! �� ���� ���İ� �༮�̶�!\n�ʸ� �� ����� ����..</color>", 2.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">..���� ���� ģ������ ��� �׾����...\nģ������ ���� �;��...</color>", 2.25f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">���, ģ������ �־��� ������.</color>", 1.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">ģ������ ����ʹ�?</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">��...</color>", 0.5f, false, () =>
        {
            ending_clockBG.DOFade(1f, 1);
        });
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">�˾Ҿ�. ���ݺ��� �ð��� �ǵ�������.</color>", 1f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">...!</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{godnessColor}\">������� ��� �־��� �ð����� �ǵ�������.\n����� �� ������ �𸣰����� ���� �ȹٷ� ������ ����� ���� ��!</color>", 2.5f, false);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">���ƾƾƾ�!</color>", 0.5f, false, () =>
        {
            ending_clockBG_warpEnter.SetActive(true);
            ending_clockBG.DOFade(0f, 1);
            ending_farmBG.DOFade(1f, 1);
        });
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">(�޻��޻�)</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"<color=\"{chickenColor}\">������..</color>", 0.5f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText($"� �Ͼ {chickenCallNames[chickenIndex]}! � ������ ��������!", 2f, true);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        if (chickenIndex != 0)
        {
            ShowText($"<color=\"{chickenColor}\">�´�! � �� �غ� �ؾ���!</color>", 1.25f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
        }
        else
        {
            ShowText($"<color=\"{chickenColor}\">(��? ��ΰ� �׾��� �� ������ ���̾���..?)</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"� ���ڴϱ�?", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">��.. �׷�, � ����!</color>", 0.5f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
        }

        ShowText($"��! ���� ���� ������ ������!!", 1f, false);
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

    private IEnumerator EndingSec() // �ߵ��� ���� �ι��� �����϶�
    {
        string chickenColor = chickenColors[chickenIndex];
        string[] chickenNames = new string[5] { "�����", "�����", "�۷���", "����", "������" };
        string[] chickenCallNames = new string[5] { "�����", "��߾�", "�۷���", "�����", "���Ͼ�" };

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
            // 1�� ��
            ShowText($"<color=\"{godnessColor}\">���, �� �Գ�?</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">��... �ٽ� �Ծ��.. ���, �̹��� �ѹ��� �ƴϱ���?</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">��.. ���ó� �� �ɷ��� ��� �ð��� ����ϴ°ǰ� ������?</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">�� �ɷ��� ������ �ʴ��� �װ� �����̾�����?\n�װ͸��� ���⿡ �ͼ� �˰� �Ǵٴ� �󸶳� ������ٰ��...\n�׸��� �׷� �ϵ��� ����� ���Ҹ� ���ݾƿ�...</color>", 5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">��... �̾�, ���� �������?</color>", 1.5f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 2�� ��
            ShowText($"<color=\"{chickenColor}\">�ٽ� �ð��� �����ּ���! �̹����� ģ������ �װ� ���� ����������..</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">��.. �Ƹ��� ���鲨��. ���� �ϰ� ���⿡ �ͼ� ����� ���ø��͵� �׷���,\n������ ���� ������ ������ �̻� ģ������ ��� ��������.</color>", 4f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">�׷��ٸ� ������ ������ ������ ���ݾƿ�!</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">���� ���� ���°� �ƴϿ���? ���� ģ������ �ٽ� ������ �� ������ ���� �� �־�?</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">��...?</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">�ϰ� ������ ������ �ʾƵ�, ģ������ ������ ��������.</color>", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">�׷��� ��� �ؿ�...</color>", 1f, false, () =>
            {
                ending_clockBG.DOFade(1f, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 3�� ��
            ShowText($"<color=\"{godnessColor}\">����, �ð��� �ǵ��� �ٰ�.</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">������ �̷��� �ٲܼ� ���ٰ� �ϼ��ݾƿ�!</color>", 1.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">���� �ȹٷ� ����! ���� ���� �Ͼ�� �ʴ� ����� �˰� �־�.\n..��� ��� �س��� ��.</color>", 2f, true, () => {
                ending_subImages[0].gameObject.SetActive(true);
                ending_subImages[0].DOFade(1, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">..�̰� �����̾�.</color>", 1f, false, () => {
                ending_subImages[0].DOFade(0, 1).OnComplete(() =>
                {
                    ending_subImages[0].gameObject.SetActive(false);
                });
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 4�� ��
            ShowText($"<color=\"{chickenColor}\">���ƾƾƾ�!</color>", 0.5f, false, () =>
            {
                ending_clockBG_warpEnter.SetActive(true);
                ending_clockBG.DOFade(0f, 1);
                ending_farmBG.DOFade(1f, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 5�� ��
            ShowText($"<color=\"{chickenColor}\">������..</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"� �Ͼ {chickenCallNames[chickenIndex]}! � ������ ��������!", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">(��? ��ΰ� �׾��� �� ������ ���̾���..?)</color>", 1.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"� ���ڴϱ�?", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">��.. �׷�, �.....</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">..���.</color>", 0.5f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 6�� ��
            ShowText($"���� ���̾�?", 0.5f, true); // �̹����� �ٸ��ٰ� �մϴ�
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">...����.. ������ ������ �Ծ�.</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"���� �Ҹ���?", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">�ٵ� ���� ������ �Ϸ� ������!</color>", 1.25f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            // 7�� ��
            ShowText($"<color=\"{chickenColor}\">�츮 ��� �� �� �־�!!</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"���� ���̾�?", 0.5f, true); // ���� �ִϸ��̼� ���� Ʈ�� ����
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
        }
        else
        {
            ShowText($"<color=\"{godnessColor}\">���, ���� �� �Գ�?</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">���� �Ƽ���...?</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">��...\n���� ����� ���ϴ±���..</color>", 1f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">..���� ���� ģ������ ��� �׾����...\nģ������ ���� �;��...</color>", 2.25f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">�̹����� ��� �׾��� ������...</color>", 1.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">�̹����� ��Ͽ�..?</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">�� �ƴϾ�.. ģ������ ����ʹ�?</color>", 1.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">��...</color>", 0.5f, false, () =>
            {
                ending_clockBG.DOFade(1f, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">�˾Ҿ�. ���ݺ��� �ð��� �ǵ�������.</color>", 1f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">...!</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{godnessColor}\">������� ��� �־��� �ð����� �ǵ�������.\n���� �ȹٷ� ������ ����� ���� ��!</color>", 2.5f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">���ƾƾƾ�!</color>", 0.5f, false, () =>
            {
                ending_clockBG_warpEnter.SetActive(true);
                ending_clockBG.DOFade(0f, 1);
                ending_farmBG.DOFade(1f, 1);
            });
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">(�޻��޻�)</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">������..</color>", 0.5f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"� �Ͼ {chickenCallNames[chickenIndex]}! � ������ ��������!", 2f, true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"<color=\"{chickenColor}\">�´�! � �� �غ� �ؾ���!</color>", 1.25f, false);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            ShowText($"��! ���� ���� ������ ������!!", 1f, false);
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