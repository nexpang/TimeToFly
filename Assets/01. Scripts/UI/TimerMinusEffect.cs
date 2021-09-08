using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerMinusEffect : MonoBehaviour
{
    private Text text;
    public float radius = 0.5f;
    public RectTransform parentRectTrm;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    public IEnumerator OnEffect(int usingTime, bool isMinus = true)
    {
        Color plusColor = GameManager.Instance.timerDefaultColor;
        if (isMinus)
        {
            text.text = "-" + usingTime;
            text.color = new Color(1f,0f,0f);
        }
        else
        {
            text.text = "+" + usingTime;
            text.color = plusColor;
        }
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f,-20f);
        //transform.localPosition = new Vector3(-parentRectTrm.rect.width/2, -parentRectTrm.rect.width / 2, 0f)+(new Vector3(Random.Range(-1,1), Random.Range(-1, 1), 0).normalized * 50);

        transform.GetComponent<RectTransform>().DOAnchorPosY(-120f, 1f);
        text.DOKill();
        if(isMinus)
            text.DOColor(new Color(1f, 0f, 0f, 1f), 0.3f);
        else
            text.DOColor(new Color(plusColor.r, plusColor.g, plusColor.b, 1f), 0.3f);
        yield return new WaitForSeconds(0.6f);
        if (isMinus)
            text.DOColor(new Color(1f, 0f, 0f, 0f), 0.8f);
        else
            text.DOColor(new Color(plusColor.r, plusColor.g, plusColor.b, 0f), 0.8f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
