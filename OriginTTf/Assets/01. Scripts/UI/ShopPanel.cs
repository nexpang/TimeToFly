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

    private void Awake()
    {
        rT = GetComponent<RectTransform>();
    }
    void Start()
    {
        defaultXPos = rT.anchoredPosition.x;
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
}
