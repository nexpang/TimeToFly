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
        for (int i = 0; i < isBuyContents.Length; i++)
        {
            isBuyContents[i] = false;
        }
        //TODO ±¸¸ÅÇÑ »óÇ° ¹Þ¾Æ¿Í¼­ Àû¿ëÇÏ±â À§¿¡²¨ Áö¿ì¼À

        for (int i = 0; i < isBuyContents.Length; i++)
        {
            if(isBuyContents[i] == false)
            {
                ChangeAlreadyBuy(Contents[i].transform.GetChild(0));
                Contents[i].interactable = false;
            }
        }
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
        print("±¤°í Á¦°Å »ï");
        //¸¸µå¼À
    }

    public void BuyCostumePack()
    {
        isBuyContents[1] = true;
        Contents[1].interactable = false;
        ChangeAlreadyBuy(Contents[1].transform.GetChild(0));
        print("ÄÚ½ºÆ¬ ¼¼Æ® »ï");
    }

    private void ChangeAlreadyBuy(Transform child)
    {
        child.GetComponent<RectTransform>().sizeDelta = alreadyBuySize;
        child.GetComponent<Image>().sprite = alreadyBuySpr;
    }
}
