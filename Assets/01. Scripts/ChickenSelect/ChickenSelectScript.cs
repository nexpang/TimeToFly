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

    private AudioSource audioSource = null;

    [SerializeField] Transform[] chickens;
    [Header("스프라이트 넣을거 너무 많음")]
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
    [Header("능력 설명 판넬관련")]
    [SerializeField] CanvasGroup selectImgs;
    [SerializeField] Transform abilityPanel;
    [SerializeField] Image playerSprite;
    [SerializeField] Image abilityIcon;
    [SerializeField] Image blackBG;
    [SerializeField] Image selectTxt;
    [SerializeField] Image blockTouchPanel;
    [SerializeField] Text playerName;
    [SerializeField] Text playerAbilityName;
    [SerializeField] Text playerAbilityExplain;

    private string[] chickenName = new string[5] { "백숙이", "토닭이", "퍼렁이", "딸기", "태일이" };

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
            "토닭이는 미래에 일어날 일들을 미리 체험합니다. 15초 동안 미래에서 일어날 일들을 미리 겪고 현재로 돌아와서 함정을 간파합니다.",
            "퍼렁이는 자신의 시간을 가속시켜서 점프 속도와 이동 속도를 크게 증가시킵니다. ",
            "딸기는 시간 폭탄을 던질 수 있습니다.\n 주변 물체의 시간을 망가트리는 이 폭탄은 함정을 제거하는데에 효과적입니다.",
            "태일이는 순간이동을 합니다.\n 발동시 시간이 느려지며 버튼을 드래그하여 이동할 지점을 조이스틱으로 선택합니다."
        }
    };
    private string[,] deadTalking;
    private float defaultAbilityPanelPosX;
    private float movedAbilityPanelPosX;
    private void Awake()
    {
        deadTalking = new string[5, 5] {
        {
            "나는 왜 능력이 없지?",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]} 어딨는지 알아?",
            "...?",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}는 우리 마음속에 남아있어...",
            ""
        },
        {
            "내가 미래를 봤더라면...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}는 좋은 친구였어...",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}는 우릴 위해 희생했어...",
            "윙크가 귀여운 친구였는데..."
        },
        {
            "내가 좀만 더 빨랐더라도...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}가 어디갔지?",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}는 우릴 위해 희생했어...",
            "뒤를 잘 확인 하면서 갔어야 했는데..."
        },
        {
            "그때 부수고 갔어야 했는데...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}는 좋은 친구였어...",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}는 우리 마음속에 남아있어...",
            "내 능력이 백숙이한테 갔더라면.."
        },
        {
            "내가 좀만 더 빨랐더라도...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}가 어디갔지?",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}는 우릴 위해 희생했어...",
            "같이 순간이동 할 수 있었으면..."
        }
    };
    }
    void Start()
    {
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
        defaultAbilityPanelPosX = abilityPanel.position.x;
        movedAbilityPanelPosX = defaultAbilityPanelPosX - 11f;
        chicken = SecurityPlayerPrefs.GetString("inGame.remainChicken", "0 1 2 3 4").Split(' ');
        //print(chicken.Length);
        livingChicken = new int[chicken.Length];
        for (int i = 0; i < chicken.Length; i++)
        {
            livingChicken[i] = int.Parse(chicken[i]);
        }
        curChapter = SceneController.targetMapId / 3;

        SetSprite(curChapter);
        Stage[curChapter].SetActive(true);
        //StartCoroutine(SelectReady());
    }

    public void SetSprite(int curStage)
    {
        for (int i = 0; i < livingChicken.Length; i++)
        {
            selectImgs.transform.GetChild(i).gameObject.SetActive(true);
            selectImgs.transform.GetChild(i).position = chickens[curStage].GetChild(i).GetChild(0).position;
            selectImgs.transform.GetChild(i).rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        if(curStage == 0)
        {
            selectImgs.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if( curStage != 4)
        {
            selectImgs.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        // 살아있는 닭들 리스트로 만듬
        List<int> livingChickenRandList = new List<int>();

        for (int i = 0; i < livingChicken.Length; i++)
        {
            livingChickenRandList.Add(livingChicken[i]);
        }
        int randCount = Random.Range(livingChicken.Length/2, (livingChicken.Length+1-livingChicken.Length/2));
        randCount = Mathf.Clamp(randCount, 1, livingChicken.Length);
        int[] randChicken = new int[randCount];
        int[] randChickenIdx = new int[randCount];
        for (int i = 0; i < randCount; i++)
        {
            int rand = Random.Range(0, livingChickenRandList.Count);
            randChicken[i] = livingChickenRandList[rand];
            randChickenIdx[i] = rand;
            livingChickenRandList.RemoveAt(rand);
        }

        List<int> randTalkList = new List<int>() { 0, 1, 2, 3 };
        int[] randTalk = new int[randCount];
        for (int i = 0; i < randCount; i++)
        {
            int rand = Random.Range(0, randTalkList.Count);
            randTalk[i] = randTalkList[rand];
            randTalkList.RemoveAt(rand);
        }

        if(SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1) == 0)
        {
            randTalk[0] = 4;
        }

        for (int i = 0; i < livingChicken.Length; i++)
        {
            chickens[curStage].GetChild(i).GetChild(2).gameObject.SetActive(false);
            //chickens[curStage].GetChild(livingChicken[i]).GetChild(2).gameObject.SetActive(false);
        }

        for (int i = 0; i < randCount; i++)
        {
            chickens[curStage].GetChild(randChickenIdx[i]).GetChild(2).GetChild(0).GetComponent<Text>().text = deadTalking[randChicken[i], randTalk[i]];
            chickens[curStage].GetChild(randChickenIdx[i]).GetChild(2).gameObject.SetActive(true);
            //chickens[curStage].GetChild(randChicken[i]).GetChild(2).GetChild(0).GetComponent<Text>().text = deadTalking[randChicken[i], randTalk[i]];
            //chickens[curStage].GetChild(randChicken[i]).GetChild(2).gameObject.SetActive(true);
        }

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
    }

    public void StartPanel(int abilityNum)
    {
        if (abilityNum != -1)
        {
            if (isPanelShow) return;
            audioSource.Play();
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

    public void SelectReadyBtn()
    {
        for (int i = 0; i < livingChicken.Length; i++)
        {
            chickens[curChapter].GetChild(i).GetChild(2).gameObject.SetActive(false);
        }
        StartCoroutine(SelectReady());
    }

    IEnumerator SelectReady()
    {
        blackBG.gameObject.SetActive(true);
        blackBG.color = new Color(0, 0, 0, 0);
        selectTxt.gameObject.SetActive(true);
        selectTxt.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(3);

        blackBG.DOFade(190 / 255f, 0.5f);
        selectTxt.DOFade(1, 0.5f).SetDelay(0.5f).OnComplete(() =>
        {
            selectTxt.transform.DOMoveY(-0.3f, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative();
            blockTouchPanel.raycastTarget = false;
        });

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < livingChicken.Length; i++)
        {
            chickens[curChapter].GetChild(i).GetChild(1).gameObject.SetActive(true);
            //chickens[curChapter].GetChild(livingChicken[i]).GetChild(1).gameObject.SetActive(true);
        }
        DOTween.To(() => selectImgs.alpha, x => selectImgs.alpha = x, 1f, 1f);
    }

    //시작버튼으로 실행
    public void GameStart()
    {
        SecurityPlayerPrefs.SetInt("inGame.saveCurrentChickenIndex", curAbility);

        if(SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0) == 0)
        {
            SceneController.targetMapId = 1;
            SecurityPlayerPrefs.SetInt("inGame.saveMapid", 1);
        }
        SceneController.LoadScene("InGame");
    }
}
