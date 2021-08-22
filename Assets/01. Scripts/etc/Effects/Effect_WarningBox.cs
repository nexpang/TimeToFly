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

    public void Create(Vector2 position, Vector2 size, float waitTime)
    {
        transform.position = position;
        rectTransform.sizeDelta = size;

        lifeWait = new WaitForSeconds(waitTime);


        StartCoroutine(LifeTime());
    }

    public void Create(Vector2 position, Vector2 size, float waitTime, Color startColor, Color endColor, float colorInterval)
    {
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;
        image.color = startColor;

        image.DOColor(endColor, colorInterval).SetLoops(-1, LoopType.Yoyo);

        lifeWait = new WaitForSeconds(waitTime);

        StartCoroutine(LifeTime());
    }

    protected override IEnumerator LifeTime()
    {
        yield return lifeWait;

        image.DOKill();
        gameObject.SetActive(false);
    }
}
