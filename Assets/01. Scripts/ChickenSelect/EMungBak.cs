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
    [SerializeField] Sprite[] chickenSprites;
    [SerializeField] Sprite[] abilityIconSprites;
    [SerializeField] Transform abilityPanel;
    [SerializeField] Image playerSprite;
    [SerializeField] Image abilityIcon;
    [SerializeField] Text playerAbilityName;
    [SerializeField] Text playerAbilityExplain;
    private string[,] abilityExplain = new string[2,5] {
        {
            "윙크~!"   ,
            "미래 예지" ,
            "시간 폭탄" ,
            "시간 가속",
            "순간이동"
        },
        {
            "백숙이는 윙크를 하여 귀여운 표정을 지을 수 있습니다!!",
            "토닭이는 미래에 일어날 일들을 미리 체험합니다.15초 동안 미래에서 일어날 일들을 미리 겪고 현재로 돌아와서 함정을 간파합니다.",
            "퍼렁이는 자신의 시간을 가속시켜서 점프 속도와 이동 속도를 크게 증가시킵니다. ",
            "딸기는 시간 폭탄을 던질 수 있습니다.\n 주변 물체의 시간을 망가트리는 이 폭탄은 함정을 제거하는데에 효과적입니다.",
            "태일이는 순간이동을 합니다.\n 사용 즉시, 시간이 느려지며 이동할 지점을 조이스틱으로 선택합니다."
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

        Stage[curChapter].SetActive(true);
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

    //시작버튼으로 실행
    public void GameStart(int abilityNum = -1)
    {
        if(abilityNum!=-1)
        {
            print("아니 어빌리티 숫자 설정하라고");
        }
        else
        {
            //어빌리티설정하고 시작
        }
    }

    void Update()
    {

    }
}
