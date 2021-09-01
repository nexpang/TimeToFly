using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EMungBak : MonoBehaviour
{
    public GameObject[] Stage = null;

    private char[] chicken;
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
    [SerializeField] Sprite[] chickens_26BtnSprites;
    [SerializeField] Sprite[] chickenSprites;
    [SerializeField] Sprite[] abilityIconSprites;
    [Header("�ɷ� ���� �ǳڰ���")]
    [SerializeField] Transform abilityPanel;
    [SerializeField] Image playerSprite;
    [SerializeField] Image abilityIcon;
    [SerializeField] Text playerAbilityName;
    [SerializeField] Text playerAbilityExplain;
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
            "����̴� �̷��� �Ͼ �ϵ��� �̸� ü���մϴ�.15�� ���� �̷����� �Ͼ �ϵ��� �̸� �ް� ����� ���ƿͼ� ������ �����մϴ�.",
            "�۷��̴� �ڽ��� �ð��� ���ӽ��Ѽ� ���� �ӵ��� �̵� �ӵ��� ũ�� ������ŵ�ϴ�. ",
            "����� �ð� ��ź�� ���� �� �ֽ��ϴ�.\n �ֺ� ��ü�� �ð��� ����Ʈ���� �� ��ź�� ������ �����ϴµ��� ȿ�����Դϴ�.",
            "�����̴� �����̵��� �մϴ�.\n ��� ���, �ð��� �������� �̵��� ������ ���̽�ƽ���� �����մϴ�."
        }
    };
    private float defaultAbilityPanelPosX;
    void Start()
    {
        defaultAbilityPanelPosX = abilityPanel.position.x;
        chicken = SecurityPlayerPrefs.GetString("inGame.remainChicken", "0 1 2 3 4").Replace(" ", "").ToCharArray();
        //print(chicken.Length);
        livingChicken = new int[chicken.Length];
        for (int i = 0; i < chicken.Length; i++)
        {
            livingChicken[i] = chicken[i] - '0';
        }
        curChapter = SceneController.targetMap/3;

        SetSprite(curChapter);
        Stage[curChapter].SetActive(true);
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
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    public void StartPanel(int abilityNum=-1)
    {
        playerSprite.sprite = chickenSprites[livingChicken[abilityNum]];
        abilityIcon.sprite = abilityIconSprites[livingChicken[abilityNum]];
        playerAbilityName.text = abilityExplain[0, livingChicken[abilityNum]];
        playerAbilityExplain.text = abilityExplain[1, livingChicken[abilityNum]];
        abilityPanel.DOKill();
        if (abilityNum != -1)
        {
            abilityPanel.DOMoveX(abilityPanel.position.x-11f,1f);
        }
        else
        {
            abilityPanel.DOMoveX(defaultAbilityPanelPosX, 1f);
        }
    }

    //���۹�ư���� ����
    public void GameStart(int abilityNum = -1)
    {
        if(abilityNum!=-1)
        {
            print("�ƴ� �����Ƽ ���� �����϶��");
        }
        else
        {
            //�����Ƽ�����ϰ� ����
        }
    }

    void Update()
    {

    }
}
