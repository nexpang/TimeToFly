using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ChickenSelectScript : MonoBehaviour
{
    private bool isPanelShow = false;
    private int curAbility = 0;

    public AudioSource BGMSource;
    private AudioSource SFXSource = null;

    public GameObject[] Stage = null;

    private string[] chicken;
    public int[] livingChicken;
    private int curChapter;

    private int[] randChicken;
    private int[] randChickenIdx;
    private int[] randTalk;

    private bool canSkipTalk = false;

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
    private string[,] deadTalking = new string[5, 6];
    private float defaultAbilityPanelPosX;
    private float movedAbilityPanelPosX;
    private void Awake()
    {
        deadTalking = new string[5, 6] {
        {
            "���� �ɷ¸� �־��..",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]} ������� �˾�?",
            "...?",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� �츮 �����ӿ� �����־�...",
            "",
            "���� �� �ɷ��� ����?"
        },
        {
            "���� �̷��� �ô����...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� ���� ģ������...",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� �츱 ���� ����߾�...",
            "��ũ�� �Ϳ��� ģ�����µ�...",
            "���� �̸� Ȯ���ϰ� ������!!"
        },
        {
            "���� ���� �� ��������...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� �����?",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� �츱 ���� ����߾�...",
            "�ڸ� �� Ȯ�� �ϸ鼭 ����� �ߴµ�...",
            "������ �����ڱ�!!"
        },
        {
            "�׶� �μ��� ����� �ߴµ�...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� ���� ģ������...",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� �츮 �����ӿ� �����־�...",
            "�� �ɷ��� ��������� �������..",
            "��ֹ��� ������ �±��!!"
        },
        {
            "���� ���� �� ��������...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� �����?",
            "...",
            $"{chickenName[SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)]}�� �츱 ���� ����߾�...",
            "���� �����̵� �� �� �־�����...",
            "���� �ϴ��� ������!!"
        }
        };
    }
    void Start()
    {
        Time.timeScale = 1;
        SFXSource = GetComponent<AudioSource>();
        SFXSource.mute = !SecurityPlayerPrefs.GetBool("inGame.SFX", true);
        BGMSource.mute = !SecurityPlayerPrefs.GetBool("inGame.BGM", true);


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
            //selectImgs.transform.GetChild(i).rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        //if(curStage == 0)
        //{
        //    selectImgs.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, 0f);
        //}
        //else if( curStage != 4)
        //{
        //    selectImgs.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, 180f);
        //}

        // ����ִ� �ߵ� ����Ʈ�� ����
        List<int> randSaveList = new List<int>();

        int randCount = Random.Range(livingChicken.Length/2, (livingChicken.Length+1-livingChicken.Length/2));
        randCount = Mathf.Clamp(randCount, 1, livingChicken.Length);
        randChicken = new int[randCount];
        randChickenIdx = new int[randCount];
        for (int i = 0; i < randCount; i++)
        {
            int rand = 0;
            bool isAlreadyUse = false;
            do
            {
                isAlreadyUse = false;
                rand = Random.Range(0, livingChicken.Length);
                for (int j = 0; j < randSaveList.Count; j++)
                {
                    if (rand == randSaveList[j])
                    {
                        isAlreadyUse = true;
                        break;
                    }
                }
            } while (isAlreadyUse);
            randChicken[i] = livingChicken[rand];
            randChickenIdx[i] = rand;
            randSaveList.Add(rand);
        }

        List<int> randTalkList = new List<int>() { 0, 1, 2, 3 };
        randTalk = new int[randCount];
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
        if(curStage == 0)
        {
            bool beakSukSay =false;
            for (int i = 0; i < randChicken.Length; i++)
            {
                if(randChicken[i] == 0)
                {
                    beakSukSay = true;
                    break;
                }
            }
            if (!beakSukSay)
            {
                randChicken[0] = 0;
                randChickenIdx[0] = 0;
            }
            for (int i = 0; i < randTalk.Length; i++)
            {
                randTalk[i] = 5;
            }
        }

        for (int i = 0; i < livingChicken.Length; i++)
        {
            chickens[curStage].GetChild(i).GetChild(1).gameObject.SetActive(false);
            //chickens[curStage].GetChild(livingChicken[i]).GetChild(2).gameObject.SetActive(false);
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

        StartCoroutine(ShowTalk(randCount));
    }

    private IEnumerator ShowTalk(int randCount)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < randCount; i++)
        {
            chickens[curChapter].GetChild(randChickenIdx[i]).GetChild(1).GetChild(0).GetComponent<Text>().text = deadTalking[randChicken[i], randTalk[i]];
            chickens[curChapter].GetChild(randChickenIdx[i]).GetChild(1).gameObject.SetActive(true);
            //chickens[curStage].GetChild(randChicken[i]).GetChild(2).GetChild(0).GetComponent<Text>().text = deadTalking[randChicken[i], randTalk[i]];
            //chickens[curStage].GetChild(randChicken[i]).GetChild(2).gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        canSkipTalk = true;
    }

    public void StartPanel(int selectIdx)
    {
        if (selectIdx != -1)
        {
            if (isPanelShow) return;
            Animator ani = selectImgs.transform.GetChild(selectIdx).GetComponent<Animator>();
            ani.Play("SeletBtn_Click");
            SFXSource.Play();
            curAbility = livingChicken[selectIdx];
            playerSprite.sprite = chickenSprites[livingChicken[selectIdx]];
            abilityIcon.sprite = abilityIconSprites[livingChicken[selectIdx]];
            playerName.text = chickenName[livingChicken[selectIdx]];
            playerAbilityName.text = abilityExplain[0, livingChicken[selectIdx]];
            playerAbilityExplain.text = abilityExplain[1, livingChicken[selectIdx]];
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
        if (!canSkipTalk)
            return;
        for (int i = 0; i < livingChicken.Length; i++)
        {
            chickens[curChapter].GetChild(i).GetChild(1).gameObject.SetActive(false);
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
            chickens[curChapter].GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
            //chickens[curChapter].GetChild(livingChicken[i]).GetChild(1).gameObject.SetActive(true);
        }
        DOTween.To(() => selectImgs.alpha, x => selectImgs.alpha = x, 1f, 1f);

        StartCoroutine(ClickAnimation());
    }

    IEnumerator ClickAnimation()
    {
        print("�̰� ��?");
        yield return new WaitForSeconds(1.5f);
        DOTween.To(() => selectImgs.alpha, x => selectImgs.alpha = x, 0f, 1.5f).SetLoops(-1,LoopType.Yoyo);
        for (int i = 0; i < livingChicken.Length; i++)
        {
            CanvasGroup cvsG = chickens[curChapter].GetChild(i).GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
            DOTween.To(() => cvsG.alpha, x => cvsG.alpha = x, 0f, 1.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    //���۹�ư���� ����
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
