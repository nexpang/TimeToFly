using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Effect_WarningBox : Effect
{
    private RectTransform rectTransform;
    private Image image;

    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    protected override void OnEnable()
    {
        
    }

    public void Create(Vector2 position, Vector2 size, float waitTime, float signSize = 150)
    {
        transform.position = position;
        rectTransform.sizeDelta = size;

        image.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(signSize, signSize);

        lifeWait = new WaitForSeconds(waitTime);


        StartCoroutine(LifeTime());
    }

    public void Create(Vector2 position, Vector2 size, float waitTime, Color startColor, Color endColor, float colorInterval, float signSize = 150)
    {
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;
        image.color = startColor;
        image.transform.GetChild(0).GetComponent<Image>().color = startColor;

        image.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(signSize, signSize);

        image.DOColor(endColor, colorInterval).SetLoops(-1, LoopType.Yoyo);
        image.transform.GetChild(0).GetComponent<Image>().DOColor(endColor, colorInterval).SetLoops(-1, LoopType.Yoyo);

        lifeWait = new WaitForSeconds(waitTime);

        StartCoroutine(LifeTime());
    }

    protected override IEnumerator LifeTime()
    {
        yield return lifeWait;

        image.DOKill();
        image.transform.GetChild(0).GetComponent<Image>().DOKill();
        gameObject.SetActive(false);
    }
}
