using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ChickenSelectScript : MonoBehaviour
{
    private bool isPanelShow = false;
    private int curAbility = 0;

    public GameObject[] Stage = null;

    private string[] chicken;
    public int[] livingChicken;
    private int curChapter;

    [SerializeField] Transform[] chickens;
    [Header("��������Ʈ ������ �ʹ� ����")]
    [SerializeField] Sprite[] chickens_0BtnSprites;
    [SerializeField] Sprite[] chickens_38BtnSprites;
    [SerializeField] Sprite[] chickens_15BtnSprites;
    [SerializeField] Sprite[] chickens_30BtnSprites;
    [SerializeField] Sprite[] chickens_35BtnSprites;
    [SerializeField] Sprite[] chickens_29BtnSprites;
    [SerializeField] Sprite[] chickens_1BtnSprites;
    [SerializeField] Sprite[] chickens_26BtnSprites;
    [SerializeField] Sprite[] chickenSprites;
    [SerializeField] Sprite[] abilityIconSprites;
    [Header("�ɷ� ���� �ǳڰ���")]
    [SerializeField] Transform abilityPanel;
    [SerializeField] Image playerSprite;
    [SerializeField] Image abilityIcon;
    [SerializeField] Image blackBG;
    [SerializeField] Image selectTxt;
    [SerializeField] Image blockTouchPanel;
    [SerializeField] Text playerName;
    [SerializeField] Text playerAbilityName;
    [SerializeField] Text playerAbilityExplain;

    private string[] chickenName = new string[5] { "�����", "�����", "�۷���", "����", "������" };

    private string[,] abilityExplain = new string[2,5] {
        {
            "��ũ~!"   ,
            "�̷� ����" ,
            "�ð� ��ź" ,
            "�ð� ����",
            "�����̵�"
        },
        {
            "����̴� ��ũ�� �Ͽ� �Ϳ��� ǥ���� ���� �� �ֽ��ϴ�!!",
            "����̴� �̷��� �Ͼ �ϵ��� �̸� ü���մϴ�. 15�� ���� �̷����� �Ͼ �ϵ��� �̸� �ް� ����� ���ƿͼ� ������ �����մϴ�.",
            "�۷��̴� �ڽ��� �ð��� ���ӽ��Ѽ� ���� �ӵ��� �̵� �ӵ��� ũ�� ������ŵ�ϴ�. ",
            "����� �ð� ��ź�� ���� �� �ֽ��ϴ�.\n �ֺ� ��ü�� �ð��� ����Ʈ���� �� ��ź�� ������ �����ϴµ��� ȿ�����Դϴ�.",
            "�����̴� �����̵��� �մϴ�.\n �ߵ��� �ð��� �������� ��ư�� �巡���Ͽ� �̵��� ������ ���̽�ƽ���� �����մϴ�."
        }
    };
    private float defaultAbilityPanelPosX;
    private float movedAbilityPanelPosX;
    void Start()
    {
        defaultAbilityPanelPosX = abilityPanel.position.x;
        movedAbilityPanelPosX = defaultAbilityPanelPosX - 11f;
        chicken = SecurityPlayerPrefs.GetString("inGame.remainChicken", "0 1 2 3 4").Split(' ');
        //print(chicken.Length);
        livingChicken = new int[chicken.Length];
        for (int i = 0; i < chicken.Length; i++)
        {
            livingChicken[i] = int.Parse(chicken[i]);
        }
        curChapter = SceneController.targetMap/3;

        SetSprite(curChapter);
        Stage[curChapter].SetActive(true);
        StartCoroutine(SelectReady());
    }

    public void SetSprite(int curStage)
    {
        switch(curStage)
        {
            case 0:
                chickens[curStage].GetChild(0).GetComponent<Image>().sprite = chickens_0BtnSprites[livingChicken[0]];
                chickens[curStage].GetChild(1).GetComponent<Image>().sprite = chickens_38BtnSprites[livingChicken[1]];
                chickens[curStage].GetChild(2).GetComponent<Image>().sprite = chickens_0BtnSprites[livingChicken[2]];
                chickens[curStage].GetChild(3).GetComponent<Image>().sprite = chickens_30BtnSprites[livingChicken[3]];
                chickens[curStage].GetChild(4).GetComponent<Image>().sprite = chickens_15BtnSprites[livingChicken[4]];
                break;
            case 1:
                chickens[curStage].GetChild(0).GetComponent<Image>().sprite = chickens_0BtnSprites[livingChicken[0]];
                chickens[curStage].GetChild(1).GetComponent<Image>().sprite = chickens_0BtnSprites[livingChicken[1]];
                chickens[curStage].GetChild(2).GetComponent<Image>().sprite = chickens_35BtnSprites[livingChicken[2]];
                chickens[curStage].GetChild(3).GetComponent<Image>().sprite = chickens_1BtnSprites[livingChicken[3]];
                break;
            case 2:
                chickens[curStage].GetChild(0).GetComponent<Image>().sprite = chickens_0BtnSprites[livingChicken[0]];
                chickens[curStage].GetChild(1).GetComponent<Image>().sprite = chickens_29BtnSprites[livingChicken[1]];
                chickens[curStage].GetChild(2).GetComponent<Image>().sprite = chickens_26BtnSprites[livingChicken[2]];
                break;
            case 3:
                chickens[curStage].GetChild(0).GetComponent<Image>().sprite = chickens_0BtnSprites[livingChicken[0]];
                chickens[curStage].GetChild(1).GetComponent<Image>().sprite = chickens_15BtnSprites[livingChicken[1]];
                break;
            case 4:
                chickens[curStage].GetChild(0).GetComponent<Image>().sprite = chickens_0BtnSprites[livingChicken[0]];
                break;
        }

        blackBG.gameObject.SetActive(true);
        selectTxt.gameObject.SetActive(true);
    }

    public void StartPanel(int abilityNum)
    {
        if (abilityNum != -1)
        {
            if (isPanelShow) return;
            curAbility = livingChicken[abilityNum];
            playerSprite.sprite = chickenSprites[livingChicken[abilityNum]];
            abilityIcon.sprite = abilityIconSprites[livingChicken[abilityNum]];
            playerName.text = chickenName[livingChicken[abilityNum]];
            playerAbilityName.text = abilityExplain[0, livingChicken[abilityNum]];
            playerAbilityExplain.text = abilityExplain[1, livingChicken[abilityNum]];
            abilityPanel.DOKill();
            abilityPanel.DOMoveX(movedAbilityPanelPosX, 1f).SetEase(Ease.OutBounce);
            isPanelShow = true;
        }
        else
        {
            if (!isPanelShow) return;
            abilityPanel.DOKill();
            abilityPanel.DOMoveX(defaultAbilityPanelPosX, 1f).SetEase(Ease.OutBounce);
            isPanelShow = false;
        }
    }

    IEnumerator SelectReady()
    {
        blackBG.gameObject.SetActive(true);
        blackBG.color = new Color(0, 0, 0, 0);
        selectTxt.gameObject.SetActive(true);
        selectTxt.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(3);

        blackBG.DOFade(190 / 255f, 0.5f);
        selectTxt.DOFade(1, 0.5f).SetDelay(1).OnComplete(() =>
        {
            selectTxt.transform.DOMoveY(-0.3f, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative();
            blockTouchPanel.raycastTarget = false;
        });
    }

    //���۹�ư���� ����
    public void GameStart()
    {
        SceneController.currentChickenIndex = curAbility;
        SceneController.LoadScene("Title");
    }
}
