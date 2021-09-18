using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public bool isOpen = false;
    private RectTransform rT = null;
    private float defaultXPos = 0;

    [SerializeField] private Transform ContentsParent = null;
    private bool[] isBuyContents;
    private Button[] Contents = new Button[2];
    [SerializeField] private Sprite alreadyBuySpr = null;
    private Vector2 alreadyBuySize = new Vector2(393, 166);

    private void Awake()
    {
        rT = GetComponent<RectTransform>();
    }
    void Start()
    {
        defaultXPos = rT.anchoredPosition.x;
        Contents = ContentsParent.GetComponentsInChildren<Button>();
        isBuyContents = new bool[Contents.Length];
        //TODO 구매한 상품 받아와서 적용하기
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShop(bool open)
    {
        if (isOpen == open) return;
        isOpen = open;
        if(open)
        {
            GetComponent<Button>().interactable = false;
            rT.DOKill();
            rT.DOAnchorPosX(-500f, 1f).SetUpdate(true);
        }
        else
        {
            rT.DOKill();
            rT.DOAnchorPosX(defaultXPos, 1f).SetUpdate(true).OnComplete(()=> { GetComponent<Button>().interactable = true; });
        }
    }

    public void ADRemoved()
    {
        isBuyContents[0] = true;
        Contents[0].interactable = false;
        ChangeAlreadyBuy(Contents[0].transform.GetChild(0));
        print("광고 제거 삼");
        //만드셈
    }

    public void BuyCostumePack()
    {
        isBuyContents[1] = true;
        Contents[1].interactable = false;
        ChangeAlreadyBuy(Contents[1].transform.GetChild(0));
        print("코스튬 세트 삼");
    }

    private void ChangeAlreadyBuy(Transform child)
    {
        child.GetComponent<RectTransform>().sizeDelta = alreadyBuySize;
        child.GetComponent<Image>().sprite = alreadyBuySpr;
    }
}
